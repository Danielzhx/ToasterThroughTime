using System;
using UnityEngine;
using System.Collections;
using AudioManger;
using UnityEngine.UI;


namespace TarodevController
{
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// I have a premium version on Patreon, which has every feature you'd expect from a polished controller. Link: https://www.patreon.com/tarodev
    /// You can play and compete for best times here: https://tarodev.itch.io/extended-ultimate-2d-controller
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/tarodev
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private float _time;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        private SpriteRenderer _sr;
        private Animator _anim;
        private float _idleTimer = 0f;

        [SerializeField] private ScriptableStats _stats;
        [SerializeField] private bool isInvincible = false;
        [SerializeField] private float invincibilityDuration = 1f;
        [SerializeField] private float bounceForce = 5f;
        [SerializeField] private float bounceVerticalForce = 2f;
        [SerializeField] private float idleThreshold = 5f;


        // Health Pool
        public int maxHealth = 4;  // Maximum health
        public int currentHealth;  // Current health
        public Slider healthBar;  // Reference to the health bar UI
        public bool IsInvincible => isInvincible;
        public bool isFacingRight = true;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            //Walking_Animator = GetComponent<Animator>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            _anim = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();

            // Initialize current health
            currentHealth = maxHealth;

        }

        void enableController()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            this.enabled = true;
        }

        void disableController()
        {
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.linearVelocity = Vector2.zero;
            this.enabled = false;
        }

        #region Health


        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();

            bool isStandingStill = _grounded
                       && Mathf.Abs(_frameInput.Move.x) < 0.01f
                       && Mathf.Abs(_frameInput.Move.y) < 0.01f;

            if (isStandingStill)
            {
                _idleTimer += Time.deltaTime;
            }
            else
            {
                _idleTimer = 0f;
            }

            if (_idleTimer >= idleThreshold)
            {
                _anim.SetTrigger("Bored");
                _idleTimer = 0f;  // Reset timer so it doesn’t keep triggering repeatedly
            }
        }


        // Method to handle taking damage
        public GameObject damageEffectPrefab;

        private IEnumerator BlinkHealthBar()
        {
            Color originalColor = healthBar.fillRect.GetComponent<Image>().color;
            Color blinkColor = Color.red;
            for (int i = 0; i < 6; i++)
            {
                healthBar.fillRect.GetComponent<Image>().color = blinkColor;
                yield return new WaitForSeconds(0.1f);
                healthBar.fillRect.GetComponent<Image>().color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
        }

       public void TakeDamage(int damageAmount)
        {

            AudioManager.instance.PlayCharacterDamagedSound();
            Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
            currentHealth -= damageAmount;
            healthBar.value = currentHealth;

            // Start blinking
            StartCoroutine(BlinkHealthBar());

            if (currentHealth <= 0)
            {
                AudioManager.instance.PlayCharacterDiesSound();
                currentHealth = 0;
                _anim.SetTrigger("Dying");
                _rb.linearVelocity = new Vector2(0, 0);
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _col.enabled = false;
                this.enabled = false;
                return;
            }

            _anim.SetTrigger("TakeDamage");

            float direction = _sr.flipX ? 1f : -1f;
            _frameVelocity.x = direction * bounceForce;
            _frameVelocity.y = bounceVerticalForce;

            _rb.linearVelocity = _frameVelocity;

            // Start invincibility if needed
            StartCoroutine(InvincibilityCoroutine());
        }


        // Coroutine to make the player invincible for a short period
        private IEnumerator InvincibilityCoroutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            isInvincible = false;
        }
        #endregion


        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetKeyDown(KeyCode.W),
                JumpHeld = Input.GetKey(KeyCode.W),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            ApplyMovement();
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            RaycastHit2D groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.bounds.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, _stats.GroundLayer);
            RaycastHit2D ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.bounds.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, _stats.GroundLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }
        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        public bool hasToast = false;
        public GameObject downwardProjectilePrefab;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.linearVelocity.y > 0)
            {
                _endedJumpEarly = true;
            }

            if (!_jumpToConsume && !HasBufferedJump)
            {
                return;
            }

            if (_grounded || CanUseCoyote)
            {
                ExecuteJump();
            }
            //double jump
            else if (hasToast && !_grounded)
            {
                ExecuteJump();
                _anim.SetTrigger("DoubleJump");

                if (downwardProjectilePrefab != null)
                {
                    Instantiate(downwardProjectilePrefab, transform.position, Quaternion.identity);
                }

                hasToast = false;
            }


            _jumpToConsume = false;
        }

        public void Bounce(float bounceMultiplier)
        {
            // Reset vertical velocity
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);

            // Ensure consistent double jump by resetting jump state
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;

            // Apply jump power using the multiplier
            _frameVelocity.y = _stats.JumpPower * bounceMultiplier;
            _rb.linearVelocity = _frameVelocity;

            Jumped?.Invoke();
        }


        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion


        #region Horizontal
        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }
        #endregion


        #region Gravity
        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }
        #endregion

        private void ApplyMovement()
        {
            _rb.linearVelocity = _frameVelocity;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }
    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}

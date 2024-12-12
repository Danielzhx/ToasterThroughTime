using System;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;


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
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        //private Animator Walking_Animator;

        // Health Pool
        public int maxHealth = 4;  // Maximum health
        public int currentHealth;  // Current health

        // Invincibility settings (optional to prevent quick repeated hits)
        //private bool isInvincible = false;
        private Animator _anim;
        [SerializeField] private bool isInvincible = false;
        [SerializeField] private float invincibilityDuration = 1f;
        public bool IsInvincible => isInvincible;

        // Reference to the HUD Arrow script
        public Arrow healthArrow;

        public bool isFacingRight = true;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion
        private float _time;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            //Walking_Animator = GetComponent<Animator>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            _anim = GetComponent<Animator>();

            // Initialize current health
            currentHealth = maxHealth;


            if (healthArrow != null)
            {
                healthArrow.SetHealthPosition(4 - currentHealth);
            }

        }


        #region Health

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the player collides with an enemy
            if (collision.CompareTag("Enemy") && !isInvincible && !justBounced)
            {
                TakeDamage(1);
            }
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();

            // Add code here to update the UI or animations based on current health if needed
            //if (currentHealth <= 0)
            //{
            //    Die();
            //}

        }


        // Method to handle taking damage
        public void TakeDamage(int damageAmount)
        {
            // Reduce current health by the damage amount
            currentHealth -= damageAmount;

            // Prevent health from going below zero
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _anim.SetTrigger("Dying");
                _rb.linearVelocity = new Vector2(0, 0);
                this.enabled = false;
            }


            // Update health HUD arrow position
            if (healthArrow != null)
            {
                healthArrow.SetHealthPosition(4 - currentHealth);
            }

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

        // Method to handle player death
        //private void Die()
        //{
        //  Debug.Log("Player has died.");

        // Add player death logic here (e.g., play death animation, disable player control)
        // this.enabled = false; // Disables the PlayerController script
        //}

        #endregion


        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
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

            // if (_frameInput.Move.x == 1 || _frameInput.Move.x == -1)
            // {
            //     Walking_Animator.SetTrigger("InitiateWalk");
            // }
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

        // Debugging
        if (groundHit.collider != null)
        {
            Debug.Log("Ground Detected on Layer: " + LayerMask.LayerToName(groundHit.collider.gameObject.layer));
        }
        else
        {
            Debug.Log("Ground not detected.");
        }
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
        
        public float doubleJumpMultiplier = 1.5f;
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
                // Perform Double Jump
                Bounce(doubleJumpMultiplier);
                hasToast = false; // Reset the hasToast flag to prevent multiple double jumps
            }

            _jumpToConsume = false;
        }
        private bool justBounced = false;
        [SerializeField] private Collider2D feetCollider;
        public void SetJustBounced(float delay = 0.2f)
        {
            justBounced = true;
            StartCoroutine(ResetJustBounced(delay));
        }
        private IEnumerator ResetJustBounced(float delay)
        {
            yield return new WaitForSeconds(delay);
            justBounced = false;
        }
        
        public void Bounce(float bounceMultiplier)
        {
            // Reset vertical velocity before applying bounce
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);

            // Apply jump power as a bounce using the multiplier
            _frameVelocity.y = _stats.JumpPower * bounceMultiplier;

            // Update Rigidbody velocity
            _rb.linearVelocity = _frameVelocity;

            // Optionally, trigger jump animation or effects
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


                // // Flip character direction
                // FlipCharacter(_frameInput.Move.x);

            }
        }

        // private void FlipCharacter(float direction)
        // {
        //     Vector3 scale = transform.localScale;
        //     if (direction > 0 && scale.x < 0 || direction < 0 && scale.x > 0)
        //     {
        //         scale.x *= -1; // Flip the x-scale
        //         transform.localScale = scale;
        //         // if flipped while facing right, face left and vice versa.
        //         if (isFacingRight){isFacingRight = false;}
        //         else {isFacingRight = true;}

        //     }
        // }

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
            // Walking_Animator.SetInteger("YVelocity", (int)_frameVelocity.y);
            // Walking_Animator.SetFloat("MovementVelocity", Mathf.Abs(_frameVelocity.x));
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
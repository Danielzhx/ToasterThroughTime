using UnityEngine;

namespace TTT.System 
{
    /// <summary>
    /// VERY primitive animator example.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Animator _anim;

        [SerializeField] private SpriteRenderer _sprite;

        [Header("Settings")]
        [SerializeField, Range(1f, 3f)]
        public Transform shootingPoint;
        public Animator DeathFadeAnim;

        private float _maxIdleSpeed = 2;
        private Vector2 firingPosition;
        private IPlayerController _player;
        private bool _grounded;

        private void Awake()
        {
            _player = GetComponentInParent<IPlayerController>();
            firingPosition = shootingPoint.localPosition;
        }

        private void OnEnable()
        {
            _player.Jumped += OnJumped;
            _player.GroundedChanged += OnGroundedChanged;
        }

        private void OnDisable()
        {
            _player.Jumped -= OnJumped;
            _player.GroundedChanged -= OnGroundedChanged;
        }

        private void Update()
        {
            if (_player == null) return;

            HandleSpriteFlip();
            HandleIdleSpeed();
        }

        private void HandleSpriteFlip()
        {
            if (_player.FrameInput.x != 0)
            {
                _sprite.flipX = _player.FrameInput.x < 0;
                if (_sprite.flipX)
                {
                    shootingPoint.localPosition = -firingPosition;
                }
                else
                {
                    shootingPoint.localPosition = firingPosition;
                }
            }
        }

        private void HandleIdleSpeed()
        {
            var inputStrength = Mathf.Abs(_player.FrameInput.x);
            _anim.SetFloat(IdleSpeedKey, Mathf.Lerp(1, _maxIdleSpeed, inputStrength));
        }

        private void OnJumped()
        {
            _anim.SetTrigger(JumpKey);
            _anim.ResetTrigger(GroundedKey);
        }

        private void OnGroundedChanged(bool grounded, float impact)
        {
            _grounded = grounded;

            if (grounded)
            {
                _anim.SetTrigger(GroundedKey);
            }
        }

        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int IdleSpeedKey = Animator.StringToHash("IdleSpeed");
        private static readonly int JumpKey = Animator.StringToHash("Jump");

        public void TriggerDeathFade(){
            DeathFadeAnim.SetTrigger("Death");
        }
    }
}

using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("HorizontalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private bool _faceRight;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _extraHeightTest;
        [SerializeField] private LayerMask _platformLayerMask;
        
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _touchingCollider;
        private bool _isJumping;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _touchingCollider = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            if (_isJumping)
            {
                UpdateJump();
            }
        }

        public void MoveHorizontally(float horizontalDirection)
        {
            SetHorizontalDirection(horizontalDirection);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = horizontalDirection * _horizontalSpeed;
            _rigidbody.velocity = velocity;
        }

        private void SetHorizontalDirection(float horizontalDirection)
        {
            if((_faceRight && horizontalDirection < 0) ||
               (!_faceRight && horizontalDirection > 0)) 
                Flip();
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }
        
        public void Jump()
        {
            if(_isJumping) 
                return;
            
            _isJumping = true;
            _rigidbody.AddForce(Vector2.up * _jumpForce);
        }

        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && IsGrounded())
                ResetJump();
        }

        private bool IsGrounded()
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(
                _touchingCollider.bounds.center, 
                _touchingCollider.bounds.size,
                0f, 
                Vector2.down, 
                _extraHeightTest, 
                _platformLayerMask);
            return raycastHit.collider != null;
        }

        private void ResetJump()
        {
            _isJumping = false;
        }
    }
}
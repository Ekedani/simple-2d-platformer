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
        
        private Rigidbody2D _rigidbody;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
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
            throw new System.NotImplementedException();
        }
    }
}
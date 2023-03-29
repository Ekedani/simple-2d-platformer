using Core.Enums;
using Core.Movement.Data;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class HorizontalMover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly HorizontalMovementData _horizontalMovementData;
        
        private Vector2 _movement;

        public Direction Direction { get; private set; }

        public bool IsMoving => _movement.magnitude > 0;

        public HorizontalMover(Rigidbody2D rigidbody, HorizontalMovementData horizontalMovementData) 
        {
            _rigidbody = rigidbody;
            _transform = rigidbody.transform;
            _horizontalMovementData = horizontalMovementData;
        }
        
        public void MoveHorizontally(float horizontalDirection)
        {
            _movement.x = horizontalDirection;
            SetHorizontalDirection(horizontalDirection);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = horizontalDirection * _horizontalMovementData.HorizontalSpeed;
            _rigidbody.velocity = velocity;
        }

        private void SetHorizontalDirection(float horizontalDirection)
        {
            if((Direction == Direction.Right && horizontalDirection < 0) ||
               (Direction == Direction.Left && horizontalDirection > 0)) 
                Flip();
        }

        private void Flip()
        {
            _transform.Rotate(0, 180, 0);
            Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
        }
    }
}
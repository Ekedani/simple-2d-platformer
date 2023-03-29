using System;
using Core.Animation;
using Core.Enums;
using Core.Tools;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("HorizontalMovement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private Direction _direction;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _extraHeightTest;
        [SerializeField] private LayerMask _platformLayerMask;

        [SerializeField] private DirectionalCameraPair _cameras;
        [SerializeField] private AnimatorController _animator;
        
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _touchingCollider;
        private bool _isJumping;
        private Vector2 _movement;
        
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
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _movement.magnitude > 0);
            _animator.PlayAnimation(AnimationType.Jump, _isJumping);
        }

        public void MoveHorizontally(float horizontalDirection)
        {
            _movement.x = horizontalDirection;
            SetHorizontalDirection(horizontalDirection);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = horizontalDirection * _horizontalSpeed;
            _rigidbody.velocity = velocity;
        }

        private void SetHorizontalDirection(float horizontalDirection)
        {
            if((_direction == Direction.Right && horizontalDirection < 0) ||
               (_direction == Direction.Left && horizontalDirection > 0)) 
                Flip();
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _direction;
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
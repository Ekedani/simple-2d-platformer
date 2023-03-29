using System;
using Core.Animation;
using Core.Enums;
using Core.Movement.Controller;
using Core.Movement.Data;
using Core.Tools;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private HorizontalMovementData _horizontalMovementData;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _extraHeightTest;
        [SerializeField] private LayerMask _platformLayerMask;

        [SerializeField] private DirectionalCameraPair _cameras;
        [SerializeField] private AnimatorController _animator;
        
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _touchingCollider;
        private bool _isJumping;

        private HorizontalMover _horizontalMover;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _touchingCollider = GetComponent<CapsuleCollider2D>();
            _horizontalMover = new HorizontalMover(_rigidbody, _horizontalMovementData);
        }

        private void Update()
        {
            if (_isJumping)
            {
                UpdateJump();
            }
            UpdateAnimations();
            UpdateCameras();
        }

        private void UpdateCameras()
        {
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _horizontalMover.Direction;
        }

        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _horizontalMover.IsMoving);
            _animator.PlayAnimation(AnimationType.Jump, _isJumping);
        }

        public void MoveHorizontally(float horizontalDirection) =>
            _horizontalMover.MoveHorizontally(horizontalDirection);
        
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
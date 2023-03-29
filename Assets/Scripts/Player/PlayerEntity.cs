using Core.Animation;
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
        [SerializeField] private JumpData _jumpData;
        [SerializeField] private DirectionalCameraPair _cameras;
        [SerializeField] private AnimatorController _animator;
        
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _touchingCollider;

        private HorizontalMover _horizontalMover;
        private Jumper _jumper;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _touchingCollider = GetComponent<CapsuleCollider2D>();
            _horizontalMover = new HorizontalMover(_rigidbody, _horizontalMovementData);
            _jumper = new Jumper(_rigidbody, _touchingCollider, _jumpData);
        }

        private void Update()
        {
            if (_jumper.IsJumping)
            {
                _jumper.UpdateJump();
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
            _animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);
        }

        public void MoveHorizontally(float horizontalDirection) =>
            _horizontalMover.MoveHorizontally(horizontalDirection);

        public void Jump() => _jumper.Jump();
    }
}
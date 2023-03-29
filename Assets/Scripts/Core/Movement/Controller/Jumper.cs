using Core.Movement.Data;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Collider2D _touchingCollider;
        private readonly JumpData _jumpData;
        private readonly IStatValueGiver _statValueGiver;
        public bool IsJumping { get; private set; }

        public Jumper(
            Rigidbody2D rigidbody,
            Collider2D touchingCollider,
            JumpData jumpData,
            IStatValueGiver statValueGiver
        )
        {
            _rigidbody = rigidbody;
            _touchingCollider = touchingCollider;
            _jumpData = jumpData;
            _statValueGiver = statValueGiver;
        }

        public void Jump()
        {
            if (IsJumping)
                return;

            IsJumping = true;
            _rigidbody.AddForce(Vector2.up * _statValueGiver.GetStatValue(StatType.JumpForce));
        }

        public void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && IsGrounded())
                ResetJump();
        }

        private bool IsGrounded()
        {
            var bounds = _touchingCollider.bounds;
            RaycastHit2D raycastHit = Physics2D.BoxCast(
                bounds.center,
                bounds.size,
                0f,
                Vector2.down,
                _jumpData.ExtraHeightTest,
                _jumpData.PlatformLayerMask);
            return raycastHit.collider != null;
        }

        private void ResetJump()
        {
            IsJumping = false;
        }
    }
}
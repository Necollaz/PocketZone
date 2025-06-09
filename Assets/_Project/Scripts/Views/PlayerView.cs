using System;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Data;

namespace _Project.Scripts.Views
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform _graphicsRoot;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Canvas _healthCanvas;

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Vector3 _initialEuler;

        private float _currentFacing = 1f;

        public event Action AttackPressed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            if (_graphicsRoot == null)
                _graphicsRoot = transform;

            _initialEuler = _graphicsRoot.localEulerAngles;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }

        public void SetWalkDirection(bool isWalking, float lastDirectionX)
        {
            SetFacing(lastDirectionX);

            SetDirectionalBool(isWalking, lastDirectionX, CharacterAnimationParams.WalkingRightPlayer, CharacterAnimationParams.WalkingLeftPlayer);
        }

        public void PlayAttack(float lastDirectionX)
        {
            SetFacing(lastDirectionX);

            SetDirectionalTrigger(lastDirectionX, CharacterAnimationParams.AttackRight, CharacterAnimationParams.AttackLeft);
        }

        public void OnAttackButtonPressed()
        {
            AttackPressed?.Invoke();
        }

        private void SetFacing(float directionX)
        {
            if (Mathf.Approximately(directionX, _currentFacing))
                return;

            _currentFacing = directionX;
            _animator.SetFloat(CharacterAnimationParams.IdleDirection, directionX);

            float addY = directionX < 0f ? 180f : 0f;
            Vector3 euler = _initialEuler;
            _graphicsRoot.localEulerAngles = new Vector3(euler.x, euler.y + addY, euler.z);
        }

        private void SetDirectionalBool(bool isActive, float directionX, int paramRight, int paramLeft)
        {
            _animator.SetBool(paramRight, false);
            _animator.SetBool(paramLeft, false);

            if (!isActive)
                return;

            if (directionX >= 0f)
                _animator.SetBool(paramRight, true);
            else
                _animator.SetBool(paramLeft, true);
        }

        private void SetDirectionalTrigger(float directionX, int triggerRight, int triggerLeft)
        {
            _animator.ResetTrigger(triggerRight);
            _animator.ResetTrigger(triggerLeft);

            if (directionX >= 0f)
                _animator.SetTrigger(triggerRight);
            else
                _animator.SetTrigger(triggerLeft);
        }
    }
}
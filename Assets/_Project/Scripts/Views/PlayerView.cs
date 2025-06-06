using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Controllers;
using _Project.Scripts.Data;

namespace _Project.Scripts.Views
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Canvas _healthCanvas;
        
        private PlayerController _controller;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        
        private float _currentFacing = 1f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }
        
        public void SetController(PlayerController controller)
        {
            _controller = controller;
        }
        
        public void SetFacing(float directionX)
        {
            if (Mathf.Approximately(directionX, _currentFacing))
                return;

            _currentFacing = directionX;
            _animator.SetFloat(CharacterAnimationParams.IdleDirection, directionX);

            float absX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(directionX * absX, transform.localScale.y, transform.localScale.z);
        }

        public void SetWalkDirection(bool walkRight, bool walkLeft, float lastDirectionX)
        {
            SetFacing(lastDirectionX);
            
            _animator.SetBool(CharacterAnimationParams.WalkingRightPlayer, false);
            _animator.SetBool(CharacterAnimationParams.WalkingLeftPlayer, false);
            
            if (walkRight)
            {
                _animator.SetBool(CharacterAnimationParams.WalkingRightPlayer, true);
            
                return;
            }
            
            if (walkLeft)
            {
                _animator.SetBool(CharacterAnimationParams.WalkingLeftPlayer, true);
            
                return;
            }
        }
        
        public void PlayAttack(float lastDirectionX)
        {
            SetFacing(lastDirectionX);
            
            _animator.SetTrigger(CharacterAnimationParams.Attack);
            
        }
        
        public void OnAttackButtonPressed()
        {
            _controller?.TryShoot();
        }
    }
}
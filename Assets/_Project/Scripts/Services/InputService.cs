using UnityEngine;
using Zenject;
using _Project.Scripts.Interfaces;

namespace _Project.Scripts.Services
{
    public class InputService
    {
        private const string VerticalAxis = "Vertical";
        private const string HorizontalAxis = "Horizontal";
        private const string FireButton = "Fire1";

        private readonly Camera _camera;
        private readonly IJoystick _joystick;

        [Inject]
        public InputService(IJoystick joystick)
        {
            _joystick = joystick;
            _camera = Camera.main;
        }

        public Vector2 GetMovement()
        {
            if (_joystick != null)
            {
                Vector2 joystickMovement = new Vector2(_joystick.Horizontal, _joystick.Vertical);

                if (joystickMovement.sqrMagnitude > Mathf.Epsilon)
                {
                    return joystickMovement.sqrMagnitude > 1f ? joystickMovement.normalized : joystickMovement;
                }
            }

            float horizontal = Input.GetAxisRaw(HorizontalAxis);
            float vertical = Input.GetAxisRaw(VerticalAxis);
            Vector2 keyboardMovement = new Vector2(horizontal, vertical);

            return keyboardMovement.sqrMagnitude > 1f ? keyboardMovement.normalized : keyboardMovement;
        }
        
        public bool IsFirePressed()
        {
            return Input.GetButtonDown(FireButton);
        }
    }
}
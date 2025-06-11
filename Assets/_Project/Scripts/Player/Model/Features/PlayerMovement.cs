using UnityEngine;
using Zenject;
using _Project.Scripts.Player.View;
using _Project.Scripts.PlayerInput;

namespace _Project.Scripts.Player.Model.Features
{
    public class PlayerMovement : ITickable
    {
        private readonly PlayerModel _playerModel;
        private readonly PlayerView _view;
        private readonly InputService _input;
        
        public float LastDirectionX { get; set; } = 1f;
        
        [Inject]
        public PlayerMovement(PlayerModel playerModel, PlayerView view, InputService input)
        {
            _playerModel = playerModel;
            _view = view;
            _input = input;
        }

        public void Tick()
        {
            Vector2 move = _input.GetMovement();
            _playerModel.MovementDirection = move;
            _playerModel.IsWalking = move.sqrMagnitude > Mathf.Epsilon;

            _view.SetVelocity(move * _playerModel.Speed);
            UpdateLastDirection(move.x);
            _view.SetWalkDirection(_playerModel.IsWalking, LastDirectionX);
        }

        private void UpdateLastDirection(float x)
        {
            if (x > 0.01f)
                LastDirectionX = 1f;
            else if (x < -0.01f)
                LastDirectionX = -1f;
        }
    }
}
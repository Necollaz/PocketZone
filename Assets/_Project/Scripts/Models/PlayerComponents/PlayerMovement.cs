using UnityEngine;
using Zenject;
using _Project.Scripts.Services;
using _Project.Scripts.Views;

namespace _Project.Scripts.Models.PlayerComponents
{
    public class PlayerMovement : ITickable
    {
        private readonly Player _player;
        private readonly PlayerView _view;
        private readonly InputService _input;
        
        public float LastDirectionX { get; set; } = 1f;
        
        [Inject]
        public PlayerMovement(Player player, PlayerView view, InputService input)
        {
            _player = player;
            _view = view;
            _input = input;
        }

        public void Tick()
        {
            Vector2 move = _input.GetMovement();
            _player.MovementDirection = move;
            _player.IsWalking = move.sqrMagnitude > Mathf.Epsilon;

            _view.SetVelocity(move * _player.Speed);
            UpdateLastDirection(move.x);
            _view.SetWalkDirection(_player.IsWalking, LastDirectionX);
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
using UnityEngine;
using _Project.Scripts.Enemy.Model;

namespace _Project.Scripts.Player.Model
{
    public class PlayerModel : LivingEntity
    {
        public PlayerModel(float speed, int maxHealth) : base(maxHealth)
        {
            Speed = speed;
        }
     
        public Vector2 MovementDirection { get; set; } = Vector2.zero;
        public float Speed { get; set; }
        public bool IsWalking { get; set; } = false;
    }
}
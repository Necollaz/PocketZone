using UnityEngine;

namespace _Project.Scripts.Models.PlayerComponents
{
    public class Player : LivingEntity
    {
        public Player(float speed, int maxHealth) : base(maxHealth)
        {
            Speed = speed;
        }
     
        public Vector2 MovementDirection { get; set; } = Vector2.zero;
        public float Speed { get; set; }
        public bool IsWalking { get; set; } = false;
    }
}
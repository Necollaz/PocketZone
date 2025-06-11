using System;

namespace _Project.Scripts.Enemy.Model
{
    public class LivingEntity
    {
        private int _currentHealth;

        public int MaxHealth { get; }
        public int CurrentHealth => _currentHealth;

        public event Action<int> HealthChanged;
        public event Action Died;

        public  LivingEntity(int maxHealth)
        {
            MaxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (_currentHealth <= 0)
                return;

            _currentHealth = Math.Max(0, _currentHealth - amount);
            
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth == 0)
                Died?.Invoke();
        }
    }
}
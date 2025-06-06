using System;

namespace _Project.Scripts.Models
{
    public abstract class LivingEntity
    {
        private int _currentHealth;

        public int MaxHealth { get; protected set; }
        public int CurrentHealth => _currentHealth;

        public event Action<int> HealthChanged;
        public event Action Died;

        protected LivingEntity(int maxHealth)
        {
            MaxHealth = maxHealth;
            _currentHealth = maxHealth;
            
            HealthChanged?.Invoke(_currentHealth);
        }

        public virtual void TakeDamage(int amount)
        {
            if (_currentHealth <= 0)
                return;

            _currentHealth = Math.Max(0, _currentHealth - amount);
            
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth == 0)
                Died?.Invoke();
        }

        public virtual void ResetHealth()
        {
            _currentHealth = MaxHealth;
            
            HealthChanged?.Invoke(_currentHealth);
        }
        
        public bool IsDead => _currentHealth <= 0;
    }
}
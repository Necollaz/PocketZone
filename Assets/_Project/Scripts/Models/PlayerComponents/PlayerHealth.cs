using Zenject;
using _Project.Scripts.Views;

namespace _Project.Scripts.Models.PlayerComponents
{
    public class PlayerHealth
    {
        [Inject] 
        public PlayerHealth(Player model, HealthView view)
        {
            view.Initialize(model.CurrentHealth, model.MaxHealth);
            
            model.HealthChanged += view.UpdateHealth;
        }
    }
}
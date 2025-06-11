using Zenject;
using _Project.Scripts.Player.View;

namespace _Project.Scripts.Player.Model
{
    public class PlayerHealth
    {
        [Inject] 
        public PlayerHealth(PlayerModel model, HealthView view)
        {
            view.Initialize(model.CurrentHealth, model.MaxHealth);
            
            model.HealthChanged += view.UpdateHealth;
        }
    }
}
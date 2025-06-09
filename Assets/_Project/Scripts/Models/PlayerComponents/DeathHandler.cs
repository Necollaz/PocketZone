using UnityEngine.SceneManagement;
using Zenject;
using _Project.Scripts.Views;

namespace _Project.Scripts.Models.PlayerComponents
{
    public class DeathHandler
    {
        [Inject]
        public DeathHandler(Player model, PlayerView view)
        {
            model.Died += () =>
            {
                view.gameObject.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            };
        }
    }
}
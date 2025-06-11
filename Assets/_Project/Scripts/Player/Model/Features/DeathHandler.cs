using UnityEngine.SceneManagement;
using Zenject;
using _Project.Scripts.Player.View;

namespace _Project.Scripts.Player.Model.Features
{
    public class DeathHandler
    {
        [Inject]
        public DeathHandler(PlayerModel model, PlayerView view)
        {
            model.Died += () =>
            {
                view.gameObject.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            };
        }
    }
}
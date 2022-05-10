using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTowerDefense.UI
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private string gameScene;
        
        public void RestartGame()
        {
            SceneManager.LoadSceneAsync(gameScene);
        }
    }
}

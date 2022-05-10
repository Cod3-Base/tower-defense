using UnityEngine;

namespace GMTowerDefense.EndScreen
{
    public class GameDataInteraction : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}

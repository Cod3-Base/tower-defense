using GMTowerDefense.GameManaging;
using GMTowerDefense.Player;
using UnityEngine;

namespace GMTowerDefense.ShopSystem
{
    public class BuyTower : MonoBehaviour
    {
        [SerializeField] private GameObject waveContainer;
        
        [SerializeField] private LayerMask buyLayer;
        [SerializeField] private Material selectMaterial;
        [SerializeField] private GameObject towerContainer;

        private GameObject _currentTower;

        private int i = 0;
        
        /// <summary>
        /// Tries to buy the abTower, based on money.
        /// </summary>
        public void TryBuyTower(GameObject towerPrefab)
        {
            Debug.Log($"Trying buy for: {towerPrefab.GetComponent<AbTowerBehaviour>().towerType}");
            
            AbTowerBehaviour abTowerBehaviourScript = towerPrefab.GetComponent<AbTowerBehaviour>();

            GameManager gameManagerScript = GetComponent<GameManager>();
            
            if (_currentTower == null && gameManagerScript.Cash >= abTowerBehaviourScript.towerCost)
            {
                i++;
                
                Debug.Log($"Buy succeeded for: {towerPrefab.GetComponent<AbTowerBehaviour>().towerType}");
                
                _currentTower = Instantiate(towerPrefab, towerContainer.transform, true);

                _currentTower.GetComponent<AbTowerBehaviour>().Initialize(this, buyLayer, selectMaterial, waveContainer);
                _currentTower.name = $"{_currentTower.name}{i}";
            }
            else
            {
                Debug.Log($"Buy failed for: {towerPrefab.GetComponent<AbTowerBehaviour>().towerType}");
            }
        }

        /// <summary>
        /// Call when abTower has been placed
        /// </summary>
        public void PlaceTower(AbTowerBehaviour abTower)
        {
            Debug.Log($"Deselecting for: {abTower.GetComponent<AbTowerBehaviour>().towerType}");
            
            GameManager gameManagerScript = GetComponent<GameManager>();
            
            gameManagerScript.DecreaseCash(abTower.towerCost);
            
            _currentTower = null;
        }
        
        /// <summary>
        /// Call when abTower placement has been cancelled
        /// </summary>
        public void CancelPlacement(AbTowerBehaviour abTower)
        {
            Debug.Log($"Canceling placement for: {abTower.GetComponent<AbTowerBehaviour>().towerType}");
            
            _currentTower = null;
        }
    }
}

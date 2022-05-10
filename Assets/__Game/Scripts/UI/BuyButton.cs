using GMTowerDefense.Player;
using GMTowerDefense.ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTowerDefense.UI
{
    public class BuyButton : MonoBehaviour
    {
        [SerializeField] private GameObject towerPrefab;
        [SerializeField] private GameObject gameManager;
        [SerializeField] private TextMeshProUGUI buyText;
        
        private Button _button;
        private BuyTower buyScript;

        private void Awake()
        {
            buyScript = gameManager.GetComponent<BuyTower>();
            AbTowerBehaviour tower = towerPrefab.GetComponent<AbTowerBehaviour>();

            buyText.text = $"{tower.towerType}\n${tower.towerCost}";
            
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Buy);
        }

        private void Buy()
        {
            Debug.Log($"Clicked buy for: {towerPrefab.GetComponent<AbTowerBehaviour>().towerType}");
            
            buyScript.TryBuyTower(towerPrefab);
        }
    }
}

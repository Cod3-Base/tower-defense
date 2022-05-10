using System.Collections.Generic;
using System.Linq;
using GMTowerDefense.Enemy;
using GMTowerDefense.GameManaging;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTowerDefense
{
    public class SpawnerBehaviour : MonoBehaviour
    {
        [Header("Objects used")]
        [SerializeField] private GameObject spawnCell;
        [SerializeField] private GameObject waveContainer;
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private GameObject gameManager;
        [SerializeField] private List<GameObject> waypoints;

        [Header("Config")]
        [SerializeField] public int minimumWaveActivation;
        [SerializeField, Tooltip("Delay in seconds between each enemy.")] private float spawnDelay;

        public void GenerateWave()
        {
            GameManager gameManagerScript = gameManager.GetComponent<GameManager>();
            
            List<GameObject> enemyPossibles = GetPossibleWaveEnemies(gameManagerScript);

            int enemySorts = Random.Range(1, enemyPossibles.Count);

            Dictionary<GameObject, int> enemyAmount = new Dictionary<GameObject, int>();

            for (int i = 0; i < enemySorts; i++)
            {
                int enemyIndex = Random.Range(0, enemyPossibles.Count - 1);
                int defaultAmount = enemyPossibles[enemyIndex].GetComponent<AbEnemyBehaviour>().firstAmount;
                
                int maxAmount = enemyPossibles[enemyIndex].GetComponent<AbEnemyBehaviour>().firstAmount + (int)(gameManagerScript.CurrentWave*0.75);

                int amount = Random.Range(defaultAmount, maxAmount);
                
                enemyAmount.Add(enemyPossibles[enemyIndex], amount);
            }

            WaveBehaviour wave = gameObject.AddComponent<WaveBehaviour>();
            wave.Initialize(enemyAmount, waveContainer, spawnDelay, gameManager);
            wave.SpawnEnemies(spawnCell, gameManagerScript.CurrentWave, waypoints);
        }

        private List<GameObject> GetPossibleWaveEnemies(GameManager gameManagerScript)
        {
            // Checks if the current wave is higher than (or equal to) the required wave by enemy, then it adds it to the possibilities.
            return enemyPrefabs.Where(enemyPrefab => gameManagerScript.CurrentWave >= enemyPrefab.GetComponent<AbEnemyBehaviour>().minimumWave).ToList();
        }
    }
}

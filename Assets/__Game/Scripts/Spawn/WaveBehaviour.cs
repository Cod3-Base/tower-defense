using System.Collections;
using System.Collections.Generic;
using GMTowerDefense.Enemy;
using GMTowerDefense.GameManaging;
using UnityEngine;

namespace GMTowerDefense
{
    public class WaveBehaviour : MonoBehaviour
    {
        private float _spawnDelay;

        private GameObject _enemyContainer;
        
        private Dictionary<GameObject, int> _enemies;

        private Coroutine _currentCor;

        private GameObject _gameManager;

        public void Initialize(Dictionary<GameObject, int> enemies, GameObject enemyContainer, float spawnDelay, GameObject gameManager)
        {
            _enemies = enemies;
            _enemyContainer = enemyContainer;
            _spawnDelay = spawnDelay;
            _gameManager = gameManager;
        }

        public void SpawnEnemies(GameObject spawner, int waveNumber, List<GameObject> path)
        {
            _currentCor ??= StartCoroutine(SpawnEnemiesCor(spawner, waveNumber, path));
        }

        private IEnumerator SpawnEnemiesCor(GameObject spawner, int waveNumber, List<GameObject> path)
        {
            GameObject parent = new GameObject($"Wave {waveNumber}")
                {
                    transform =
                        {
                            parent = _enemyContainer.transform
                        }
                };

            foreach (KeyValuePair<GameObject,int> enemyAmountKv in _enemies)
            {
                Vector3 startLocation = spawner.GetComponent<Renderer>().bounds.center;
                    
                Vector3 startLocationNormalized = new Vector3(startLocation.x, startLocation.y + (enemyAmountKv.Key.transform.lossyScale.y/2), startLocation.z);
                
                for (int i = 0; i < enemyAmountKv.Value; i++)
                {
                    GameObject enemy = Instantiate(enemyAmountKv.Key, parent.transform, true);
                    enemy.transform.position = startLocationNormalized;

                    AbEnemyBehaviour abEnemyScript = enemy.GetComponent<AbEnemyBehaviour>();
                    abEnemyScript.Initialize(path, spawner, _gameManager);

                    float currentSpawnDelay = _spawnDelay - ((float)_gameManager.GetComponent<GameManager>().CurrentWave/15) + 0.5f;
                
                    yield return new WaitForSeconds(currentSpawnDelay);   
                }
            }

            _currentCor = null;
            
            yield return null;
        }
    }
}

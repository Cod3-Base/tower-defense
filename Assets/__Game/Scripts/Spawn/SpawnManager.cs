using System.Collections.Generic;
using GMTowerDefense.GameManaging;
using UnityEngine;

namespace GMTowerDefense
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> spawners;
        
        [SerializeField, Tooltip("Delay in seconds between each wave.")] private float waveDelay;
        
        private float _currentTimer;
        private void Awake()
        {
            GenerateWaves();
        }
        
        private void Update()
        {
            _currentTimer += Time.deltaTime;

            if (_currentTimer >= waveDelay)
            {
                GenerateWaves();
            
                _currentTimer = 0;
            }
        }

        private void GenerateWaves()
        {
            GameManager gameManager = GetComponent<GameManager>();
         
            gameManager.IncreaseWaves();
            waveDelay += 0.5f;
            
            int currentWave = gameManager.CurrentWave;
            
            foreach (GameObject spawner in spawners)
            {
                SpawnerBehaviour spawnerBehaviour = spawner.GetComponent<SpawnerBehaviour>();

                if (spawnerBehaviour.minimumWaveActivation <= currentWave)
                    spawnerBehaviour.GenerateWave();
            }
        }
    }
}

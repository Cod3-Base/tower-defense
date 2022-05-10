using System;
using System.Collections;
using System.Collections.Generic;
using __Common.Extensions;
using GMTowerDefense.GameManaging;
using UnityEngine;

namespace GMTowerDefense.Enemy
{
    /// <summary>
    /// The abstract enemy class. Handles all base enemy behaviour.
    /// </summary>
    public abstract class AbEnemyBehaviour : MonoBehaviour
    {
        private GameObject _spawnCell;
        private GameObject _gameManager;

        [Header("Config")]
        [SerializeField] private int cashWorth;
        [SerializeField] private string enemyType;
        [SerializeField] private float maxSpeed;
        [SerializeField] private int liveDecrease;
        [SerializeField] private float maxHealth;
        [SerializeField] public int minimumWave;
        [SerializeField] public int firstAmount;
        [SerializeField] private float speedRegainPerSec;
        [SerializeField] private int scoreWorth;

        private float _speed;
        private List<GameObject> _path;

        [SerializeField] private float currentHealth;

        private Coroutine _currentCor;
        private float _currentTimer;

        private void Awake()
        {
            _speed = maxSpeed;
        }

        public void Initialize(List<GameObject> path, GameObject spawnCell, GameObject gameManager)
        {
            _path = path;
            _spawnCell = spawnCell;
            _gameManager = gameManager;
            currentHealth = maxHealth;

            _currentCor ??= StartCoroutine(MoveAlongPathCor());
        }
        
        protected virtual void Update()
        {
            if (currentHealth <= 0)
            {
                _gameManager.GetComponent<GameManager>().IncreaseCash(cashWorth);
                _gameManager.GetComponent<GameManager>().IncreaseScore(scoreWorth);
                Destroy(gameObject);
            }

            if (_speed < maxSpeed)
            {
                _currentTimer += Time.deltaTime;

                if (_currentTimer >= 1)
                {
                    _speed += speedRegainPerSec;
                    _currentTimer = 0;
                }
            }
        }

        /// <summary>
        /// The coroutine to move to each target within path.
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveAlongPathCor()
        {
            GameObject currentCell = null;
            
            foreach (GameObject target in _path)
            {
                float t = 0;

                Vector3 startPos = transform.position;
                Vector3 targetPos = target.GetComponent<Renderer>().bounds.center;
                Vector3 endPos = new Vector3(targetPos.x, startPos.y, targetPos.z);
                Vector3 distance = currentCell == null ? _spawnCell.transform.GetDistanceBetween(target.transform) : currentCell.transform.GetDistanceBetween(target.transform);

                float targetDuration = distance.GetMaxValue() / _speed;
                
                while (t < 1)
                {
                    t += Time.deltaTime / targetDuration;

                    transform.position = Vector3.Lerp(startPos, endPos, t);

                    yield return new WaitForEndOfFrame();
                }

                currentCell = target;
            }

            _currentCor = null;
            
            yield return null;
        }

        /// <summary>
        /// Decreases the enemy health by amount.
        /// </summary>
        public void DecreaseHealth(float amount)
        {
            currentHealth -= amount;
        }

        /// <summary>
        /// Decreases the enemy _speed by amount.
        /// </summary>
        public void DecreaseSpeed(float amount)
        {
            _speed -= amount;
        }
        
        private void OnTriggerStay(Collider other)
        {
            GameObject otherObject = other.gameObject;

            GameManager gameManager = _gameManager.GetComponent<GameManager>();
            
            if (otherObject.CompareTag("Player"))
            {
                if (otherObject.GetComponent<Collider>().FullyWithinBorders(GetComponent<Collider>()))
                {
                    gameManager.DecreaseLives(liveDecrease);
                    Destroy(gameObject);
                }
            }
        }
    }
}

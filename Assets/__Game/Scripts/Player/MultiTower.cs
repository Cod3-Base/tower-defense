using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTowerDefense.Player
{
    public class MultiTower : AbTowerBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private List<GameObject> firePoints;

        protected override IEnumerator FireRoutine()
        {
            CurrentCor = true;
            
            List<GameObject> enemiesInRadius = SearchEnemies();
            
            while (enemiesInRadius.Count > 0)
            {
                enemiesInRadius = SearchEnemies();

                for (int i = 0; i < enemiesInRadius.Count; i++)
                {
                    Fire(enemiesInRadius[i], 0);
                    
                    if (i + 1 < enemiesInRadius.Count)
                        Fire(enemiesInRadius[i + 1], 1);

                    else
                    {
                        Fire(enemiesInRadius[i], 1);
                    }
                    
                    if (i + 2 < enemiesInRadius.Count)
                        Fire(enemiesInRadius[i + 2], 2);

                    else if (i + 1 < enemiesInRadius.Count)
                    {
                        Fire(enemiesInRadius[i + 1], 2);
                    }

                    else
                    {
                        Fire(enemiesInRadius[i], 2);
                    }

                    yield return new WaitForSeconds(1 / fireRate);
                }

                yield return new WaitForEndOfFrame();
            }

            CurrentCor = false;

            yield return null;
        }

        private void Fire(GameObject enemy, int point)
        {
            GameObject bullet = Instantiate(projectilePrefab);

            bullet.GetComponent<Transform>().position = firePoints[point].transform.position;
            bullet.GetComponent<Bullet>().Initialize(enemy, towerDamage);
        }
    }
}

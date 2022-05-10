using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTowerDefense.Player
{
    public class SlowTowerBehaviour : AbTowerBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject projectilePrefab;

        [SerializeField] private float speedDecrease;

        protected override IEnumerator FireRoutine()
        {
            CurrentCor = true;
            
            List<GameObject> enemiesInRadius = SearchEnemies();

            GameObject lastEnemy = null;
            
            while (enemiesInRadius.Count > 0)
            {
                enemiesInRadius = SearchEnemies();
                
                foreach (GameObject enemy in enemiesInRadius)
                {
                    if (lastEnemy != null && lastEnemy == enemy && enemiesInRadius.Count > 1)
                        continue;

                    GameObject bullet = Instantiate(projectilePrefab);

                    bullet.GetComponent<Transform>().position = firePoint.position;
                    bullet.GetComponent<SlowBullet>().Initialize(enemy, towerDamage, speedDecrease);

                    lastEnemy = enemy;

                    yield return new WaitForSeconds((1 / fireRate));
                }

                yield return new WaitForEndOfFrame();
            }

            CurrentCor = false;

            yield return null;
        }
    }
}

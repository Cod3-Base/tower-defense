using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTowerDefense.Player
{
    public class BasicTowerBehaviour : AbTowerBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject projectilePrefab;

        protected override IEnumerator FireRoutine()
        {
            CurrentCor = true;
            
            List<GameObject> enemiesInRadius = SearchEnemies();
            
            while (enemiesInRadius.Count > 0)
            {
                enemiesInRadius = SearchEnemies();
                
                foreach (GameObject enemy in enemiesInRadius)
                {
                    GameObject bullet = Instantiate(projectilePrefab);

                    bullet.GetComponent<Transform>().position = firePoint.position;
                    bullet.GetComponent<Bullet>().Initialize(enemy, towerDamage);

                    yield return new WaitForSeconds(1 / fireRate);
                }

                yield return new WaitForEndOfFrame();
            }

            CurrentCor = false;

            yield return null;
        }
    }
}

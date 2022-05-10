using GMTowerDefense.Enemy;
using UnityEngine;

namespace GMTowerDefense.Player
{
    public class SlowBullet : AbBullet
    {
        private float _speedDecrease;

        public void Initialize(GameObject targetEnemy, float damageDone, float speedDecrease)
        {
            base.Initialize(targetEnemy, damageDone);
            _speedDecrease = speedDecrease;
        }

        protected override void OnHit()
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            AbEnemyBehaviour enemyScript = Target.GetComponent<AbEnemyBehaviour>();

            if (enemyScript != null)
            {
                enemyScript.DecreaseHealth(BulletDamage);
                enemyScript.DecreaseSpeed(_speedDecrease);
                
                Destroy(gameObject);
            }
        }
    }
}

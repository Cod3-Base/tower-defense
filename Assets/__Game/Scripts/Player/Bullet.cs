using GMTowerDefense.Enemy;

namespace GMTowerDefense.Player
{
    public class Bullet : AbBullet
    {
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
                Destroy(gameObject);
            }
        }
    }
}

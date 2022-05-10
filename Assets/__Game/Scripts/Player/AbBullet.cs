using GMTowerDefense.Enemy;
using UnityEngine;

namespace GMTowerDefense.Player
{
    public abstract class AbBullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float bulletLiveTimeInSeconds;
        
        protected GameObject Target;
        private Vector3 _initialTargetLoc;
        
        protected float BulletDamage;

        private float _currentTimer;
        
        public void Initialize(GameObject targetEnemy, float damageDone)
        {
            if (targetEnemy == null)
                return;
            
            Target = targetEnemy;
            _initialTargetLoc = targetEnemy.transform.position;
            
            BulletDamage = damageDone;
        }
        
        private void Update()
        {
            _currentTimer += Time.deltaTime;
            
            if (_currentTimer >= bulletLiveTimeInSeconds)
                Destroy(gameObject);
            
            Vector3 dir = _initialTargetLoc - transform.position;

            float distanceThisFrame = bulletSpeed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                OnHit();
            }
            
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }

        protected abstract void OnHit();
    }
}

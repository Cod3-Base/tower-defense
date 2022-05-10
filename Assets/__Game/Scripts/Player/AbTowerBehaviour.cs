using System.Collections;
using System.Collections.Generic;
using GMTowerDefense.ShopSystem;
using JetBrains.Annotations;
using UnityEngine;

namespace GMTowerDefense.Player
{
    [RequireComponent(typeof (BoxCollider))]
    [RequireComponent(typeof (RadiusIndicator))]
    public abstract class AbTowerBehaviour : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] public string towerType;
        [SerializeField] public int towerCost;
        [SerializeField] protected float towerDamage;
        
        
        [SerializeField] protected float searchRadius;
        [SerializeField, Tooltip("Projectiles per second")] protected float fireRate;

        private GameObject _enemyContainer;

        private BuyTower _buyScript;
        private bool _inHand;
        private LayerMask _buyAble;
        private Material _selectMaterial;

        private Material _oldMaterial;

        [CanBeNull]
        private GameObject _currentCell;

        protected bool CurrentCor;

        private RadiusIndicator _radiusIndicator;
        
        private void Awake()
        {
            _radiusIndicator = GetComponent<RadiusIndicator>();
            _radiusIndicator.Initialize(searchRadius);
        }

        private void Start()
        {
            _radiusIndicator.NotShow();
        }

        private void OnMouseDown()
        {
            _radiusIndicator.Show();
        }

        private void OnMouseExit()
        {
            _radiusIndicator.NotShow();
        }

        public virtual void Initialize(BuyTower buyScript, LayerMask buyAble, Material selectMaterial, GameObject enemyContainer)
        {
            _buyScript = buyScript;
            _inHand = true;
            _buyAble = buyAble;
            _selectMaterial = selectMaterial;
            this._enemyContainer = enemyContainer;

            DisableRenderer();
        }

        private void DisableRenderer()
        {
            foreach (Transform trans in transform)
            {
                GameObject obj = trans.gameObject;
                
                MeshRenderer ren = obj.GetComponent<MeshRenderer>();

                if (ren != null)
                    ren.enabled = false;
            }
        }

        private void EnableRenderer()
        {
            foreach (Transform trans in transform)
            {
                GameObject obj = trans.gameObject;
                
                MeshRenderer ren = obj.GetComponent<MeshRenderer>();

                if (ren != null)
                    ren.enabled = true;
            }
        }
        
        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }

        private void Update()
        {
            if (_inHand)
            {
                GameObject cell = CheckCell();

                if (_currentCell != cell)
                {
                    ResetCell(_currentCell);
                    
                    if(cell != null && cell.layer == LayerMask.NameToLayer("BuyTile"))
                        HighlightCell(cell);
                }

                _currentCell = cell;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _radiusIndicator.AlterPosition();
                    _radiusIndicator.Show();
                    PlaceTower();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                    CancelPlacement();
            }

            if (!_inHand && !CurrentCor)
            {
                StartCoroutine(FireRoutine());
            }
        }

        [CanBeNull]
        private GameObject CheckCell()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _buyAble)) 
            {
                return hit.collider.gameObject;
            }

            return null;
        }

        private void HighlightCell([CanBeNull] GameObject cell)
        {
            if (cell != null)
            {
                _oldMaterial = cell.GetComponent<MeshRenderer>().material;
                
                cell.GetComponent<MeshRenderer>().material = _selectMaterial;
            }
                
        }

        private void ResetCell([CanBeNull] GameObject cell)
        {
            if (cell != null)
            {
                cell.GetComponent<MeshRenderer>().material = _oldMaterial;
            }
        }

        private void PlaceTower()
        {
            // When the tower is placed.
            if (_currentCell != null)
            {
                if (_currentCell.GetComponentInChildren<AbTowerBehaviour>())
                    return;
                
                Vector3 cellCenter = _currentCell.transform.position;
                Vector3 lossyScale = transform.lossyScale;
                
                float x = cellCenter.x + (lossyScale.x/2);
                float y = cellCenter.y + (lossyScale.y/2);
                float z = cellCenter.z + (lossyScale.z/2);

                Vector3 newPos = new Vector3(x, y, z);

                transform.parent = _currentCell.transform;
                transform.position = newPos;
                
                EnableRenderer();
                _buyScript.PlaceTower(this);
                _inHand = false;
            }
        }

        private void CancelPlacement()
        {
            _buyScript.CancelPlacement(this);
            
            _inHand = false;
        }

        protected abstract IEnumerator FireRoutine();
        
        protected virtual List<GameObject> SearchEnemies()
        {
            List<GameObject> enemies = new List<GameObject>();

            foreach (Transform waveTransform in _enemyContainer.transform)
            {
                foreach (Transform enemyTransform in waveTransform)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);
                    if (distanceToEnemy <= searchRadius)
                        enemies.Add(enemyTransform.gameObject);                    
                }
            }
            
            return enemies;
        }

    }
}

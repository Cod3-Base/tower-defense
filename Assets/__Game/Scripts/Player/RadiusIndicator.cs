using UnityEngine;

namespace GMTowerDefense.Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class RadiusIndicator : MonoBehaviour {
        [Range(0,50)]
        private int segments = 50;
        LineRenderer line;

        [SerializeField] private float yOffset;

        private float _radius = 12;

        void Awake()
        {
            line = gameObject.GetComponent<LineRenderer>();
            line.startWidth = 0.2f;
            line.endWidth = 0.2f;

            line.SetVertexCount (segments + 1);
            line.useWorldSpace = false;
            CreatePoints();
        }

        public void Initialize(float radius)
        {
            _radius = radius;
        }

        public void Show()
        {
            if (line.enabled)
                return;
            
            line.enabled = true;
        }
        
        public void NotShow()
        {
            if (!line.enabled)
                return;
            
            line.enabled = false;
        }

        public void AlterPosition()
        {
            CreatePoints();
        }

        private void CreatePoints ()
        {
            float angle = 20f;

            for (int i = 0; i < (segments + 1); i++)
            {
                float x = (Mathf.Sin(Mathf.Deg2Rad*angle)*_radius) / transform.lossyScale.x;
                float z = (Mathf.Cos(Mathf.Deg2Rad*angle)*_radius) / transform.lossyScale.z;

                line.SetPosition (i,new Vector3(x,-transform.lossyScale.y/2 + yOffset,z) );

                angle += (360f / segments);
            }
        }
    }
}

using UnityEngine;

namespace _Project.Scripts.Common
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);

        private void LateUpdate()
        {
            if (_target == null)
                return;
            
            transform.position = _target.position + _offset;
        }
        
        private void Reset()
        {
            _offset = new Vector3(0f, 0f, -10f);
        }
    }
}
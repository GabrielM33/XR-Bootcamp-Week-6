using UnityEngine;

namespace XR
{
    public class MinimapRotator : MonoBehaviour
    {
        [SerializeField] private Transform _rotationReference;

        private Vector3 _initialRotation;
        
        // Start is called before the first frame update
        private void Start()
        {
            _initialRotation = transform.eulerAngles;
        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 newRot = new Vector3(0, 0, -_rotationReference.eulerAngles.y) + _initialRotation;
            transform.rotation = Quaternion.Euler(newRot);
        }
    }
}

using UnityEngine;
using UnityEngine.Serialization;

namespace a_Scripts
{
    public class BasicMovement : MonoBehaviour
    {
        public float moveSpeed;
        public float rotateSpeed;
        public bool lamp;
        
        private BasicWorld _basicWorld;
	
        private CarController _carController;
        private Transform carTransform;

        private void Start()
        {
            _carController = FindFirstObjectByType<CarController>();
            _basicWorld = FindFirstObjectByType<BasicWorld>();

            if (_carController != null)
            {
                carTransform = _carController.gameObject.transform;
            }
                
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));

            if (_carController != null)
            {
                CheckRotate();
            }
                
        }
	
        private void CheckRotate()
        {
            var direction = (lamp) ? Vector3.right : Vector3.forward;
		
            var carRotation = carTransform.localEulerAngles.y;

            if (carRotation > _carController.rotationAngle * 2f)
            {
                carRotation = (360 - carRotation) * -1f;
            }
                
            
            transform.Rotate(direction * (-rotateSpeed * (carRotation / _carController.rotationAngle) * (36f / _basicWorld.dimensions.x) * Time.deltaTime));
        }
    }
}
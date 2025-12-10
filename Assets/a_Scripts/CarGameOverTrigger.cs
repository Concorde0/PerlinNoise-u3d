using UnityEngine;

namespace a_Scripts
{
    public class CarGameOverTrigger : MonoBehaviour
    {
        private GameMaganer manager;
	
        private void Start()
        {
            manager =FindFirstObjectByType<GameMaganer>();
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.name == "World piece")
            {
                manager.GameOver();
            }
              
        }
    }
}
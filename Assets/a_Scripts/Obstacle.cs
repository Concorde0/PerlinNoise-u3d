using Unity.VisualScripting;
using UnityEngine;

namespace a_Scripts
{
    public class Obstacle : MonoBehaviour
    {
        private GameMaganer manager;
	
        private void Start()
        {
            manager = FindFirstObjectByType<GameMaganer>();
        }
	
         private void OnCollisionEnter(Collision other)
         {

             if (other.gameObject.transform.root.CompareTag("Player"))
             {
                 manager.GameOver();
             }
                
         }
    }
}
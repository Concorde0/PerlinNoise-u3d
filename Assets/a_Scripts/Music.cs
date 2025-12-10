using UnityEngine;

namespace a_Scripts
{
    public class Music : MonoBehaviour
    {
        private static Music instance;

        private void Awake()
        {
            if(!instance)
            {
                instance = this; 
            }
            else
            {
                Destroy(gameObject); 
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }
}
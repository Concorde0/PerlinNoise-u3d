using UnityEngine;

namespace a_Scripts
{
    public class Gate: MonoBehaviour
    {
        public AudioSource scoreAudio;  
        
        private GameMaganer manager;
        private bool addedScore; 
	
        private void Start()
        {
            manager = FindFirstObjectByType<GameMaganer>();
        }
	
        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.transform.root.CompareTag("Player") || addedScore) return;
            
            addedScore = true;
            manager.UpdateScore(1); 
            scoreAudio.Play(); 
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace a_Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public Animator UIAnimator;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()))
            {
                if (!(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject((Input.GetTouch(0).fingerId))))
                {
                    StartGame();
                }
            }
        }
        
        private void StartGame()
        {
            UIAnimator.SetTrigger("Start");
            StartCoroutine(LoadScene("Game"));
        }
        
        private IEnumerator LoadScene(string scene)
        {
            yield return new WaitForSeconds(0.6f);
            SceneManager.LoadScene(scene);
        }
    }
}
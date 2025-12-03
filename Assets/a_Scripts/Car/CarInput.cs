using System;
using UnityEngine;

namespace a_Scripts
{
    public class CarInput : MonoBehaviour
    {
        private PlayerInput _input;
        
        public float Horizontal { get; private set; }
        public bool Clicked { get; private set; }
        public Vector2 MousePosition { get; private set; }
        
        private void Awake()
        {
            _input = new PlayerInput();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.GamePlay.Move.performed += ctx => Horizontal = ctx.ReadValue<Vector2>().x;
            _input.GamePlay.Move.canceled += ctx => Horizontal = 0;
            
            _input.GamePlay.Click.performed += ctx => Clicked = true;
            _input.GamePlay.Click.canceled += ctx => Clicked = false;

            _input.GamePlay.Mouse.performed += ctx => MousePosition = ctx.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            _input.Disable();
        }
    }
}
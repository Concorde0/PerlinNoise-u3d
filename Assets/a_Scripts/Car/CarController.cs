using System;
using System.Collections;
using System.Collections.Generic;
using a_Scripts;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private CarInputSystem _input;
    
    public Transform[] wheelMeshes;
    public WheelCollider[] wheelColliders;

    public int rotateSpeed;
    public int rotationAngle;
    public int wheelRotateSpeed;
    
    private int targetRotation;

    private void LateUpdate()
    {
        for(int i = 0; i < wheelMeshes.Length; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);
            wheelMeshes[i].position = pos;
            wheelMeshes[i].Rotate(Vector3.right * (Time.deltaTime * wheelRotateSpeed));
        }
        
        if(_input.Clicked || _input.Horizontal != 0)
        {
            UpdateTargetRotation();
        }
        else if(targetRotation != 0)
        {
            targetRotation = 0;
        }
        
        Vector3 rotation = new Vector3(transform.localEulerAngles.x, targetRotation, transform.localEulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotation), rotateSpeed * Time.deltaTime);
        
    }

    private void UpdateTargetRotation()
    {
        if(_input.Horizontal == 0){
            if(Input.mousePosition.x > Screen.width * 0.5f)
            {
                targetRotation = rotationAngle;
            }
            else
            {
                targetRotation = -rotationAngle;
            }
        }
        else{
            targetRotation = (int)(rotationAngle * _input.Horizontal);
        }
    }
    
    
}

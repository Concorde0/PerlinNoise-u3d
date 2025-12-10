using System;
using System.Collections;
using System.Collections.Generic;
using a_Scripts;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private CarInput _input;

    private Rigidbody _rb;
    public Transform[] wheelMeshes;
    public WheelCollider[] wheelColliders;

    public int rotateSpeed;
    public int rotationAngle;
    public int wheelRotateSpeed;
    
    public Transform[] grassEffects;  
    public Transform[] skidMarkPivots;
    public float grassEffectOffset; 
    
    public Transform back;
    public float constantBackForce; 
    
    public GameObject skidMark;
    public float skidMarkSize;
    public float skidMarkDelay;
    public float minRotationDifference;
    
    public GameObject ragdoll;
    
    private int targetRotation;
    private BasicWorld basicWorld;
            
    private float lastRotation;
    private bool skidMarkRoutine;

    private void Start()
    {
        basicWorld = FindFirstObjectByType<BasicWorld>();
        _input = GetComponent<CarInput>();
        _rb = GetComponent<Rigidbody>();
        
        StartCoroutine(SkidMark());
    }
    
    private void FixedUpdate()
    {
		UpdateEffects();
	}
    
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
		if(_input.Horizontal == 0)
		{
			if(_input.MousePosition.x > Screen.width * 0.5f)
			{
				targetRotation = rotationAngle;
			}
			else
			{
				targetRotation = -rotationAngle;
			}
		}
		else
		{
			targetRotation = (int)(rotationAngle * _input.Horizontal);
		}
	}

    
	private void UpdateEffects()
	{
		bool addForce = true;
		bool rotated = Mathf.Abs(lastRotation - transform.localEulerAngles.y) > minRotationDifference;
		for(int i = 0; i < 2; i++)
		{
			Transform wheelMesh = wheelMeshes[i + 2];
			
			if(Physics.Raycast(wheelMesh.position, Vector3.down, grassEffectOffset * 1.5f))
			{
				if (!grassEffects[i].gameObject.activeSelf)
				{
					grassEffects[i].gameObject.SetActive(true);
				}
					
				
				float effectHeight = wheelMesh.position.y - grassEffectOffset;
				Vector3 targetPosition = new Vector3(grassEffects[i].position.x, effectHeight, wheelMesh.position.z);
				grassEffects[i].position = targetPosition;
				skidMarkPivots[i].position = targetPosition;
				
				addForce = false;
			}
			else if(grassEffects[i].gameObject.activeSelf)
			{
				grassEffects[i].gameObject.SetActive(false);
			}
		}
		
		if(addForce)
		{
			_rb.AddForceAtPosition(back.position, Vector3.down * constantBackForce);
			skidMarkRoutine = false;
		}
		else
		{
			if(targetRotation != 0)
			{
				if(rotated && !skidMarkRoutine)
				{
					skidMarkRoutine = true;
				}
				else if(!rotated && skidMarkRoutine)
				{
					skidMarkRoutine = false;
				}
			}
			else
			{
				skidMarkRoutine = false;
			}
		}
		
		lastRotation = transform.localEulerAngles.y;
	}
	
	public void FallApart()
	{
		Instantiate(ragdoll, transform.position, transform.rotation);
		gameObject.SetActive(false);
	}
	
	private IEnumerator SkidMark()
	{
		while(true)
		{
			yield return new WaitForSeconds(skidMarkDelay);

			if(skidMarkRoutine)
			{
				for(int i = 0; i < skidMarkPivots.Length; i++)
				{
					GameObject newskidMark = Instantiate(skidMark, skidMarkPivots[i].position, skidMarkPivots[i].rotation);
					newskidMark.transform.parent = basicWorld.GetWorldPiece();
					newskidMark.transform.localScale = new Vector3(1, 1, 4) * skidMarkSize;
				}
			}
		}
	}
    
}

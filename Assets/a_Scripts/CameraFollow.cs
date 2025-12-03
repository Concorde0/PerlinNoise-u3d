using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public Transform camTarget;
	
	 [SerializeField] private float startDelay;
     [SerializeField] private float distance;
     [SerializeField] private float height;
     [SerializeField] private float heightDamping;
     [SerializeField] private float rotationDamping; 

	private float originalRotationDamping; 
	private bool canSwitch;

	private void Start()
	{
		originalRotationDamping = rotationDamping;
		rotationDamping = 0.1f;

		StartCoroutine(SwitchAngle());
	}
	
	private void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetAxis("Horizontal") != 0) && rotationDamping == 0.1f && canSwitch)
		{
			rotationDamping = originalRotationDamping;
		}
			
	}
	 
	private void LateUpdate()
	{		
		if (!camTarget)
		{
			return;	
		}
           	
        
        float wantedRotationAngle = camTarget.eulerAngles.y;
        float wantedHeight = camTarget.position.y + height;
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;
        
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        
        transform.position = camTarget.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        
        transform.LookAt(camTarget);
    }
	
	private IEnumerator SwitchAngle()
	{
		yield return new WaitForSeconds(startDelay);

		canSwitch = true;
	}
}

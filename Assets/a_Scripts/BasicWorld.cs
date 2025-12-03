using System;
using System.Collections;
using System.Collections.Generic;
using a_Scripts;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class BasicWorld : MonoBehaviour
{
    
    [SerializeField] private Material meshMaterial; 
    [SerializeField] private float scale;
    [SerializeField] private float perlinScale;
    [SerializeField] private float waveHeight;
    [SerializeField] private float offset;
    [SerializeField] private float randomness; 
    [SerializeField] private float globalSpeed; 
    [SerializeField] private int startTransitionLength;
    [SerializeField] private GameObject[] obstacles; 
    [SerializeField] private GameObject gate; 
    [SerializeField] private int startObstacleChance; 
    [SerializeField] private int obstacleChanceAcceleration; 
    [SerializeField] private int gateChance;
    [SerializeField] private int showItemDistance;
    [SerializeField] private float shadowHeight;
    [SerializeField] private BasicMovement lampMovement;
    
    public Vector2 dimensions;
    private Vector3[] beginPoints;
    private GameObject[] pieces = new GameObject[2];
    private GameObject currentCylinder;
    
    private void Start()
    {
	    beginPoints = new Vector3[(int)dimensions.x + 1];
	    
	    for(int i = 0; i < 2; i++)
	    {
		    GenerateWorldPiece(i);
	    }
    }

    private void LateUpdate()
    {
	    if (pieces[1] && pieces[1].transform.position.z <= 0)
	    {
		    StartCoroutine(UpdateWorldPieces());
	    }
	    
	    UpdateAllItems();
    }
    
    private void UpdateAllItems()
    {
	    GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
	    
	    for(int i = 0; i < items.Length; i++){

		    foreach(MeshRenderer renderer in items[i].GetComponentsInChildren<MeshRenderer>())
		    {

			    bool show = items[i].transform.position.z < showItemDistance;
			    
			    if (show)
			    {
				    renderer.shadowCastingMode = (items[i].transform.position.y < shadowHeight) ? ShadowCastingMode.On : ShadowCastingMode.Off;
			    }
			    
			    renderer.enabled = show;
		    }
	    }
    }
    
    private void GenerateWorldPiece(int i)
    {

	    pieces[i] = CreateCylinder();
		pieces[i].transform.Translate(Vector3.forward * (dimensions.y * scale * Mathf.PI) * i);
		
		UpdateSinglePiece(pieces[i]);
    }


    private IEnumerator UpdateWorldPieces()
    {

	    Destroy(pieces[0]);

	    pieces[0] = pieces[1];

	    pieces[1] = CreateCylinder();

	    pieces[1].transform.position = pieces[0].transform.position + Vector3.forward * (dimensions.y * scale * Mathf.PI);
	    pieces[1].transform.rotation = pieces[0].transform.rotation;
	    
	    UpdateSinglePiece(pieces[1]);

	    yield return 0;
    }
    
    private void UpdateSinglePiece(GameObject piece)
    {
	    BasicMovement movement = piece.AddComponent<BasicMovement>();
	    movement.moveSpeed = -globalSpeed;
	    
	    if(lampMovement != null)
		    movement.rotateSpeed = lampMovement.rotateSpeed;
	    
	    GameObject endPoint = new GameObject();
	    endPoint.transform.position = piece.transform.position + Vector3.forward * (dimensions.y * scale * Mathf.PI);
	    endPoint.transform.parent = piece.transform;
	    endPoint.name = "End Point";
	    
	    offset += randomness;
	    
	    if (startObstacleChance > 5)
	    {
		    startObstacleChance -= obstacleChanceAcceleration;
	    }
		   
    }

    
   
    private GameObject CreateCylinder()
    {
        GameObject newCylinder = new GameObject();
        newCylinder.name = "World piece";
        
        currentCylinder = newCylinder;
        
        MeshFilter meshFilter = newCylinder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newCylinder.AddComponent<MeshRenderer>();
        
        meshRenderer.material = meshMaterial;
        meshFilter.mesh = Generate();	
        
        newCylinder.AddComponent<MeshCollider>();
        
        
        return newCylinder;
    }
    
    private Mesh Generate()
    {
        Mesh mesh = new Mesh();
        mesh.name = "MESH";
        
        Vector3[] vertices = null;
        Vector2[] uvs = null;
        int[] triangles = null;
        
        CreateShape(ref vertices, ref uvs, ref triangles);
        
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
        return mesh;
    }
    private void CreateShape(ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles)
    {
	    int xCount = (int)dimensions.x; 
	    int zCount = (int)dimensions.y; 


	    vertices = new Vector3[(xCount + 1) * (zCount + 1)];
	    uvs = new Vector2[(xCount + 1) * (zCount + 1)];

	    int index = 0;

	    float radius = xCount * scale * 0.5f;
	    
	    for(int x = 0; x <= xCount; x++)
	    {
	        for(int z = 0; z <= zCount; z++)
	        {

	            var angle = x * Mathf.PI * 2f / xCount;

	            vertices[index] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, z * scale * Mathf.PI);

	            uvs[index] = new Vector2(x * scale, z * scale);

	            var pX = (vertices[index].x * perlinScale) + offset;
	            var pZ = (vertices[index].z * perlinScale) + offset;
	            
	            Vector3 center = new Vector3(0, 0, vertices[index].z);
	            vertices[index] += (center - vertices[index]).normalized * (Mathf.PerlinNoise(pX, pZ) * waveHeight);
	            
	            if(z < startTransitionLength && beginPoints[0] != Vector3.zero)
	            {
	                var perlinPercentage = z * (1f / startTransitionLength);
	                var beginPoint = new Vector3(beginPoints[x].x, beginPoints[x].y, vertices[index].z);
	                vertices[index] = (perlinPercentage * vertices[index]) + ((1f - perlinPercentage) * beginPoint);
	            }
	            else if(z == zCount)
	            {
	                beginPoints[x] = vertices[index];
	            }

	            if (Random.Range(0, startObstacleChance) == 0 && !(gate == null && obstacles.Length == 0))
	            {
		            CreateItem(vertices[index], x);
	            }
	                
	            
	            index++;
	        }
	    }
	    
	    triangles = new int[xCount * zCount * 6];  
	    
	    int[] boxBase = new int[6];

	    int current = 0;
	    
	    for(int x = 0; x < xCount; x++)
	    {
	        boxBase = new[]
	        { 
	            x * (zCount + 1), 
	            x * (zCount + 1) + 1,
	            (x + 1) * (zCount + 1),
	            x * (zCount + 1) + 1,
	            (x + 1) * (zCount + 1) + 1,
	            (x + 1) * (zCount + 1),
	        };
	        
	        for(int z = 0; z < zCount; z++)
	        {
	            for(int i = 0; i < 6; i++)
	            {
	                boxBase[i] += 1;
	            }
	            
	            for(int j = 0; j < 6; j++)
	            {                    
	                triangles[current + j] = boxBase[j] - 1;
	            }
	            
	            current += 6;
	        }
	    }
    }
    
    private void CreateItem(Vector3 vert, int x)
    {
	    var zCenter = new Vector3(0f, 0f, vert.z);

	    if (zCenter - vert == Vector3.zero || x == (int)dimensions.x / 4 || x == (int)dimensions.x / 4 * 3)
		    return;

	    var prefab = (Random.Range(0, gateChance) == 0) ? gate : obstacles[Random.Range(0, obstacles.Length)];
	    var newItem = Instantiate(prefab);

	    var normal = -(vert - zCenter).normalized; 
	    var forward = Vector3.forward;
	    newItem.transform.rotation = Quaternion.LookRotation(forward, normal);

	    newItem.transform.position = vert + normal * 0.5f;
	    newItem.transform.SetParent(currentCylinder.transform, false);
    }
    
    public Transform GetWorldPiece()
    {
	    return pieces[0].transform;
    }
    
    
}

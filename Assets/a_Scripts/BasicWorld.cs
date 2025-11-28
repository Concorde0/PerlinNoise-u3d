using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWorld : MonoBehaviour
{
    
    [SerializeField] private Material meshMaterial; 
    
    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        
    }


    private IEnumerator UpdateWorldPieces()
    {
        
        
        
        yield return 0;
    }

    private GameObject CreateCylinder()
    {
        GameObject newCylinder = new GameObject();
        newCylinder.name = "World piece";
        MeshFilter meshFilter = newCylinder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newCylinder.AddComponent<MeshRenderer>();
        
        meshRenderer.material = meshMaterial;
        meshFilter.mesh = Generate();	
        
        newCylinder.AddComponent<MeshCollider>();
        
        
        return newCylinder;
    }
    
    private Mesh Generate(){
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
        
    }
    
    
}

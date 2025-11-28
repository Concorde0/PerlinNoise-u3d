using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Perlin : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private float _a = 0.06f;
    public bool perlin;
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        var positionArr = new Vector3[100];
        for (int i = 0; i < positionArr.Length; i++)
        {
            if (perlin)
            {
                positionArr[i] = new Vector3(i * 0.1f,Mathf.PerlinNoise(i * _a, i * _a) ,0);
            }
            else
            {
                positionArr[i] = new Vector3(i * 0.1f,Random.value ,0);
            }
            
        }
        _lineRenderer.SetPositions(positionArr);
    }
}

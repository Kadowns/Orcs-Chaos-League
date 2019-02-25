using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour {
    
    [SerializeField] private float _scale = 0.1f;
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _frequency = 1f;


    private Vector3[] _baseHeight;
    private Mesh _mesh;

    private void Start() {
        _mesh = GetComponent<MeshFilter>().mesh;
        _baseHeight = _mesh.vertices;
    }

    void Update() {           

        Vector3[] vertices = new Vector3[_baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 vertex = _baseHeight[i];
            vertex.y += Mathf.Sin(_frequency *
                                  (Time.time * _speed + _baseHeight[i].x + _baseHeight[i].y + _baseHeight[i].z)) *
                        _scale;
            vertices[i] = vertex;
        }

        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();
    }
}
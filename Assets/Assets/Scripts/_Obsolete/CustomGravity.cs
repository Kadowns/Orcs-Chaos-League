using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour {
    // Gravity Scale editable on the inspector
    // providing a gravity scale per object

    public float GravityScale = 1.0f;
    [SerializeField] private float _extraGravity = 5f;
    // Global Gravity doesn't appear in the inspector. Modify it here in the code
    // (or via scripting) to define a different default gravity for all objects.

    public float GlobalGravity = -9.81f;

    Rigidbody _rb;

    void OnEnable() {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    void FixedUpdate() {
       
        Vector3 gravity = GlobalGravity * (GravityScale + _extraGravity) * Vector3.up;
        _rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MagnetCollider : MonoBehaviour {

    public delegate void DetectedColliderDelegate(Collider other);

    public event DetectedColliderDelegate DetectedColliderEvent;

    private void OnTriggerEnter(Collider other) {
        if (DetectedColliderEvent != null)
            DetectedColliderEvent.Invoke(other);
    }
}

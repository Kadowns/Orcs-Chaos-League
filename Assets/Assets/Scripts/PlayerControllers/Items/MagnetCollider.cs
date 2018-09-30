using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MagnetCollider : MonoBehaviour {

    public delegate void DetectedColliderDelegate(Collider other);

    public event DetectedColliderDelegate DetectedColliderEvent;

    private void OnTriggerEnter(Collider other) {
        DetectedColliderEvent?.Invoke(other);
    }
}

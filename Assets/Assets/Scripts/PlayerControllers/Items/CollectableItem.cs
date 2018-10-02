using UnityEngine;

public abstract class CollectableItem : MonoBehaviour, ICollectable, ISpawnable {

	public enum ItemType {
		OrcHead
	};

	public ItemType Type;

	public abstract void Collect();
	
	public abstract void OnSpawn();
}

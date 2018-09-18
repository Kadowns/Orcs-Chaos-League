using UnityEngine;

public abstract class CollectableItem : CustomGravity, ICollectable, ISpawnable {

	public enum ItemType {
		OrcHead
	};

	public ItemType Type;
	
	public float SpawnForce = 100f;

	public float TimeDelayToCollect = 1f;

	protected bool CanBeCollected;
	protected float WillBeAbleToCollect = 0;


	private void Update() {
		if (Time.time > WillBeAbleToCollect) {
			CanBeCollected = true;
		}
	}

	public void Collect() {
		gameObject.SetActive(false);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.CompareTag("Player") && CanBeCollected) {
			var otherEntity = other.gameObject.GetComponent<MovableEntity>();
			var otherMotor = otherEntity.Motor as OrcMotor;
			var otherState = otherEntity.State as OrcEntityState;
			otherMotor.CollectItem(otherState, Type);
			Collect();
		} else if (other.collider.CompareTag("Lava")) {
			Collect();
		}
	}
	public abstract void OnSpawn();
}

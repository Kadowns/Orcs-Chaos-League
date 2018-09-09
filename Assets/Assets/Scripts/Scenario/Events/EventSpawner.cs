using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class EventSpawner : MonoBehaviour {

	public enum EventType {
		RockExplosion,
		GuidedRock
	};

	[SerializeField] private EventType _type;

	private ObjectPooler _pool;

	private IEvent _event;


	private void Start() {
		_pool = ObjectPooler.Instance;
		ChangeEventType(_type);
	}
	
	public void Execute() {

		_event.Execute();
	}
	public void ChangeEventType(EventType type) {
		_type = type;
		GameObject eventObj = null;
		switch (_type) {
			case EventType.RockExplosion:
				eventObj = _pool.SpawnFromPool("RockExplosion", transform.position, Quaternion.identity, transform);
				break;
			case EventType.GuidedRock:
				eventObj = _pool.SpawnFromPool("GuidedRockExplosion", transform.position, Quaternion.identity, transform);
				break;
		}
		_event = eventObj.GetComponent<IEvent>();
	}
}

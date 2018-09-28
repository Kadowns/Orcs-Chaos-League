using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class EventSpawner : MonoBehaviour {

	private ObjectPooler _pool;

	[SerializeField] private IEvent _event;


	private void Start() {
		_pool = ObjectPooler.Instance;
		//ChangeEventType(_type);
	}
	
	public void Execute() {

		_event.Execute();
	}
//	public void ChangeEventType(EventType type) {
//		_type = type;
//		GameObject eventObj = null;
//		switch (_type) {
//			case EventType.RockExplosion:
//				eventObj = _pool.SpawnFromPool("RockExplosion", transform.position, Quaternion.identity, transform);
//				break;
//			case EventType.GuidedRock:
//				eventObj = _pool.SpawnFromPool("GuidedRockExplosion", transform.position, Quaternion.identity, transform);
//				break;
//				case EventType.PotatoBomb:
//					break;
//		}
//		_event = eventObj.GetComponent<IEvent>();
//	}
}

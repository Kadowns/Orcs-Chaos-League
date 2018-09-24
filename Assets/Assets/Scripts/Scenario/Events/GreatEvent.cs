using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GreatEvent : ScriptableObject {


	public enum GreatEventType {
		Eruption
	};

	public GreatEventType EventType;
	
	public float Duration;

	public abstract void Setup(ArenaState state);
	public abstract void Execute(ArenaState state);
	public abstract void Terminate(ArenaState state);
}

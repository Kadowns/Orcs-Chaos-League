using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GreatEvent : MonoBehaviour {

	public float Duration;

	public abstract void Execute();
	public abstract void Terminate();
}

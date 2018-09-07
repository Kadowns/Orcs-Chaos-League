using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FighterColors : SingletonScriptableObject<FighterColors> {

	public Color HurtColor;
	public Color AttackColor;
	public Color AttackPreparationColor;
	public Color ParryColor;
}


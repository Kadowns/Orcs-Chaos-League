using System.Collections;
using System.Collections.Generic;
using OCL;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour {

	[SerializeField] private AudioClip[] _sfx;

	public void PlaySFxByIndex(int index, float pitch) {
		if(index >= _sfx.Length)
			return;
		AudioController.Instance.Play(_sfx[index], AudioController.SoundType.ExclusiveSoundEffect, false, pitch);
	}	
}

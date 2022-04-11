using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

	public AudioClip clipLeaf, clipWater, clipFrog, clipFrog2;
	private AudioSource audio;

	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	void Update () {
	}

	public void SoundOnLeaf() {
		if (clipLeaf != null) {
			audio.PlayOneShot(clipLeaf, 1.0f);
		} 
	}

	public void SoundOnWater() {
		if (clipWater != null) {
			audio.PlayOneShot(clipWater, 1.0f);
		} 
	}

	public void SoundFrog() {
		if (clipFrog != null && clipFrog2 != null) {
			audio.PlayOneShot(clipFrog, 1.0f);
			audio.PlayOneShot(clipFrog2, 1.0f);
		}
	}
}

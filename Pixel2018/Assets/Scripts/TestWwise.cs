using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class TestWwise : MonoBehaviour {

	private bool musicPlaying;
	private bool ambPlaying;

	// Use this for initialization
	void Start () {
		musicPlaying = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Jukebox.instance.PlayGunShot ();
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			musicPlaying = !musicPlaying;
			Jukebox.instance.PlayMusic (musicPlaying);
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			ambPlaying = !ambPlaying;
			Jukebox.instance.PlayAmbiance (ambPlaying);
		}
	}
}

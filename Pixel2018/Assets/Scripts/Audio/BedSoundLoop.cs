using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSoundLoop : MonoBehaviour 
{
	[HideInInspector]
	public string BedSoundName;

	private AudioSource audioSource;

	public bool shouldFade;
	public float maxVolume;
	public float fadeDuration;

	private bool fade;
	private bool isFadeIn;
	private float fadeTime;
	private float initialVolume;
	private float gotoVolume;
	private float currentTime;

	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();

		if (shouldFade) {
			fade = true;
			isFadeIn = true;
			currentTime = 0f;
			fadeTime = fadeDuration;
			initialVolume = 0f;
			gotoVolume = maxVolume;

		}
	}

	public void Stop()
	{
		fade = true;
		isFadeIn = false;
		currentTime = 0f;
		fadeTime = fadeDuration;
		initialVolume = audioSource.volume;
		gotoVolume = 0f;
	}

	void Update () {

		if (fade) {
			currentTime += Time.deltaTime;
			float percFade = Mathf.Clamp (currentTime / fadeTime, 0f, 1f);
			float currentVolume = initialVolume + (percFade * (gotoVolume - initialVolume));
			audioSource.volume = currentVolume;

			if (currentTime >= fadeTime) {
				fade = false;
				if (!isFadeIn) {
					Destroy (gameObject);
				}
			}
		}
	}
}
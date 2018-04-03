using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSound : MonoBehaviour 
{
	private AudioSource audioSource;
	public Action afterSound;

	public bool shouldFade;
	public float maxVolume;
	public float fadeDuration;
	public float percentOfSoundForFadeOut;

	private bool fade;
	private bool isFadeIn;
	private float fadeTime;
	private float initialVolume;
	private float gotoVolume;
	private float currentTime;

	private float audioTimeForFadeOut;

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
			audioTimeForFadeOut = audioSource.clip.length * percentOfSoundForFadeOut;
		} else {
			audioSource.volume = maxVolume;
		}
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
					if (afterSound != null)
						afterSound ();
					
					Destroy (gameObject);
				}
			}
		} else {
			if (shouldFade) {
				if (audioSource.time >= audioTimeForFadeOut) {
					fade = true;
					isFadeIn = false;
					currentTime = 0f;
					fadeTime = fadeDuration;
					initialVolume = audioSource.volume;
					gotoVolume = 0f;
				}
			} else {
				if (!audioSource.isPlaying) {
					if (afterSound != null)
						afterSound ();

					Destroy (gameObject);
				}
			}
		}
	}
}
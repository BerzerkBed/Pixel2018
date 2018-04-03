using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BedMusic : MonoBehaviour 
{
	private bool fade;
	private bool isFadeIn;
	private float initialVolume;
	private float gotoVolume;
	private float fadeTime;
	private float currentTime;

	private AudioSource audioSource;
	private float audioTimeForCallback;

	public Action afterMusic;

	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();
		audioTimeForCallback = audioSource.clip.length * 0.99f;
	}

	void Update()
	{
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

		if (afterMusic != null && (audioSource.time >= audioTimeForCallback)) {
			afterMusic ();
			afterMusic = null;
		}

		if (!audioSource.loop && audioSource.time >= audioSource.clip.length) {
			Destroy (gameObject);
		}
	}

	public void FadeIn(float i_FadeInTime, float i_GotoVolume)
	{
		fade = true;
		isFadeIn = true;
		currentTime = 0f;
		fadeTime = i_FadeInTime;
		initialVolume = 0f;
		gotoVolume = i_GotoVolume;
	}

	public void FadeOut(float i_FadeOutTime)
	{
		fade = true;
		isFadeIn = false;
		currentTime = 0f;
		fadeTime = i_FadeOutTime;
		initialVolume = audioSource.volume;
		gotoVolume = 0f;
	}
}
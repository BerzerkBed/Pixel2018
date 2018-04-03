using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;
using AK;

public class Jukebox : MonoBehaviour 
{
	public static Jukebox instance;

	void Awake()
	{
		instance = this;
	}

	public void PlayGunShot()
	{
		AkSoundEngine.PostEvent ("play_gunshot" , gameObject);
	}

	public void PlayMusic(bool i_Play)
	{
		AkSoundEngine.PostEvent (i_Play ? "play_music" : "stop_music", gameObject);
	}

	public void PlayAmbiance(bool i_Play)
	{
		AkSoundEngine.PostEvent (i_Play ? "play_amb" : "stop_amb", gameObject);
	}
}
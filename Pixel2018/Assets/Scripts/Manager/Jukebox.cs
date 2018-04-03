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
		AkSoundEngine.PostEvent("play_gunshot", gameObject, (uint)AkCallbackType.AK_EndOfEvent, AfterGunShot, this);
	}

	private void AfterGunShot(object in_cookie, AkCallbackType in_type, object in_info)
	{
		if (in_type == AkCallbackType.AK_EndOfEvent)
			Debug.Log ("EndOfGunShot");
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
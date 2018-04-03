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

	public void PlaySound()
	{
		
	}
}
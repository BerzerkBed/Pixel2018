using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackManager : MonoBehaviour 
{
	private class Callback
	{
		private GameObject g;
		private Action a;
		private float t;
		public bool isDestroyed;

		public Callback(GameObject iG , Action iA, float iT)
		{
			g = iG;
			a = iA;
			t = iT;
			isDestroyed = false;
		}

		public void UpdateCallbackTime(float deltaTime)
		{
			if (!isDestroyed) {
				t -= deltaTime;
				if (t <= 0f) {
					isDestroyed = true;
					Call ();
				}
			}
		}

		private void Call()
		{
			if (g != null) {
				if (a != null)
					a ();
			}
		}
	}

	public static CallbackManager instance;

	private List<Callback> allCallback;
	private bool paused;

	void Awake()
	{
		instance = this;

		allCallback = new List<Callback> ();
		paused = false;
	}

	void Update()
	{
		if (!paused) {
			float deltaTime = Time.deltaTime;
			for (int i = allCallback.Count - 1; i >= 0; i--) {
				allCallback [i].UpdateCallbackTime (deltaTime);
				if (allCallback [i].isDestroyed)allCallback.RemoveAt (i);
			}
		}
	}

	public void Pause(bool i_Pause)
	{
		paused = i_Pause;
	}

	public void AddCallback(GameObject iRefG , Action iActionOnEnd , float iTime)
	{
		Callback c = new Callback (iRefG, iActionOnEnd, iTime);
		allCallback.Add (c);
	}
}
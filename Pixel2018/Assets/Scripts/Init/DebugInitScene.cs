using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugInitScene : MonoBehaviour 
{
	private bool gameCreated;
	void Awake()
	{
		gameCreated = false;
		if (GameManager.instance == null) {
			GameObject.Instantiate (Resources.Load("GameManager"));
			gameCreated = true;
			GameManager.instance.ShowInGameWindow ();
			GameManager.instance.SpawnHeroDebug ();
		}
	}

	void Start()
	{
		if (!gameCreated) {
			Destroy (gameObject);
		}
	}

	void Update()
	{
		if (gameCreated) {
			TransitionManager.instance.ActivateAfterFade ();
		}
		Destroy (gameObject);
	}
}
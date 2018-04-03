using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour 
{
	public static TransitionManager instance;
	private string sceneToLoad;

	private Action loadSceneCallback;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		SceneManager.sceneLoaded += SceneLoaded;
	}

	void OnDestroy()
	{
		SceneManager.sceneLoaded -= SceneLoaded;
	}

	public void TransitionTo(string i_SceneToLoad, Action i_LoadSceneCallback)
	{
		sceneToLoad = i_SceneToLoad;
		loadSceneCallback = i_LoadSceneCallback;

		TransitionWindow transitionWindow = (TransitionWindow)WindowManager.instance.ShowWindow (WindowID.ID.TransitionWindow);
		transitionWindow.OnEndFadeIn = LoadScene;
		transitionWindow.OnActivateAfterFade = ActivateAfterFade;
	}
		
	private void LoadScene()
	{
		if(loadSceneCallback != null)loadSceneCallback ();
		SceneManager.LoadScene (sceneToLoad, LoadSceneMode.Single);
	}

	public void ActivateAfterFade()
	{

	}

	private void SceneLoaded(Scene i_Scene, LoadSceneMode i_Mode)
	{
		GameManager.instance.ShowInGameWindow ();
		GameManager.instance.SpawnHero (Vector3.zero);
	}
}
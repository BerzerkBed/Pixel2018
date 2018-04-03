using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	static public GameManager instance;

	[HideInInspector]
	public InGameWindow inGameWindow;

	private List<GameObject> allActorToPause;

	[HideInInspector]
	public bool IsPaused;

	void Awake()
	{
		Application.targetFrameRate = 60;

		IsPaused = false;
		instance = this;

		allActorToPause = new List<GameObject> ();
	}

	void Start()
	{
		DontDestroyOnLoad (this);
	}

	public void Pause(bool i_Pause)
	{
		for(int i = allActorToPause.Count - 1 ; i >= 0 ; i--)
		{
			if (allActorToPause [i] == null) {
				allActorToPause.RemoveAt (i);
			} else {
				allActorToPause [i].SendMessage ("Pause", i_Pause);
			}
		}
		CallbackManager.instance.Pause (i_Pause);

		IsPaused = i_Pause;
	}

	public void RegisterActorToPause(GameObject i_ActorToPause)
	{
		allActorToPause.Add (i_ActorToPause);
	}

	public void SpawnHero(Vector3 pos)
	{
		//GameObject heroGO = GameObject.Instantiate (GetHeroPrefab()) as GameObject;
		//heroGO.transform.SetPositionAndRotation (new Vector3(pos.x , pos.y , 0f), Quaternion.identity);
		CameraManager.instance.CreateCam ();
	}

	public void SpawnHeroDebug()
	{
		SpawnHero (Vector3.zero);
	}

	public void ShowInGameWindow()
	{
		if(inGameWindow == null)
			inGameWindow = (InGameWindow)WindowManager.instance.ShowWindow (WindowID.ID.InGameWindow);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour 
{
	void Awake()
	{
		if(GameManager.instance == null)
			GameObject.Instantiate (Resources.Load("GameManager"));
	}

	void Start()
	{
		WindowManager.instance.ShowWindow (WindowID.ID.TitleWindow);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	static public CameraManager instance;

	public GameObject GameCamera_GO;
	[HideInInspector]
	public FollowTargetCam followCam;

	void Awake()
	{
		instance = this;
	}

	public void CreateCam()
	{
		GameObject go = (GameObject)GameObject.Instantiate (GameCamera_GO);
		followCam = go.GetComponent<FollowTargetCam> ();
		//followCam.target = BloodyMuscleManager.instance.hero.transform;
	}
}

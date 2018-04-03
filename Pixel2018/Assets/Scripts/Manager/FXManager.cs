using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour 
{
	public static FXManager instance;

	void Awake()
	{
		instance = this;
	}

	public GameObject CreateStaticFX(float px , float py , string i_FXName, float scaleX = 1f , float scaleY = 1f)
	{
		PoolObject p = PoolManager.instance.GetFromPool (i_FXName , px , py , scaleX , scaleY);
		return p.gameObject;
	}
}
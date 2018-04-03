using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {


	public bool returnOnTransition;
	[HideInInspector]
	public string poolObjectName;
	[HideInInspector]
	public Transform myTransform;
	[HideInInspector]
	public bool isActive;

	virtual protected void Awake()
	{
		myTransform = transform;

		if (returnOnTransition)PoolManager.instance.RegisterToRemoveOnTransition (this);
	}

	virtual public void OutOfPool()
	{
		
	}

	public void ReturnToPool()
	{
		PoolManager.instance.ReturnToPool (this);
	}
}

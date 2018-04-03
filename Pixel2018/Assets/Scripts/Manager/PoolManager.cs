using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour 
{
	public static PoolManager instance;

	private Transform myTransform;

	private Dictionary<string , List<PoolObject>> pool;
	private List<PoolObject> returnOnTransition = new List<PoolObject>();

	void Awake()
	{
		instance = this;

		myTransform = transform;

		pool = new Dictionary<string, List<PoolObject>> ();
		returnOnTransition = new List<PoolObject> ();

	/*	CreatePoolObject ("FXBloodPool", 15);*/
	}

	private void CreatePoolObject(string i_FXName , int i_Num)
	{
		List<PoolObject> poolToAdd = new List<PoolObject> ();
		for (int i = 0; i < i_Num; i++) {
			GameObject fx = GameObject.Instantiate (Resources.Load("FX/" + i_FXName)) as GameObject;
			fx.transform.SetParent (myTransform);
			fx.transform.position = new Vector2 (-10000f, -10000f);
			fx.transform.localScale = new Vector2 (1f, 1f);
			PoolObject p = fx.GetComponent<PoolObject> ();
			p.poolObjectName = i_FXName;
			fx.SetActive (false);
			p.isActive = false;
			poolToAdd.Add (p);
		}

		pool.Add (i_FXName, poolToAdd);
	}

	private PoolObject CreateSinglePoolObject(string i_FXName)
	{
		GameObject fx = GameObject.Instantiate (Resources.Load("FX/" + i_FXName)) as GameObject;
		fx.transform.SetParent (myTransform);
		fx.transform.position = new Vector2 (-10000f, -10000f);
		fx.transform.localScale = new Vector2 (1f, 1f);
		PoolObject p = fx.GetComponent<PoolObject> ();
		p.poolObjectName = i_FXName;
		p.isActive = true;
		fx.SetActive (true);
		p.OutOfPool ();

		Debug.Log ("CREATING NEW : " + i_FXName);
		return p;
	}

	public PoolObject GetFromPool(string i_FXName, float px , float py , float scaleX , float scaleY)
	{
		if (pool.ContainsKey (i_FXName)) {
			if (pool [i_FXName].Count > 0) {
				PoolObject p = pool [i_FXName] [0];
				pool [i_FXName].RemoveAt (0);
				p.gameObject.SetActive (true);
				p.myTransform.position = new Vector2 (px, py);
				p.myTransform.localScale = new Vector2 (scaleX, scaleY);
				p.isActive = true;
				p.OutOfPool ();
				return p;
			} else {
				return CreateSinglePoolObject (i_FXName);
			}
		}

		return null;
	}

	public void ReturnToPool(PoolObject i_PoolObject)
	{
		if (pool.ContainsKey (i_PoolObject.poolObjectName)) {
			i_PoolObject.isActive = false;
			i_PoolObject.myTransform.position = new Vector2 (-10000f, -10000f);
			i_PoolObject.gameObject.SetActive (false);
			pool [i_PoolObject.poolObjectName].Add (i_PoolObject);
		}
	}

	public void RegisterToRemoveOnTransition(PoolObject i_PoolObject)
	{
		returnOnTransition.Add (i_PoolObject);
	}

	public void ReturnObjectOnTransition()
	{
		for (int i = 0; i < returnOnTransition.Count; i++) {
			if (returnOnTransition [i].isActive) {
				returnOnTransition [i].ReturnToPool ();
			}
		}
	}
}
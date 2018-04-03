using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour 
{
	private FlumpMovie myMovie;

	void Start () {

		myMovie = FlumpMovieManager.Instance.AddMovie ("START_IDLE", 0f, 0f);
		myMovie.m_Transform.SetParent (transform);
		myMovie.m_Transform.localPosition = Vector3.zero;
		myMovie.m_Transform.localScale = Vector3.one;
		myMovie.m_Transform.localRotation = Quaternion.identity;
		myMovie.Play (0, 1, true, -1);

		myMovie.AddAnimationCallback (10, Caca);
		myMovie.AddEndCallback (Caca2);
	}

	private void Caca()
	{
		Debug.Log("Caca");
	}

	private void Caca2()
	{
		Debug.Log("Caca2");
	}
}
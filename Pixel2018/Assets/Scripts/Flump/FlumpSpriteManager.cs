using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlumpSpriteManager : MonoBehaviour {

	private static FlumpSpriteManager m_Instance;
	public static FlumpSpriteManager Instance
	{
		get{return m_Instance;}
	}

	void Awake()
	{
		//DontDestroyOnLoad (this);
		
		m_Instance = this;	
	}

	public List<FlumpSprite> m_FlumpSpriteList;

	public GameObject m_FlumpSpritePrefab;

	public bool AlreadyCreated(string i_SpriteName)
	{
		for(int i = 0 ; i < m_FlumpSpriteList.Count ; i++)
		{
			if(m_FlumpSpriteList[i].m_SpriteName == i_SpriteName)
			{
				return true;
			}
		}

		return false;
	}

	public FlumpSprite CreateSprite(float i_OffSetX , float i_OffSetY, string i_SpriteName)
	{
		GameObject go = (GameObject)GameObject.Instantiate(m_FlumpSpritePrefab);
		FlumpSprite flumpSprite = go.GetComponent<FlumpSprite>();
		flumpSprite.SetData(i_OffSetX , i_OffSetY , i_SpriteName);
		go.transform.parent = gameObject.transform;
		go.SetActive(false);

		m_FlumpSpriteList.Add(flumpSprite);

		return flumpSprite;
	}

	public FlumpSprite GetSprite(string i_Name)
	{
		FlumpSprite reference = null;
		for(int i = 0 ; i < m_FlumpSpriteList.Count ; i++)
		{
			if(m_FlumpSpriteList[i].m_SpriteName == i_Name)
			{
				reference = m_FlumpSpriteList[i];
				if(m_FlumpSpriteList[i].m_Available)
				{
					FlumpSprite returnSprite = m_FlumpSpriteList[i];
					return returnSprite;
				}
			}
		}

		if(reference != null)
		{
			return CreateSprite(reference.m_OffSetX , reference.m_OffSetY , reference.m_SpriteName);
		}

		return null;
	}

	public FlumpSprite AddSprite(string i_SpriteName , float i_PosX , float i_PosY)
	{
		FlumpSprite sprite = GetSprite(i_SpriteName);
		if(sprite != null)
		{
			sprite.gameObject.SetActive(true);
			sprite.gameObject.transform.position = Vector2.zero;
			sprite.m_Available = false;
			return sprite;
		}

		Debug.Log ("Couldn't find : " + i_SpriteName);
		return null;
	}

	public void ReturnSprite(FlumpSprite i_Sprite)
	{
		//m_FlumpSpriteList.Add(i_Sprite);
		i_Sprite.gameObject.SetActive(false);
		i_Sprite.transform.SetParent (transform);
		i_Sprite.m_Available = true;
	}
}
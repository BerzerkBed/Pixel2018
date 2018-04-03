using UnityEngine;
using System.Collections;

public class FlumpSprite : MonoBehaviour {

	public SpriteRenderer m_SpriteRenderer;

	[HideInInspector]
	public string m_SpriteName;
	[HideInInspector]
	public Sprite m_Sprite;
	[HideInInspector]
	public float m_OffSetX;
	[HideInInspector]
	public float m_OffSetY;
	[HideInInspector]
	public bool m_Available;

	public void SetData(float i_OffsetX , float i_OffsetY, string i_SpriteName)
	{
		if (!i_SpriteName.Contains ("Locator")) {
			m_Sprite = (Sprite)Resources.Load<Sprite> ("Flump/SpriteExport/" + i_SpriteName + "img");
			m_SpriteRenderer.sprite = m_Sprite;
			m_SpriteRenderer.gameObject.transform.localPosition = new Vector2 (i_OffsetX, i_OffsetY);
		} 
		else 
		{
			m_SpriteRenderer.enabled = false;
		}

		transform.position = Vector2.zero;

		m_OffSetX = i_OffsetX;
		m_OffSetY = i_OffsetY;

		m_SpriteName = i_SpriteName;
		gameObject.name = m_SpriteName;
		m_Available = true;
	}

	public void SetLayer(int i_SortingOrder , float i_Layer , float i_LocatorLayer)
	{
		m_SpriteRenderer.sortingOrder = i_SortingOrder;
		if (i_LocatorLayer >= 0f) 
		{
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -(i_Layer * 0.001f));
		} 
		else 
		{
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -i_Layer * 0.01f);
		}
	}
}
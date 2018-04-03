using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlumpMovie : MonoBehaviour 
{
	public class Layer
	{
		public List<KeyFrame> m_KeyFrames;
		private KeyFrame m_ActiveKeyFrame;
		private int m_ActiveKeyFrameIndex;

		public Layer(List<KeyFrame> i_KeyFrames)
		{
			m_KeyFrames = i_KeyFrames;

			m_ActiveKeyFrame = null;
			m_ActiveKeyFrameIndex = 0;
		}

		public void SetFrame(int i_Frame , FlumpMovie i_FlumpMovie , int i_Layer)
		{
			// Frame Actif Expire
			if (m_ActiveKeyFrame != null && i_Frame == 1) {
				m_ActiveKeyFrameIndex = 0;
				m_ActiveKeyFrame.Disable (i_FlumpMovie);
				m_ActiveKeyFrame = null;
			} else if (m_ActiveKeyFrame != null && i_Frame >= m_ActiveKeyFrame.LastFrame ()) {
				m_ActiveKeyFrameIndex++;
				m_ActiveKeyFrame.Disable (i_FlumpMovie);
				m_ActiveKeyFrame = null;
			} else if (m_ActiveKeyFrame != null && m_ActiveKeyFrameIndex < m_KeyFrames.Count - 1) {
				m_ActiveKeyFrame.Tween (i_Frame , m_KeyFrames[m_ActiveKeyFrameIndex + 1] , i_Layer , i_FlumpMovie);
			}

			// Aucun Frame actif, mais mon next keyframe est actif
			if(m_ActiveKeyFrame == null && m_ActiveKeyFrameIndex < m_KeyFrames.Count && i_Frame >= m_KeyFrames[m_ActiveKeyFrameIndex].m_Index)
			{
				m_ActiveKeyFrame = m_KeyFrames[m_ActiveKeyFrameIndex];
				m_ActiveKeyFrame.Activate(i_FlumpMovie , i_Layer);
			}
		}

		public void RemoveMovies(FlumpMovie i_FlumpMovie)
		{
			if (m_ActiveKeyFrame != null) 
			{
				m_ActiveKeyFrame.Disable (i_FlumpMovie);
			}
			m_ActiveKeyFrame = null;
			m_ActiveKeyFrameIndex = 0;
		}
	}

	public class KeyFrame
	{
		public Vector2 m_Position;
		public Vector2 m_Pivot;
		public Vector2 m_Rotation;
		public Vector3 m_Scale;
		public int m_Ease;
		public int m_Duration;
		public int m_Index;
		public bool m_Tweened;

		public string m_Ref;

		public FlumpSprite m_Sprite;

		public KeyFrame()
		{
			m_Position = Vector2.zero;
			m_Pivot = Vector2.zero;
			m_Rotation = Vector2.zero;
			m_Scale = Vector3.one;
			m_Ease = 0;
			m_Duration = 0;
			m_Index = 0;
			m_Ref = "";
			m_Tweened = false;
		}

		public KeyFrame Copy()
		{
			KeyFrame copy = new KeyFrame();
			copy.m_Position = m_Position;
			copy.m_Pivot = m_Pivot;
			copy.m_Rotation = m_Rotation;
			copy.m_Scale = m_Scale;
			copy.m_Ease = m_Ease;
			copy.m_Duration = m_Duration;
			copy.m_Index = m_Index;
			copy.m_Ref = m_Ref;
			copy.m_Tweened = m_Tweened;
			return copy;
		}

		public Vector3 GetPosition()
		{
			return m_Position;
		}

		public float GetTweenPercentage(int i_CurrentFrame)
		{
			return ((float)i_CurrentFrame - (float)m_Index) / (float)m_Duration;
		}

		public void Disable(FlumpMovie i_FlumpMovie)
		{
			i_FlumpMovie.RemoveLocator (m_Sprite);
			if(m_Sprite != null)
			{
				FlumpSpriteManager.Instance.ReturnSprite(m_Sprite);
			}
		}

		public void Activate(FlumpMovie i_FlumpMovie , int i_Layer)
		{
			if (string.IsNullOrEmpty (m_Ref))
				return;
			
			m_Sprite = FlumpSpriteManager.Instance.AddSprite(m_Ref , 0 , 0);
			m_Sprite.transform.SetParent(i_FlumpMovie.m_Transform);
			m_Sprite.transform.localPosition = GetPosition ();
			m_Sprite.transform.localRotation = Quaternion.identity;
			m_Sprite.SetLayer (i_FlumpMovie.m_SortingOrder , i_Layer , i_FlumpMovie.m_LocatorLayer);
			ApplyRotation (m_Sprite.transform , -m_Rotation.x);
			m_Sprite.transform.localScale = m_Scale;

			if (m_Sprite.m_SpriteName.Contains ("Locator")) 
			{
				i_FlumpMovie.AddLocator (m_Sprite , i_Layer);
			}
			/*if (m_Sprite.m_SpriteName == "Locator_Staff") 
			{
				m_Movie = FlumpMovieManager.Instance.AddMovie ("Movie_Staff" , 0f , 0f);
				m_Movie.transform.SetParent (m_Sprite.transform);
				m_Movie.transform.localPosition = Vector2.zero;
				m_Movie.transform.localRotation = Quaternion.identity;
				m_Movie.Play(true , i_Layer);
			}*/
		}

		public void Tween(int i_CurrentFrame , KeyFrame i_NextKeyFrame , int i_Layer , FlumpMovie i_FlumpMovie)
		{
			if (m_Sprite != null) 
			{
				m_Sprite.transform.localPosition = GetPosition() + ((i_NextKeyFrame.GetPosition() - GetPosition()) * GetTweenPercentage(i_CurrentFrame));
				m_Sprite.SetLayer (i_FlumpMovie.m_SortingOrder , i_Layer , i_FlumpMovie.m_LocatorLayer);
				ApplyRotation (m_Sprite.transform, -(m_Rotation.x + ((i_NextKeyFrame.m_Rotation.x - m_Rotation.x) * GetTweenPercentage(i_CurrentFrame))));
				m_Sprite.transform.localScale = m_Scale + ((i_NextKeyFrame.m_Scale - m_Scale) * GetTweenPercentage(i_CurrentFrame));
			} 
		}

		private void ApplyRotation(Transform i_Transform , float i_Rotation)
		{
			if (i_Rotation != 0f) 
			{
				i_Transform.localRotation = Quaternion.Euler (0f, 0f, Mathf.Rad2Deg * i_Rotation);
			}
		}

		public int LastFrame()
		{
			return m_Index + m_Duration;
		}
	}

	[HideInInspector]
	public string m_MovieName;
	[HideInInspector]
	public int m_FrameRate;
	[HideInInspector]
	public List<Layer> m_Layers;
	[HideInInspector]
	public Transform m_Transform;

	private bool m_Play;
	private bool m_Loop;
	[HideInInspector]
	public int m_CurrentFrame;
	private int m_LastFrame;
	private int m_MovieDuration;
	private float m_FrameCounter;
	private float m_FrameTime;

	[HideInInspector]
	public int m_LocatorLayer;
	[HideInInspector]
	public bool m_Available;

	private int m_SortingOrder;

	private List<Action> m_EndsCallback;
	private List<AnimationCallback> m_AnimationsCallback;

	private Action<FlumpSprite , int> m_AddLocatorCallback = null;
	private Action<FlumpSprite> m_RemoveLocatorCallback = null;

	public void ReturnMovies()
	{
		for (int i = 0; i < m_Layers.Count; i++) 
		{
			m_Layers [i].RemoveMovies(this);
		}

		m_AddLocatorCallback = null;
		m_RemoveLocatorCallback = null;
	}

	public void CopyData(FlumpMovie i_FlumpMovie)
	{
		m_MovieName = i_FlumpMovie.m_MovieName;
		m_FrameRate = i_FlumpMovie.m_FrameRate;
		gameObject.name = m_MovieName;
		m_Available = true;

		m_Layers = new List<Layer> ();
		for (int i = 0; i < i_FlumpMovie.m_Layers.Count; i++) 
		{
			Layer currentLayer = i_FlumpMovie.m_Layers [i];

			List<KeyFrame> keyFramesToAdd = new List<KeyFrame> ();
			for (int j = 0; j < currentLayer.m_KeyFrames.Count; j++) 
			{
				KeyFrame currentKeyFrame = currentLayer.m_KeyFrames [j];
				keyFramesToAdd.Add (currentKeyFrame.Copy ());
			}
			m_Layers.Add (new Layer (keyFramesToAdd));
		}
	}

	public void SetData(List<Layer> i_Layers , int i_FrameRate , string i_MovieName)
	{
		m_Layers = i_Layers;
		m_FrameRate = i_FrameRate;
		m_MovieName = i_MovieName;
		gameObject.name = m_MovieName;

		m_Play = false;
		m_Available = true;
	}

	public void AddLocatorCallbacks(Action<FlumpSprite , int> i_AddLocatorCallback , Action<FlumpSprite> i_RemoveLocatorCallback)
	{
		m_AddLocatorCallback = i_AddLocatorCallback;
		m_RemoveLocatorCallback = i_RemoveLocatorCallback;
	}

	public void Play(int i_SortingOrder , int i_StartingFrame = 1, bool i_Loop = false , int i_LocatorLayer = -1)
	{
		m_AnimationsCallback = new List<AnimationCallback> ();
		m_EndsCallback = new List<Action> ();

		m_Play = true;
		m_Loop = i_Loop;
		m_LocatorLayer = i_LocatorLayer;
		m_SortingOrder = i_SortingOrder;

		m_CurrentFrame = i_StartingFrame;
		m_LastFrame = 0;
		m_FrameTime = 1f / (float)m_FrameRate;
		m_FrameCounter = ((float)m_CurrentFrame - 1f) * m_FrameTime;

		m_MovieDuration = -1;
		for (int i = 0; i < m_Layers.Count; i++) 
		{
			for (int j = 0; j < m_Layers [i].m_KeyFrames.Count; j++) 
			{
				int keyframeLastFrame = m_Layers [i].m_KeyFrames [j].LastFrame();
				if (keyframeLastFrame > m_MovieDuration) 
				{
					m_MovieDuration = keyframeLastFrame;
				}
			}
		}

		UpdateFrameCounter ();
	}

	void Awake()
	{
		m_Transform = transform;
	}

	void Update()
	{
		if(m_Play)
		{
			UpdateFrameCounter ();
		}
	}

	void UpdateFrameCounter()
	{
		m_FrameCounter += Time.deltaTime;

		m_CurrentFrame = Mathf.CeilToInt(m_FrameCounter / m_FrameTime);
		if (m_CurrentFrame != m_LastFrame) 
		{
			DoAnimationCallbacks(m_CurrentFrame);
			if (m_CurrentFrame >= m_MovieDuration) 
			{
				DoEndCallbacks();

				if (m_Loop) {
					m_CurrentFrame = 1;
					m_FrameCounter = 0f;
				} 
				else 
				{
					FlumpMovieManager.Instance.ReturnMovie (this);
				}
			}
			UpdateLayers();
			m_LastFrame = m_CurrentFrame;
		}
	}

	private void UpdateLayers()
	{
		for(int i = m_Layers.Count - 1 ; i >= 0 ; i--)
		{
			m_Layers[i].SetFrame(m_CurrentFrame , this , i);
		}
	}

	public class Locator
	{
		public int m_Layer;
		public FlumpSprite m_Sprite;

		public Locator(int i_Layer , FlumpSprite i_Sprite)
		{
			m_Layer = i_Layer;
			m_Sprite = i_Sprite;
		}
	}

	public void AddLocator(FlumpSprite i_Sprite , int i_LocatorLayer)
	{
		if (m_AddLocatorCallback != null)m_AddLocatorCallback (i_Sprite , i_LocatorLayer);
	}

	public void RemoveLocator(FlumpSprite i_Sprite)
	{
		if(m_RemoveLocatorCallback != null)m_RemoveLocatorCallback (i_Sprite);
	}

	public void AddEndCallback(Action i_EndCallback)
	{
		m_EndsCallback.Add (i_EndCallback);
	}

	private void DoEndCallbacks()
	{
		for (int i = m_EndsCallback.Count - 1 ; i >= 0 ; i--) 
		{
			m_EndsCallback [i].Invoke ();
			m_EndsCallback.RemoveAt (i);
		}
	}

	public class AnimationCallback
	{
		public int m_FrameToCallback;
		public Action m_AnimationCallback;

		public AnimationCallback(int i_Frame , Action i_AnimationCallback)
		{
			m_FrameToCallback = i_Frame;
			m_AnimationCallback = i_AnimationCallback;
		}
	}

	public void AddAnimationCallback(int i_Frame , Action i_AnimationCallback)
	{
		m_AnimationsCallback.Add(new AnimationCallback(i_Frame , i_AnimationCallback));
	}

	private void DoAnimationCallbacks(int i_CurrentFrame)
	{
		for (int i = m_AnimationsCallback.Count - 1; i >= 0; i--) 
		{
			if (m_AnimationsCallback [i].m_FrameToCallback == i_CurrentFrame) 
			{
				m_AnimationsCallback [i].m_AnimationCallback.Invoke ();
				m_AnimationsCallback.RemoveAt (i);
			}
		}
	}
}
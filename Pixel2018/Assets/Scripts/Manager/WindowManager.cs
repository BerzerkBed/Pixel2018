using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

	static public WindowManager instance;

	private Canvas canvas;

	private List<Window> allWindows;

	void Awake()
	{
		instance = this;
		canvas = GetComponentInChildren<Canvas> ();
		allWindows = new List<Window>();
	}

	public bool isAnyPopupActive()
	{
		for (int i = 0; i < allWindows.Count; i++) {
			Window w = allWindows [i];
			if (w.windowID == WindowID.ID.TransitionWindow)
				return true;
		}

		return false;
	}

	public Window ShowWindow(WindowID.ID i_WindowID, bool i_PushOnTop = false)
	{
		GameObject go = GameObject.Instantiate(Resources.Load ("UI/" + i_WindowID.ToString ())) as GameObject;
		go.transform.SetParent (canvas.transform);
		go.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		go.GetComponent<RectTransform> ().localScale = Vector3.one;
		if (i_PushOnTop)go.transform.SetAsLastSibling ();
		Window window = go.GetComponent<Window> ();
		allWindows.Add (window);
		return window;
	}

	public void CloseWindow(WindowID.ID i_WindowID)
	{
		for (int i = allWindows.Count - 1; i >= 0; i--) {
			Window window = allWindows [i];
			if (window.windowID == i_WindowID) {
				Destroy (window.gameObject);
				allWindows.RemoveAt (i);
			}
		}
	}

	public bool IsWindowActive(WindowID.ID i_WindowID)
	{
		for (int i = allWindows.Count - 1; i >= 0; i--) {
			Window window = allWindows [i];
			if (window.windowID == i_WindowID) {
				return true;
			}
		}

		return false;
	}
}
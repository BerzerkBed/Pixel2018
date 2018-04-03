using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TitleWindow : Window {

	private bool listenToControl;

	public override void Start ()
	{
		base.Start ();

		listenToControl = true;
	}

	public override void Update ()
	{
		base.Update ();

		if (listenToControl && MyInputManager.instance.IsAnyActionDone()){
			listenToControl = false;
			TransitionManager.instance.TransitionTo ("Game" , CloseMe);
		}
	}

	private void CloseMe()
	{
		WindowManager.instance.CloseWindow (WindowID.ID.TitleWindow);
	}
}
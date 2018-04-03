using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransitionWindow : Window {

	public Action OnEndFadeIn;
	public Action OnActivateAfterFade;
	private Animator animator;

	public override void Start ()
	{
		base.Start ();

		animator = GetComponent<Animator> ();
	}

	public void EndFadeIn()
	{
		if (OnEndFadeIn != null)
			OnEndFadeIn ();

		animator.SetTrigger ("FadeOut");
	}

	public void ActivateAfterFade()
	{
		if (OnActivateAfterFade != null)
			OnActivateAfterFade ();
	}

	public void EndFadeOut()
	{
		WindowManager.instance.CloseWindow (WindowID.ID.TransitionWindow);
	}
}
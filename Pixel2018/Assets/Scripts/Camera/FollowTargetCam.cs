using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowTargetCam : MonoBehaviour 
{
	[HideInInspector]
	public Transform target;

	private Rect m_BoundariesRect;

	private Transform cameraTransform;
	[HideInInspector]
	public Camera cam;

	private bool freezeY;

	private float shake;
	private float ShakeAppliedX;
	private float ShakeAppliedY;

	private float constantShake;

	private bool gotoPos;
	private float lerpGotoPos;
	private float lerpSpeed;
	private Vector3 initialPosV;
	private Vector3 gotoPosV;
	private Action EndGotoPos;

	private float CamPixelPerfectZoom;
	private bool zoom;
	private float zoomLerp;
	private float initialZoom;
	private float gotoZoom;
	private float zoomSpeed;
	private bool easeIn;
	private Action onEndZoom;

	void Awake()
	{
		cameraTransform = transform;
		cam = GetComponent<Camera> ();
		freezeY = false;

		constantShake = 0f;
		shake = 0f;
		ShakeAppliedX = 0f;
		ShakeAppliedY = 0f;

		gotoPos = false;
		lerpGotoPos = 0f;
		initialPosV = new Vector2 (0f, 0f);
		gotoPosV = new Vector2 (0f, 0f);
		EndGotoPos = null;

		zoom = false;
	}

	void Start()
	{
		CamPixelPerfectZoom = cam.orthographicSize;

		Transform cubeBoundaries = GameObject.Find ("CamBoundaries").transform;
		m_BoundariesRect = new Rect (cubeBoundaries.position.x, cubeBoundaries.position.y, cubeBoundaries.localScale.x, cubeBoundaries.localScale.y);
		GameManager.instance.RegisterActorToPause (gameObject);
	}

	public void Pause(bool i_Pause)
	{
		
	}

	public void ZoomAt(float i_Perc , float i_ZoomSpeed, bool i_EaseIn , Action i_OnEndZoom)
	{
		zoom = true;
		zoomLerp = 0f;
		initialZoom = cam.orthographicSize;
		gotoZoom = CamPixelPerfectZoom * i_Perc;
		easeIn = i_EaseIn;
		zoomSpeed = i_ZoomSpeed;
		onEndZoom = i_OnEndZoom;
	}

	void FixedUpdate () 
	{
		if (gotoPos) {
			lerpGotoPos += (Time.deltaTime * lerpSpeed);
			Vector3 targetPos = Vector3.Lerp (initialPosV, gotoPosV, lerpGotoPos);
			cameraTransform.SetPositionAndRotation (targetPos, Quaternion.identity);
			if (lerpGotoPos >= 1f) {
				lerpGotoPos = 1f;
				gotoPos = false;
				if (EndGotoPos != null) {
					EndGotoPos ();
					EndGotoPos = null;
				}
			}
		} else if(target != null) {
			Vector3 targetPos = new Vector3 (target.transform.position.x, target.transform.position.y, cameraTransform.position.z);
			cameraTransform.SetPositionAndRotation (targetPos, Quaternion.identity);
		}

		if (zoom) {
			if (easeIn) {
				zoomSpeed *= 1.05f;
			} else {
				zoomSpeed *= 0.95f;
			}
			zoomLerp += (Time.deltaTime * zoomSpeed);
			float newOrthographicSize = Mathf.Lerp (initialZoom, gotoZoom, zoomLerp);
			if (zoomLerp >= 1f) {
				newOrthographicSize = gotoZoom;
				zoom = false;
				if (onEndZoom != null) {
					onEndZoom ();
					onEndZoom = null;
				}
			}
			cam.orthographicSize = newOrthographicSize;
		}

		if (shake > 0) {
			shake *= 0.9f;
			if (shake <= 0.01f)
				shake = 0f;
		}
			
		ResepectBoundaries ();
	}

	public void FollowActor(Transform i_Transform)
	{
		target = i_Transform;
		gotoPos = false;
	}

	public void Shake(float i_ShakeForce)
	{
		shake += i_ShakeForce;
	}

	public void SetConstantShake(float i_ConstantShake)
	{
		constantShake = i_ConstantShake;
	}

	public void GotoPos(float gotoX , float gotoY, float i_LerpSpeed, Action i_EndGotoCallback)
	{
		gotoPos = true;
		target = null;
		lerpGotoPos = 0f;
		lerpSpeed = i_LerpSpeed;
		initialPosV = cameraTransform.position;
		gotoPosV.x = gotoX;
		gotoPosV.y = gotoY;
		gotoPosV.z = initialPosV.z;
		EndGotoPos = i_EndGotoCallback;
	}

	private void ResepectBoundaries()
	{
		//float ratioPixels = 6.25f;//100f / 16f;

		float height = 2f * cam.orthographicSize;
		float camhalfHeight = height * 0.5f; //1080f * 0.005f * ratioPixels;
		float camhalfWidth = height * cam.aspect * 0.5f; // 1920f * 0.005f * ratioPixels;
		Vector3 currentPos = cameraTransform.position;

		float boundariesHalfWidth = m_BoundariesRect.width * 0.5f;
		float boundariesHalfHeight = m_BoundariesRect.height * 0.5f;
		if (boundariesHalfHeight < camhalfHeight)
			freezeY = true;

		float boundariesLeft = m_BoundariesRect.x - boundariesHalfWidth;
		float boundariesRight = m_BoundariesRect.x + boundariesHalfWidth;
		float boundariesUp = m_BoundariesRect.y + boundariesHalfHeight;
		float boundariesDown =m_BoundariesRect.y - boundariesHalfHeight;

		Vector3 newPos = currentPos;

		RemoveLastShake (ref newPos);

		if ((currentPos.x - camhalfWidth) < boundariesLeft) newPos.x = boundariesLeft + camhalfWidth; 
		if ((currentPos.x + camhalfWidth) > boundariesRight) newPos.x = boundariesRight - camhalfWidth; 

		if (freezeY) {
			newPos.y = m_BoundariesRect.y;
		} else {
			if ((currentPos.y + camhalfHeight) > boundariesUp) newPos.y = boundariesUp - camhalfHeight; 
			if ((currentPos.y - camhalfHeight) < boundariesDown) newPos.y = boundariesDown + camhalfHeight; 
		}
			
		ApplyShake (ref newPos);
		RoundToPixels (ref newPos);
		cameraTransform.position = newPos;
	}

	// Make sur the camera is never in between 2 pixels and make everything flicker
	private void RoundToPixels(ref Vector3 newPos)
	{
		float valueXInPixel = newPos.x * 16f;
		valueXInPixel = Mathf.Round (valueXInPixel);
		newPos.x = valueXInPixel * (1f / 16f);

		float valueYInPixel = newPos.y * 16f;
		valueYInPixel = Mathf.Round (valueYInPixel);
		newPos.y = valueYInPixel * (1f / 16f);
	}

	private void RemoveLastShake(ref Vector3 newPos)
	{
		newPos.x -= ShakeAppliedX;
		newPos.y -= ShakeAppliedY;
	}

	private void ApplyShake(ref Vector3 newPos)
	{
		ShakeAppliedX = UnityEngine.Random.Range (-shake - constantShake, shake + constantShake);
		ShakeAppliedY = UnityEngine.Random.Range (-shake - constantShake, shake + constantShake);
		newPos.x += ShakeAppliedX;
		newPos.y += ShakeAppliedY;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (new Vector3 (m_BoundariesRect.x, m_BoundariesRect.y, 0f), new Vector3 (m_BoundariesRect.width, m_BoundariesRect.height, 0f));
	}
}
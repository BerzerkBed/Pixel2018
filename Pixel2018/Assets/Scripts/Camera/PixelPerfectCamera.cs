using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelPerfectCamera : MonoBehaviour 
{
	void OnEnable()
	{
		float unitsPerPixel = 1f / 64f;
		Camera.main.orthographicSize = (1080f / 2f) * unitsPerPixel;	
	}
}
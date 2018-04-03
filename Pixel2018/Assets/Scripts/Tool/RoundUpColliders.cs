using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class RoundUpColliders : MonoBehaviour
{

	#if UNITY_EDITOR
    public void RoundUp()
    {
		EdgeCollider2D edge = GetComponent<EdgeCollider2D> ();
		if (edge != null) {
			Vector2[] newPoints = new Vector2[edge.points.Length];
			for (int i = 0; i < edge.points.Length; i++) {
				Vector2 p = new Vector2 (edge.points [i].x , edge.points[i].y);

				p.x = RoundValue (p.x);
				p.y = RoundValue (p.y);

				newPoints[i] = p;
			}
			edge.points = newPoints;
			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
		}

		PolygonCollider2D poly = GetComponent<PolygonCollider2D> ();
		if (poly != null) {
			Vector2[] newPoints = new Vector2[poly.points.Length];
			for (int i = 0; i < poly.points.Length; i++) {
				Vector2 p = new Vector2 (poly.points [i].x , poly.points[i].y);

				p.x = RoundValue (p.x);
				p.y = RoundValue (p.y);

				newPoints[i] = p;
			}
			poly.points = newPoints;
			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
		}
    }

	public static float RoundValue(float value)
	{
		float rest = Mathf.Abs( value % 1 );
		if (rest > 0.25f && rest < 0.75f) {
			if (value < 0f) {
				value = Mathf.Ceil (value) - 0.5f;
			} else {
				value = Mathf.Floor (value) + 0.5f;
			}
		} else {
			value = Mathf.Round (value);
		}

		return value;
	}
	#endif
}
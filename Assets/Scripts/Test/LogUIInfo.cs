using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUIInfo : MonoBehaviour 
{

//	// Use this for initialization
//	void Start () 
//	{
//		
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		
//	}

	public void DebugUIInfo()
	{
		Debug.Log ("<color=magenta>***************************</color>",gameObject);
		Debug.Log ("UI Element Name: "+gameObject.name);
		Debug.Log ("Go Position: "+gameObject.transform.position);
		RectTransform myRect = gameObject.GetComponent<RectTransform> ();
		Debug.Log ("Rect is Available: "+myRect);
		if (myRect != null) 
		{
			Debug.Log ("Rect Pos: "+myRect.position);
			Debug.Log ("Rect localPos: "+myRect.localPosition);
			Debug.Log ("Rect Anchored Pos: "+myRect.anchoredPosition);
			Debug.Log ("Rect Info: "+myRect.rect);
		}
	}
}

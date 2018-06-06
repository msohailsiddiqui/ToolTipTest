using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOnElement : MonoBehaviour 
{
	public RectTransform targetRect;

	private RectTransform myRect ;
	// Use this for initialization
	void Start () 
	{
		myRect = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame


	void Update () 
	{
		if (Input.GetKeyUp (KeyCode.P)) 
		{
			if (myRect != null && targetRect != null) 
			{
				myRect.SetParent(targetRect,false);
			}
		}
		//This testing code has been shifted to ToolTipTestObj
//		if (Input.GetKeyUp (KeyCode.Alpha1)) 
//		{
//			if (myRect != null && targetRect != null) 
//			{
//				myRect.SetParent(targetRect,false);
//				myRect.pivot = new Vector2(0,0);
//				myRect.anchorMax = new Vector2(0.0f,1);
//				myRect.anchorMin = new Vector2(0.0f,1);
//
//			}
//		}
//
//		if (Input.GetKeyUp (KeyCode.Alpha2)) 
//		{
//			if (myRect != null && targetRect != null) 
//			{
//				myRect.SetParent(targetRect,false);
//				myRect.pivot = new Vector2(0,0);
//				myRect.anchorMax = new Vector2(0.5f,1);
//				myRect.anchorMin = new Vector2(0.5f,1);
//
//			}
//		}
//
//		if (Input.GetKeyUp (KeyCode.Alpha3)) 
//		{
//			if (myRect != null && targetRect != null) 
//			{
//				myRect.SetParent(targetRect,false);
//				myRect.pivot = new Vector2(0,0);
//				myRect.anchorMax = new Vector2(1f,1);
//				myRect.anchorMin = new Vector2(1f,1);
//
//			}
//		}
//
//		if (Input.GetKeyUp (KeyCode.Alpha4)) 
//		{
//			if (myRect != null && targetRect != null) 
//			{
//				myRect.SetParent(targetRect,false);
//				myRect.pivot = new Vector2(0,1);
//				myRect.anchorMax = new Vector2(0.0f,0);
//				myRect.anchorMin = new Vector2(0.0f,0);
//
//			}
//		}
//
//		if (Input.GetKeyUp (KeyCode.Alpha5)) 
//		{
//			if (myRect != null && targetRect != null) 
//			{
//				myRect.SetParent(targetRect,false);
//				myRect.pivot = new Vector2(0,1);
//				myRect.anchorMax = new Vector2(0.5f,0);
//				myRect.anchorMin = new Vector2(0.5f,0);
//
//			}
//		}
//
//		if (Input.GetKeyUp (KeyCode.Alpha6)) 
//		{
//			if (myRect != null && targetRect != null) 
//			{
//				myRect.SetParent(targetRect,false);
//				myRect.pivot = new Vector2(0,1);
//				myRect.anchorMax = new Vector2(1.0f,0);
//				myRect.anchorMin = new Vector2(1.0f,0);
//
//			}
//		}
	}
}

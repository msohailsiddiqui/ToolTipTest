using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicResizer : MonoBehaviour 
{
	private List<RectTransform> childrenRects;
	private float totalContentWidth;
	private float totalContentHeight;
	private RectTransform myRectTransform;
	// Use this for initialization
	void Start () 
	{
		childrenRects = new List<RectTransform> ();

		RectTransform[] allRects = GetComponentsInChildren<RectTransform> ();

		int index = 0;
		foreach (RectTransform rect in allRects) 
		{
			Debug.Log (index + ". " + rect.name);

			//The First transform will be the object itself
			if (index != 0) {
				childrenRects.Add (rect);
			} else {
				myRectTransform = rect;
			}

			index++;
		}

		totalContentWidth = 0;
		totalContentHeight = 0;

		
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKeyUp (KeyCode.A)) 
		{
			int index = 1;
			foreach (RectTransform rect in childrenRects) 
			{
				Debug.Log (index + ". " + rect.name + ", size: " + rect.rect.size);
				totalContentWidth += rect.rect.size.x;
				totalContentHeight += rect.rect.size.y;
				index++;
			}

			Debug.Log ("totalContentWidth: " + totalContentWidth + ", totalContentHeight: " + totalContentHeight);
		}

		if (Input.GetKeyUp (KeyCode.B)) 
		{
			myRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, totalContentHeight); 
		}
		
	}
}

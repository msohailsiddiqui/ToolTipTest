using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper 
{
	#region singleton class
	private static UIHelper instance = null;
	private UIHelper() { }

	public static UIHelper Instance
	{
		get
		{
			if (instance == null)
				instance = new UIHelper ();

			return instance;
		}
	}

	#endregion

	// This function will calculate the size of the tool tip 
	// based on the size of the content
	public static void CalculateContentSize()
	{

	}

	public static Vector2 CalculatePosition (Vector2 posToBeUsed, ToolTipAnchor preferredAnchor)
	{
		return Vector2.zero;
	}

	public static Vector2 CalculateSize ()
	{
		return Vector2.zero;
	}

	//This function will check if a point/position is on the screen
	public static bool IsPointOnScreen(Vector2 positionToBeChecked)
	{

		return true;
	}

	//This function will check if a complete rect is is on the screen
	public static bool IsRectOnScreen()
	{
		return true;
	}

	public static bool IsThereSpaceToShowToolTip()
	{
		return true;
	}

	public static void FindNewPositionForToolTip()
	{

	}

}

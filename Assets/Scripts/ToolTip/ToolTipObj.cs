using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ToolTipAnchor
{
	TopLeft = 0,
	TopCenter,
	TopRight,
	BottomLeft,
	BottomCenter,
	BottomRight
}

//************************************************************************************
// This class will serve as the base class that provides function to actually show the tool tip
// This should be attached to a game object that has the needed elements to display the tool tip
//************************************************************************************

public class ToolTipObj : MonoBehaviour 
{
	

	public Image toolTipBG;
	public Text toolTipSmallText;

	// The position of the object on which this tip will be displayed
	// This will be populated by the object that is requesting the tool tip
	private Vector2 objPos;

	// We get the preferred anchor of the tool tip, from a setting of the tool tip controller
	// but depending on the scenario it might be changed, if there isn't enough space to display it on this anchor
	// For Example, if the preferred anchor is set to bottom right and the button trying to display the tool tip
	// is located at the bottom right of the screen it will reposition it 
	// This variable will contain the final anchor on which the tool tip can be displayed
	//private ToolTipAnchor selectedAnchor;

	// Use this for initialization
	void Start () 
	{
		if (toolTipBG == null) 
		{
			Debug.LogError ("toolTipBG is Null, it should never be null");
		}

		if (toolTipSmallText == null) 
		{
			Debug.LogError ("toolTipSmallText is Null, it should never be null");
		}
	}
	
	// Update is called once per frame
//	void Update () 
//	{
//		
//	}

	public void Initialize()
	{
		gameObject.transform.localScale = new Vector3 (0, 0, 0);
		if (toolTipBG != null) 
		{
			//toolTipBG.GetComponent<>
		}
	}

	public bool SetupToolTip (Vector2 posToBeUsed, ToolTipData data, ToolTipAnchor anchorToBeUsed)
	{
		//Check if the data object is valid
		if (data == null) 
		{
			Debug.LogError ("SimpleToolTipObj::Tool Tip Data Validation failed, data was NULL, This should never be null ");
			return false;
		}
		if (!UIHelper.IsPointOnScreen (posToBeUsed)) 
		{
			Debug.LogError ("ToolTipObj::Obj that wants to show tool tip is not on Screen, object pos: "+ posToBeUsed );
			return false;
		}

		//Check if the bare minimum data is present (small tool tip text)
		if (data.HasElement (ToolTipElementID.SmallToolTipText)) 
		{
			// if we have the right data copy it to our variables
			toolTipSmallText.text = data.GetElement(ToolTipElementID.SmallToolTipText);
			//selectedAnchor = SelectAnchor (anchorToBeUsed, data);
			gameObject.transform.position = new Vector3 (posToBeUsed.x, posToBeUsed.y, 0);
			//CalculatePosition (posToBeUsed, selectedAnchor);
			//CalculateSize ();
			return true;
		} 
		else 
		{
			return false;
		}

	}

	public void ResetToolTip()
	{
		toolTipSmallText.text = "";
	}

	private ToolTipAnchor SelectAnchor (ToolTipAnchor preferredAnchor, ToolTipData data)
	{
		float preferredHeight =  toolTipSmallText.cachedTextGenerator.GetPreferredHeight (
			toolTipSmallText.text, toolTipSmallText.GetGenerationSettings (toolTipSmallText.rectTransform.rect.size));

		float preferredWidth =  toolTipSmallText.cachedTextGenerator.GetPreferredWidth (
			toolTipSmallText.text, toolTipSmallText.GetGenerationSettings (toolTipSmallText.rectTransform.rect.size));

		Debug.Log ("ToolTipObject::SelectAnchor:preferredHeight: "+preferredHeight+",preferredWidth: "+preferredWidth );

		return preferredAnchor;
	}
}

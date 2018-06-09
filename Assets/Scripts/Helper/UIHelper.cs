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

/*
public Image bgImage;
public Text smallText;
public Text detailedText;
public Text linkText;
public Image smallImage;
public Image largeImage;

// Formatting variables for the tool tip
// The space between the border of the tool tip and the content it conatins
public uint borderX;
public uint borderY;
public uint paddingBetweenElementsX;
public uint paddingBetweenElementsY;

private float smallTextYOffset;
private float smallImageXOffset;

Dictionary<ToolTipElementID, ToolTipUIElement> uiElementsDict;


private float totalContentWidth;
private float totalContentHeight;
private RectTransform toolTipRectTransform;

private ToolTipPlacementData currentToolTipPlacement;

public ToolTipPlacementData CurrentToolTipPlacement
{
	get
	{
		return currentToolTipPlacement;
	}

	set
	{
		currentToolTipPlacement = value;
	}
}


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
	if (bgImage == null) 
	{
		Debug.LogError ("bgImage is Null, it should never be null");
	}

	if (smallText == null) 
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
	if (bgImage != null) 
	{
		//bgImage.GetComponent<>
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
		smallText.text = data.GetElement(ToolTipElementID.SmallToolTipText);
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
	smallText.text = "";
}

private ToolTipAnchor SelectAnchor (ToolTipAnchor preferredAnchor, ToolTipData data)
{
	float preferredHeight =  smallText.cachedTextGenerator.GetPreferredHeight (
		smallText.text, smallText.GetGenerationSettings (smallText.rectTransform.rect.size));

	float preferredWidth =  smallText.cachedTextGenerator.GetPreferredWidth (
		smallText.text, smallText.GetGenerationSettings (smallText.rectTransform.rect.size));

	Debug.Log ("ToolTipObject::SelectAnchor:preferredHeight: "+preferredHeight+",preferredWidth: "+preferredWidth );

	return preferredAnchor;
}
*/
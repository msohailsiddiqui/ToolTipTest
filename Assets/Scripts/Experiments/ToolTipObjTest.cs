using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToolTipObjTest : MonoBehaviour 
{

	public Text smallText;
	public Text detailedText;
	public Text linkText;
	public Image smallImage;
	public Image largeImage;
	public RectTransform CanvasRect;

    // Formatting variables for the tool tip
    // The space between the border of the tool tip and the content it conatins
	public uint borderX;
	public uint borderY;
	public uint paddingBetweenElementsX;
	public uint paddingBetweenElementsY;

	private float currentXOffset;
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



    // Use this for initialization
    void Start () 
	{
        CurrentToolTipPlacement = new ToolTipPlacementData();

        uiElementsDict = new Dictionary<ToolTipElementID, ToolTipUIElement> ();

		ToolTipUIElement smallTextUI = new ToolTipUIElement ();

		smallTextUI.elementID = ToolTipElementID.SmallToolTipText;
		smallTextUI.elementType = ToolTipElementType.Text;
		smallTextUI.textElement = smallText;
		smallTextUI.uiElementRectTransform = smallText.rectTransform;
		smallTextUI.textElement.text = "";

		ToolTipUIElement detailedTextUI = new ToolTipUIElement ();

		detailedTextUI.elementID = ToolTipElementID.DetailedToolTipText;
		smallTextUI.elementType = ToolTipElementType.Text;
		detailedTextUI.textElement = detailedText;
		detailedTextUI.uiElementRectTransform = detailedText.rectTransform;
		detailedTextUI.textElement.text = "";

		ToolTipUIElement smallImageUI = new ToolTipUIElement ();

		smallImageUI.elementID = ToolTipElementID.SmallDescriptionImage;
		smallImageUI.elementType = ToolTipElementType.Image;
		smallImageUI.imageElement = smallImage;
		smallImageUI.uiElementRectTransform = smallImage.rectTransform;

		uiElementsDict.Add (ToolTipElementID.SmallToolTipText, smallTextUI);
		uiElementsDict.Add (ToolTipElementID.DetailedToolTipText, detailedTextUI);
		uiElementsDict.Add (ToolTipElementID.SmallDescriptionImage, smallImageUI);

		toolTipRectTransform = GetComponent<RectTransform> ();
	}

    public RectTransform GetRectTransform()
    {
        return toolTipRectTransform;
    }
	
	// Update is called once per frame
	void Update () 
	{
		

	}

	public bool SetupToolTip(ToolTipData data)
	{
		ResetToolTip ();
		if (!data.HasElement (ToolTipElementID.SmallToolTipText)) 
		{
			return false;	

		}
		uiElementsDict [ToolTipElementID.SmallToolTipText].textElement.text = data.GetElement(ToolTipElementID.SmallToolTipText);
		uiElementsDict [ToolTipElementID.SmallToolTipText].UpdateElement ();

		if (data.HasElement (ToolTipElementID.SmallDescriptionImage)) 
		{
			//Debug.Log ("ToolTipObjTest::SetupToolTip:Data has small image: "+data.GetElement (ToolTipElementID.SmallDescriptionImage));

			Sprite tempSprite = Resources.Load (data.GetElement (ToolTipElementID.SmallDescriptionImage), typeof(Sprite)) as Sprite;
			//Debug.Log ("ToolTipObjTest::SetupToolTip:small image: "+tempSprite);
			//Debug.Log ("ToolTipObjTest::SetupToolTip:small image size: "+tempSprite.rect.size );
			uiElementsDict [ToolTipElementID.SmallDescriptionImage].imageElement.sprite = tempSprite;
			uiElementsDict [ToolTipElementID.SmallDescriptionImage].UpdateElement ();
		}
		if (data.HasElement (ToolTipElementID.DetailedToolTipText)) 
		{
			uiElementsDict [ToolTipElementID.DetailedToolTipText].textElement.text = data.GetElement(ToolTipElementID.DetailedToolTipText);
			uiElementsDict [ToolTipElementID.DetailedToolTipText].UpdateElement ();
		}

		UpdateToolTip ();
		return true;
	}

	//Clears previous data so thatwe don't get elements from previous tool tips
	public void ResetToolTip()
	{
		if (uiElementsDict.ContainsKey (ToolTipElementID.SmallToolTipText)) 
		{
			uiElementsDict [ToolTipElementID.SmallToolTipText].ResetElement ();
		}

		if (uiElementsDict.ContainsKey (ToolTipElementID.DetailedToolTipText)) 
		{
			uiElementsDict [ToolTipElementID.DetailedToolTipText].ResetElement ();
		}

		if (uiElementsDict.ContainsKey (ToolTipElementID.SmallDescriptionImage)) 
		{
			uiElementsDict [ToolTipElementID.SmallDescriptionImage].ResetElement ();
		}

		if (uiElementsDict.ContainsKey (ToolTipElementID.LargeDescriptionImage)) 
		{
			uiElementsDict [ToolTipElementID.LargeDescriptionImage].ResetElement ();
		}
		if (uiElementsDict.ContainsKey (ToolTipElementID.HelpLinkURL)) 
		{
			uiElementsDict [ToolTipElementID.HelpLinkURL].ResetElement ();
		}

		UpdateToolTip ();
	}

	private void CalculateToolTipElementSizes()
	{

        // TODO: Ideally we should not be dealing with the public Text and Image objects at all
        // We should be working with the UI elements since they have been populated
        // Need a better way to do this

        // If we have reached here we must have atleast a valid small text to show

        // Reset Variables
        // Before we start placing elements we need to add the specified borders
        // at the top and to the left
        totalContentWidth = borderX;
		totalContentHeight = borderY;
		smallTextYOffset = 0;
		smallImageXOffset = borderX;
		// Before we start placing elements we need to add the specified borders
		if (smallText != null && smallText.text.Length > 0) 
		{
			smallText.fontStyle = FontStyle.Normal;
			totalContentWidth += smallText.rectTransform.rect.size.x;
			totalContentHeight += smallText.rectTransform.rect.size.y;
		}

		// TODO: This is a very ugly way to do this, need to clean this up
		// if we have something more to show
		if ((smallImage != null && smallImage.sprite != null && smallImage.sprite.rect.size.x > 0) ||
		    (detailedText != null && detailedText.text.Length > 0)) 
		{
			// AddingVerticalPadding, once here since any and all additional elements will come under the small text
			totalContentHeight += paddingBetweenElementsY;
			//Change the appearance of the small text to make it a heading
			smallText.fontStyle = FontStyle.Bold;
		}

        // Need to save this for placing other contents under the tool tip (if there are any)
        smallTextYOffset = totalContentHeight;

        // make sure we have something valid to show
        if (smallImage != null && smallImage.sprite != null && smallImage.sprite.rect.size.x > 0) 
		{
			// Since the small text will already be there
			// First add some padding to it
			//smallTextYOffset += paddingBetweenElementsY;

			// Do we need to update the total width of the content
			totalContentWidth = smallImage.rectTransform.rect.size.x  > totalContentWidth ? 
				detailedText.rectTransform.rect.size.x  : totalContentWidth;

			totalContentHeight += smallImage.rectTransform.rect.size.y;
			smallImageXOffset += smallImage.rectTransform.rect.size.x + paddingBetweenElementsX;
		}

        // only the small text is compulsory the detailed text is optional
        // It may appear with an image or without an image
        // If it appears with an image it is placed on the right of the image
        // Hence we need to calculate this after the image
        if (detailedText != null && detailedText.text.Length > 0) 
		{
            // if the width of the current content is larger, we keep that otherwise it should be set to the 
            // width of the detailed text
            // Width of the detailed text will generally be greater than the totalContentWidth (which at this point 
            // would contain the small text width +border) unless we constrain it
            totalContentWidth = detailedText.rectTransform.rect.size.x + smallImageXOffset > totalContentWidth ? 
				detailedText.rectTransform.rect.size.x + smallImageXOffset : totalContentWidth;
			// Since the detailed text is placed under the small text, we update the height accordingly
			totalContentHeight =  detailedText.rectTransform.rect.size.y + smallTextYOffset > totalContentHeight ? 
				detailedText.rectTransform.rect.size.y + smallTextYOffset  : totalContentHeight; 
			//			detailedText.rectTransform.pivot = new Vector2(1,0);
			//			detailedText.rectTransform.anchorMax = new Vector2(1.0f,0.5f);
			//			detailedText.rectTransform.anchorMin = new Vector2(1.0f,0.5f);
		}

        // Now that all the content has been added to the size
        // Add the borders at the right and bottom
        totalContentWidth += borderX;
		totalContentHeight += borderY;

		//Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Final totalContentHeight: " +totalContentHeight+ "</color>");
		//Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Final totalContentWidth: " +totalContentWidth+ "</color>");
		//Debug.Log ("<color=orange>smallTextYOffset: "+smallTextYOffset+ "</color>");
		//Debug.Log ("<color=orange>smallTextYOffset: "+smallImageXOffset+ "</color>");
	}

	private void PositionToolTipElements()
	{
        //We start placing elements from top left towards bottom right
        // 1 -->
        // 2 --> 3 -->

        // Place the small text at the top left but offset by the border
        smallText.rectTransform.pivot = new Vector2(0,1);
        // We need to handle the case where the border might be zero
        // otherwise the result might be a NaN
        float applicableXOffset = borderX < 1 ? 0 : (borderX / totalContentWidth);
        float applicableYOffset = borderY < 1 ? 0 : (borderY / totalContentHeight);

        smallText.rectTransform.anchorMax = new Vector2(0.0f + applicableXOffset, 1- applicableYOffset);
		smallText.rectTransform.anchorMin = new Vector2(0.0f + applicableXOffset, 1- applicableYOffset);
		
		if (smallImage != null && smallImage.sprite!=null && smallImage.sprite.rect.size.x > 0) 
		{
			smallImage.rectTransform.pivot = new Vector2(0,1f);
			smallImage.rectTransform.anchorMax = new Vector2 (0.0f  + applicableXOffset, 1 - (smallTextYOffset / totalContentHeight));
			smallImage.rectTransform.anchorMin = new Vector2 (0.0f  + applicableXOffset, 1 - (smallTextYOffset / totalContentHeight));
		}

		if (detailedText != null && detailedText.text.Length > 0) 
		{
			// Place the detailed text under the small text, its anchor will also be top left but
			// Its anchor wil be offset by the height of the small text
			//Debug.Log("<color=yellow>Trying to position detailed text, smallImageXOffset: "+smallImageXOffset+"</color>");
			detailedText.rectTransform.pivot = new Vector2 (0, 1);
			detailedText.rectTransform.anchorMax = new Vector2 (0.0f + (smallImageXOffset / totalContentWidth), 1 - (smallTextYOffset / totalContentHeight));
			detailedText.rectTransform.anchorMin = new Vector2 (0.0f + (smallImageXOffset / totalContentWidth), 1 - (smallTextYOffset / totalContentHeight));
		}

		toolTipRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, totalContentWidth); 
		toolTipRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, totalContentHeight); 

	}

	private void UpdateToolTip()
	{
		//General Format of this tool tip format is as follows:
		// ------------------------------------------------
		// | small Text - That shows initial description  |
		// |----------------------------------------------|
		// |                PaddingY                      |
		// |----------------------------------------------|
		// |
		// |  Optional Small Image | PX |                  ----------------|
		// |                       |    |                                  |
		// |                       |    |  Detailed Text Area              |
		// |---------------------------------------------------------------|

		// First to calculate the size of each available element
		// It is necessary to do this before positioning the elements
		// since elements are positioned as percenatges of the total content size of the tool tip
		// If we calculate size and position the elements indiviually, it results in incorrect results
		// For example in the start the small text places itself at 0.1f width according to the specified border
		// and current content size (assuming 100, so it is placed at 10) but as more elements are added 
		// later on the total size is changed but the small text is still at 0.1 width (assuming increased width is 200, 
		// so now it is placed at 20) which will be incorrect
		// 
		CalculateToolTipElementSizes ();
		PositionToolTipElements ();
	}

	public void UpdateToolTipAnchor(ToolTipPlacementData requiredPlacement)
	{
        CurrentToolTipPlacement.VerticalPlacement = requiredPlacement.VerticalPlacement;
        CurrentToolTipPlacement.HorizontalPlacement = requiredPlacement.HorizontalPlacement;
        CurrentToolTipPlacement.HorizontalDirection = requiredPlacement.HorizontalDirection;
        CurrentToolTipPlacement.RequiredAnchorMax = requiredPlacement.RequiredAnchorMax;
        CurrentToolTipPlacement.RequiredAnchorMin = requiredPlacement.RequiredAnchorMin;
        CurrentToolTipPlacement.RequiredPivot  = requiredPlacement.RequiredPivot;

        toolTipRectTransform.pivot = requiredPlacement.RequiredPivot;
		toolTipRectTransform.anchorMax = requiredPlacement.RequiredAnchorMax;
		toolTipRectTransform.anchorMin = requiredPlacement.RequiredAnchorMin;
		CheckAndFitWithinScreenBounds ();
	}

	private void CheckAndFitWithinScreenBounds()
	{
        int allowedRecursionLevel = 2;
        CheckIfWithinBounds(allowedRecursionLevel);
    }

    private bool CheckIfWithinBounds(int _allowedRecursionLevel)
    {
        Debug.Log("CheckIfWithinBounds::_allowedRecursionLevel: " + _allowedRecursionLevel);
        if (_allowedRecursionLevel > 0)
        {
            _allowedRecursionLevel--;
            Vector3[] v = new Vector3[4];
            toolTipRectTransform.GetWorldCorners(v);

            float maxX = Mathf.Max(v[0].x, v[1].x, v[2].x, v[3].x);
            float minX = Mathf.Min(v[0].x, v[1].x, v[2].x, v[3].x);

            float maxY = Mathf.Max(v[0].y, v[1].y, v[2].y, v[3].y);
            float minY = Mathf.Min(v[0].y, v[1].y, v[2].y, v[3].y);

            // If tool tip is going out of screen bounds, fix it
            if (minY < 0 || maxY > Screen.height || minX < 0 || maxX > Screen.width)
            {
                // Tooltip is outside of the screen
                Debug.Log("<color=red>Tooltip is outside of the screen:maxY: " + maxY + ", minY: " + minY + ", ReFtting </color>");
                FitWithinScreenBounds(minX, maxX, minY, maxY);
                return CheckIfWithinBounds(_allowedRecursionLevel);
            }
            else
            {
                Debug.Log("<color=green>Tooltip is inside of the screen:maxY: " + maxY + ", minY: " + minY + " </color>");
                return true;
            }
        }
        return true;
    }


    private void FitWithinScreenBounds(float _minX, float _maxX, float _minY, float _maxY)
    {
        Debug.Log("<color=orange>Tooltip specified placement is: "
            +CurrentToolTipPlacement.VerticalPlacement+CurrentToolTipPlacement.HorizontalPlacement+CurrentToolTipPlacement.HorizontalDirection+" </color>");
        // In most cases these two scenarios will be mutually exclusive unless the tool tip content is larger than the screen width
        // not sure what we can do in this scenario.
        if (_minX < 0 && _maxX > Screen.width)
        {
            Debug.LogError("ToolTipObj::FitWithinScreenBounds: Seems like the tool tip content is greater than screen width, minX: " + _minX + ", maxX: " + _maxX);
        }
        // means that the tool tip is going out of screen from the left
        // This shouldn't happen because we first check if the element that we are placing 
        // the tool tip on is on the screen and current implementation does not support the RightToLeft Direction
        // When Anchor is at the top or bottom left
        // This would only happen if the element is at the left side of the screen and horizontal direction is RightToLeft
        // In this case we can just switch the direction to LeftToRight
        else if (_minX < 0)
        {
            Debug.Log("<color=yellow>minX<0</color>");
            CurrentToolTipPlacement.UpdateToolTipHorizontalDirection(UIElementHorizontalDirection.LeftToRight);
        }
        // means that the tool tip is going out of screen from the right
        // This would happen if the element is at the right side of the screen and horizontal direction is LeftToRight
        // In this case we can just switch the direction to RightToLeft
        else if (_maxX > Screen.width)
        {
            Debug.Log("<color=yellow>maxX<Screen.Width</color>");
            CurrentToolTipPlacement.UpdateToolTipHorizontalDirection(UIElementHorizontalDirection.RightToLeft);
        }

        // In most cases these two scenarios will be mutually exclusive unless the tool tip content is larger than the screen height
        // not sure what we can do in this scenario.
        if (_minY < 0 && _maxY > Screen.width)
        {
            Debug.LogError("ToolTipObj::FitWithinScreenBounds: Seems like the tool tip content is greater than screen Height, minY: " + _minY + ", maxY: " + _maxY);
        }
        // means that the tool tip is going out of screen from the bottom
        // This would only happen if the element is at the bottom of the screen and the tool tip is also placed at the bottom of the element
        // In this case we have to put the tool tip at the top
        else if (_minY < 0)
        {
            Debug.Log("<color=yellow>minY<0</color>");
            CurrentToolTipPlacement.UpdateToolTipVerticalPlacement(UIElementVerticalPlacement.Top);
        }
        // means that the tool tip is going out of screen from the top
        // This would happen if the element is at the top of the screen and the tool tip is also placed at the top of the element
        // In this case we have to put the tool tip at the bottom
        else if (_maxY > Screen.height)
        {
            Debug.Log("<color=yellow>maxY>Screen.Height</color>");
            CurrentToolTipPlacement.UpdateToolTipVerticalPlacement(UIElementVerticalPlacement.Bottom);
        }

        Debug.Log("<color=yellow>Tooltip refitted placement is: "
            + CurrentToolTipPlacement.VerticalPlacement + CurrentToolTipPlacement.HorizontalPlacement + CurrentToolTipPlacement.HorizontalDirection + " </color>");

        toolTipRectTransform.pivot = CurrentToolTipPlacement.RequiredPivot;
        toolTipRectTransform.anchorMax = CurrentToolTipPlacement.RequiredAnchorMax;
        toolTipRectTransform.anchorMin = CurrentToolTipPlacement.RequiredAnchorMin;

    }


}

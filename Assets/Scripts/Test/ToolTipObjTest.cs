using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipUIElement
{
	public RectTransform uiElementRectTransform;
	public ToolTipElementID elementID;
	public ToolTipElementType elementType;
	public Text textElement;
	public Image imageElement;


	public void UpdateElement()
	{
		float preferredWidth,preferredHeight;
		switch (elementType) 
		{
		case ToolTipElementType.Text:
			preferredWidth = textElement.cachedTextGenerator.GetPreferredWidth (textElement.text, textElement.GetGenerationSettings(textElement.rectTransform.rect.size) );
			textElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, preferredWidth);
			preferredHeight = textElement.cachedTextGenerator.GetPreferredHeight (textElement.text, textElement.GetGenerationSettings (textElement.rectTransform.rect.size));
			textElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, preferredHeight);
			break;
		case ToolTipElementType.Image:
			Debug.Log ("ToolTipElement::UpdateElement:Sprite size: "+imageElement.sprite.rect.size);
			//preferredWidth = textElement.cachedTextGenerator.GetPreferredWidth (textElement.text, textElement.GetGenerationSettings(textElement.rectTransform.rect.size) );
			imageElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, imageElement.sprite.rect.size.x);
			//preferredHeight = textElement.cachedTextGenerator.GetPreferredHeight (textElement.text, textElement.GetGenerationSettings (textElement.rectTransform.rect.size));
			imageElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, imageElement.sprite.rect.size.y);
			Debug.Log ("ToolTipElement::UpdateElement: Image element rect size: "+imageElement.rectTransform.rect.size);
			imageElement.color = new Color (1, 1, 1, 1);
			break;
		default:
			break;
		}
	}

	public void ResetElement()
	{
		switch (elementType) 
		{
		case ToolTipElementType.Text:
			textElement.text = "";
			textElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 0);
			textElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 0);
			break;
		case ToolTipElementType.Image:
			imageElement.sprite = null;
			imageElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 0);
			imageElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 0);
			imageElement.color = new Color (1, 1, 1, 0);
			break;
		default:
			break;
		}
	}
}


public class ToolTipObjTest : MonoBehaviour 
{

	public Text smallText;
	public Text detailedText;
	public Text linkText;
	public Image smallImage;
	public Image largeImage;

	public int borderX;
	public int borderY;
	public int paddingBetweenElementsX;
	public int paddingBetweenElementsY;

	private float currentXOffset;
	private float smallTextYOffset;
	private float smallImageXOffset;

	ToolTipData smallTextData;
	ToolTipData detailedTextData;
	ToolTipData smallImageData;
	ToolTipData smallTextWithImageData;
	Dictionary<ToolTipElementID, ToolTipUIElement> uiElementsDict;

	private float totalContentWidth;
	private float totalContentHeight;
	private RectTransform toolTipRectTransform;



	// Use this for initialization
	void Start () 
	{
		// Create a new Tool Tip Data
		// In the actual code this will be passed
		smallTextData = new ToolTipData ("This yqgpj is a small tooltip only");
		detailedTextData = new ToolTipData ("This yqgpj is the small tooltip with detailed text", 
			"This is the large multiline Tool tip Text\n The quick brown fox jumped over the lazy dog");

		smallImageData = new ToolTipData ("This yqgpj is the small tooltip text", 
			"This is the large multiline Tool tip Text\n The quick brown fox jumped over the lazy dog",
		"smallImageTestRect64");

		smallTextWithImageData = new ToolTipData ("This yqgpj is a small tooltip with image only");
		smallTextWithImageData.AddElement (ToolTipElementID.SmallDescriptionImage, "smallImageTestRect");

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
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyUp (KeyCode.S)) 
		{
			//Populate the Tool Tip Obj
			SetupToolTip(smallTextData);
		}	

		if (Input.GetKeyUp (KeyCode.D)) 
		{
			//Populate the Tool Tip Obj

			SetupToolTip(smallTextWithImageData);
		}	

		if (Input.GetKeyUp (KeyCode.F)) 
		{
			//Populate the Tool Tip Obj
			SetupToolTip(detailedTextData);
		}	

		if (Input.GetKeyUp (KeyCode.G)) 
		{
			SetupToolTip (smallImageData);
		}

		if (Input.GetKeyUp (KeyCode.R)) 
		{
			ResetToolTip ();
		}
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
			Debug.Log ("ToolTipObjTest::SetupToolTip:Data has small image: "+data.GetElement (ToolTipElementID.SmallDescriptionImage));

			Sprite tempSprite = Resources.Load (data.GetElement (ToolTipElementID.SmallDescriptionImage), typeof(Sprite)) as Sprite;
			Debug.Log ("ToolTipObjTest::SetupToolTip:small image: "+tempSprite);
			Debug.Log ("ToolTipObjTest::SetupToolTip:small image size: "+tempSprite.rect.size );
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
	private void ResetToolTip()
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
		totalContentWidth = 0;
		totalContentHeight = 0;
		smallTextYOffset = 0;
		smallImageXOffset = 0;
		// Before we start placing elements we need to add the specified borders
		if (smallText != null && smallText.text.Length > 0) 
		{
			totalContentWidth += smallText.rectTransform.rect.size.x;
			totalContentHeight += smallText.rectTransform.rect.size.y;
		}
		smallTextYOffset = totalContentHeight;

		if (smallImage != null && smallImage.sprite != null && smallImage.sprite.rect.size.x > 0) 
		{
			// Since the small text will already be there
			// First add some padding to it
			//smallTextYOffset += paddingBetweenElementsY;

			// Do we need to update the total width of the content
			totalContentWidth = smallImage.rectTransform.rect.size.x  > totalContentWidth ? 
				detailedText.rectTransform.rect.size.x  : totalContentWidth;

			totalContentHeight += smallImage.rectTransform.rect.size.y;
			//smallImageXOffset += smallImage.rectTransform.rect.size.x + paddingBetweenElementsX;
			smallImageXOffset += smallImage.rectTransform.rect.size.x;
		}

		if (detailedText != null && detailedText.text.Length > 0) 
		{
			totalContentWidth = detailedText.rectTransform.rect.size.x + smallImageXOffset > totalContentWidth ? 
				detailedText.rectTransform.rect.size.x + smallImageXOffset + borderX : totalContentWidth;
			// Since the detailed text is placed under the small text, we update the height accordingly
			totalContentHeight =  detailedText.rectTransform.rect.size.y + smallTextYOffset > totalContentHeight ? 
				detailedText.rectTransform.rect.size.y + smallTextYOffset  : totalContentHeight; 
			//			detailedText.rectTransform.pivot = new Vector2(1,0);
			//			detailedText.rectTransform.anchorMax = new Vector2(1.0f,0.5f);
			//			detailedText.rectTransform.anchorMin = new Vector2(1.0f,0.5f);
		}

		Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Final totalContentHeight: " +totalContentHeight+ "</color>");
		Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Final totalContentWidth: " +totalContentWidth+ "</color>");
		Debug.Log ("<color=orange>smallTextYOffset: "+smallTextYOffset+ "</color>");
		Debug.Log ("<color=orange>smallTextYOffset: "+smallImageXOffset+ "</color>");
	}

	private void PositionToolTipElements()
	{
		// Place the small text at the top left but offset by the border
		smallText.rectTransform.pivot = new Vector2(0,1);
		//smallText.rectTransform.anchorMax = new Vector2(0.0f + (borderX/totalContentWidth),1-(borderY/totalContentHeight));
		//smallText.rectTransform.anchorMin = new Vector2(0.0f + (borderX/totalContentWidth),1-(borderY/totalContentHeight));
		smallText.rectTransform.anchorMax = new Vector2(0,1);
		smallText.rectTransform.anchorMin = new Vector2(0,1);

		if (smallImage != null && smallImage.sprite!=null && smallImage.sprite.rect.size.x > 0) 
		{
			smallImage.rectTransform.pivot = new Vector2(0,1f);
			smallImage.rectTransform.anchorMax = new Vector2 (0.0f , 1 - (smallTextYOffset / totalContentHeight));
			smallImage.rectTransform.anchorMin = new Vector2 (0.0f , 1 - (smallTextYOffset / totalContentHeight));
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

	private void CalculateToolTipElementSizes_Orig()
	{
		//If we have reached here we must have atleast a valid small text to show

		// Reset Variables
		currentXOffset = 0;
		smallTextYOffset = 0;
		smallImageXOffset = 0;

		// Before we start placing elements we need to add the specified borders
		// at the top and to the left
		totalContentWidth = borderX;
		totalContentHeight = borderY;

		Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Added Top Border:totalContentHeight: " +totalContentHeight+ "</color>");

		if (smallText != null && smallText.text.Length > 0) 
		{
			totalContentWidth += smallText.rectTransform.rect.size.x;
			totalContentHeight += smallText.rectTransform.rect.size.y;
		}
		Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Added small text :totalContentHeight: " +totalContentHeight+ "</color>");
		// Need to save this for placing other contents under the tool tip (if there are any)
		smallTextYOffset = totalContentHeight + paddingBetweenElementsY;

		// make sure we have something valid to show
		if (smallImage != null && smallImage.sprite != null && smallImage.sprite.rect.size.x > 0) 
		{
			// Since the small text will already be there
			// First add some padding to it
			//smallTextYOffset += paddingBetweenElementsY;

			// Do we need to update the total width of the content
			totalContentWidth = smallImage.rectTransform.rect.size.x + borderX*2 > totalContentWidth ? 
				detailedText.rectTransform.rect.size.x + borderX*2 : totalContentWidth;
			
			totalContentHeight += smallImage.rectTransform.rect.size.y;
			smallImageXOffset += smallImage.rectTransform.rect.size.x + paddingBetweenElementsX;
		}

		Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Added small Image:totalContentHeight: " +totalContentHeight+", smallTextYOffset: " +smallTextYOffset+ "</color>");

		// only the small text is compulsory the detailed text is optional
		// It may appear with an image or without an image
		// If it appears with an image it is placed on the right of the image
		// Hence we need to calculate this after the image
		if (detailedText != null && detailedText.text.Length > 0) 
		{
			// Since the small text will already be there
			// First add some padding to it
			//smallTextYOffset = totalContentHeight + paddingBetweenElementsY;

			// if the width of the current content is larger, we keep that otherwise it should be set to the 
			// width of the detailed text
			// Width of the detailed text will generally be greater than the totalContentWidth (which at this point 
			// would contain the small text width +border) unless we constrain it
			totalContentWidth = detailedText.rectTransform.rect.size.x + smallImageXOffset + borderX > totalContentWidth ? 
				detailedText.rectTransform.rect.size.x + smallImageXOffset + borderX : totalContentWidth;
			// Since the detailed text is placed under the small text, we update the height accordingly
			totalContentHeight =  detailedText.rectTransform.rect.size.y + smallTextYOffset > totalContentHeight ? 
				detailedText.rectTransform.rect.size.y + smallTextYOffset  : totalContentHeight; 
			//			detailedText.rectTransform.pivot = new Vector2(1,0);
			//			detailedText.rectTransform.anchorMax = new Vector2(1.0f,0.5f);
			//			detailedText.rectTransform.anchorMin = new Vector2(1.0f,0.5f);
		}

		Debug.Log ("<color=yellow>CalculateToolTipElementSizes: Added detailed text:totalContentHeight: " +totalContentHeight+", smallTextYOffset: " +smallTextYOffset+ "</color>");

		// Now that all the content has been added to the size
		// Add the borders at the right and bottom
		totalContentWidth += borderX;
		totalContentHeight += borderY;

		Debug.Log ("<color=yellow>CalculateToolTipElementSizes: final totalContentHeight: " +totalContentHeight+"</color>");
	}

	private void PositionToolTipElements_Orig()
	{
		//We start placing elements from top left towards bottom right
		// 1 -->
		// 2 --> 3 -->

		// Place the small text at the top left but offset by the border
		smallText.rectTransform.pivot = new Vector2(0,1);
		smallText.rectTransform.anchorMax = new Vector2(0.0f + (borderX/totalContentWidth),1-(borderY/totalContentHeight));
		smallText.rectTransform.anchorMin = new Vector2(0.0f + (borderX/totalContentWidth),1-(borderY/totalContentHeight));

		if (smallImage != null && smallImage.sprite!=null && smallImage.sprite.rect.size.x > 0) 
		{
			smallImage.rectTransform.pivot = new Vector2(0,1f);
			smallImage.rectTransform.anchorMax = new Vector2 (0.0f + (borderX / totalContentWidth), 1 - (smallTextYOffset / (totalContentHeight+paddingBetweenElementsY)));
			smallImage.rectTransform.anchorMin = new Vector2 (0.0f + (borderX / totalContentWidth), 1 - (smallTextYOffset / (totalContentHeight+paddingBetweenElementsY)));
		}

		if (detailedText != null && detailedText.text.Length > 0) 
		{
			// Place the detailed text under the small text, its anchor will also be top left but
			// Its anchor wil be offset by the height of the small text
			//Debug.Log("<color=yellow>Trying to position detailed text, smallImageXOffset: "+smallImageXOffset+"</color>");
			detailedText.rectTransform.pivot = new Vector2 (0, 1);
			detailedText.rectTransform.anchorMax = new Vector2 (0.0f + ((borderX + smallImageXOffset) / totalContentWidth), 1 - (smallTextYOffset / (totalContentHeight+paddingBetweenElementsY)));
			detailedText.rectTransform.anchorMin = new Vector2 (0.0f + ((borderX + smallImageXOffset) / totalContentWidth), 1 - (smallTextYOffset / (totalContentHeight+paddingBetweenElementsY)));
		}

		float temp = 1 - (smallTextYOffset / totalContentHeight);
		Debug.Log ("<color=yellow>PositionToolTipElement: totalContentHeight: " +totalContentHeight+"</color>");
		Debug.Log ("<color=yellow>PositionToolTipElement: Y percentage: " + (smallTextYOffset / totalContentHeight) + "</color>");
		Debug.Log ("<color=yellow>PositionToolTipElement: modified Y percentage: " + (smallTextYOffset / (totalContentHeight-borderY)) + "</color>");
		Debug.Log ("<color=yellow>PositionToolTipElement: Y Anchor: " +temp+"</color>");



		//		public Text linkText;
		//		public Image smallImage;
		//		public Image largeImage;

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


}

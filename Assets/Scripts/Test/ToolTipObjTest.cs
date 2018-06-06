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
	private float currentYOffset;

	ToolTipData smallTextData;
	ToolTipData detailedTextData;
	ToolTipData smallImageData;
	Dictionary<ToolTipElementID, ToolTipUIElement> uiElementsDict;

	private float totalContentWidth;
	private float totalContentHeight;
	private RectTransform toolTipRectTransform;



	// Use this for initialization
	void Start () 
	{
		// Create a new Tool Tip Data
		// In the actual code this will be passed
		smallTextData = new ToolTipData ("This is a test tooltip");
		detailedTextData = new ToolTipData ("This is the small tooltip text", 
			"This is the large multiline Tool tip Text\n The large brown fox jumped over the lazy dog");

		smallImageData = new ToolTipData ("This is the small tooltip text", 
			"This is the large multiline Tool tip Text\n The large brown fox jumped over the lazy dog",
		"smallImageTest");

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
			SetupToolTip(detailedTextData);
		}	

		if (Input.GetKeyUp (KeyCode.F)) 
		{
			//Populate the Tool Tip Obj
			SetupToolTip(smallImageData);
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

	private void UpdateToolTip()
	{
		currentXOffset = 0;
		currentYOffset = 0;

		totalContentWidth = borderX*2;
		totalContentHeight = borderY*2;

		if (smallText != null && smallText.text.Length > 0) 
		{
			totalContentWidth += smallText.rectTransform.rect.size.x;
			totalContentHeight += smallText.rectTransform.rect.size.y;
		}

		smallText.rectTransform.pivot = new Vector2(0,1);
		smallText.rectTransform.anchorMax = new Vector2(0.0f + (borderX/totalContentWidth),1-(borderY/totalContentHeight));
		smallText.rectTransform.anchorMin = new Vector2(0.0f + (borderX/totalContentWidth),1-(borderY/totalContentHeight));

		currentYOffset = totalContentHeight;


		if (detailedText != null && detailedText.text.Length > 0) 
		{
			currentYOffset = totalContentHeight + paddingBetweenElementsY;

			totalContentWidth = detailedText.rectTransform.rect.size.x;
			totalContentHeight += detailedText.rectTransform.rect.size.y;

//			detailedText.rectTransform.pivot = new Vector2(1,0);
//			detailedText.rectTransform.anchorMax = new Vector2(1.0f,0.5f);
//			detailedText.rectTransform.anchorMin = new Vector2(1.0f,0.5f);

			detailedText.rectTransform.pivot = new Vector2(0,1);
			detailedText.rectTransform.anchorMax = new Vector2(0.0f + (borderX/totalContentWidth),1-(currentYOffset/totalContentHeight));
			detailedText.rectTransform.anchorMin = new Vector2(0.0f + (borderX/totalContentWidth),1-(currentYOffset/totalContentHeight));
		}

		if (smallImage != null && smallImage.sprite!=null && smallImage.sprite.rect.size.x > 0) 
		{
			totalContentWidth += smallImage.rectTransform.rect.size.x;
			totalContentHeight += smallImage.rectTransform.rect.size.y;

			smallImage.rectTransform.pivot = new Vector2(0,0.5f);
			smallImage.rectTransform.anchorMax = new Vector2(0.0f,0.5f);
			smallImage.rectTransform.anchorMin = new Vector2(0.0f,0.5f);
		}
//		public Text linkText;
//		public Image smallImage;
//		public Image largeImage;

		toolTipRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, totalContentWidth); 
		toolTipRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, totalContentHeight); 

	}


}

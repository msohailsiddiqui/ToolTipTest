using UnityEngine;
using UnityEngine.UI;


public enum ToolTipElementType
{
    Text = 0,
    Image,
    Link
}

public enum ToolTipElementID
{
    SmallToolTipText = 0,
    DetailedToolTipText,
    SmallDescriptionImage,
    LargeDescriptionImage,
    HelpLinkURL
}

//************************************************************************************
// This class represents one single UI element in a tool tip. A tool tip can be composed of multiple UI elements
// For example a tool tip might just have a text to show OR
// it can have multiple text fields or images
// Currently this class supports Text and images (links are also a subtype of text)
// It can update the sizes/rects based on updated data - UpdateElement Function
// and it can reset the size/rect to 0 so that the element is no longer visible
// This calss also assumes that it is connected to a Unity UI object
//************************************************************************************

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
			//Debug.Log ("ToolTipElement::UpdateElement:Sprite size: "+imageElement.sprite.rect.size);
			//preferredWidth = textElement.cachedTextGenerator.GetPreferredWidth (textElement.text, textElement.GetGenerationSettings(textElement.rectTransform.rect.size) );
			imageElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, imageElement.sprite.rect.size.x);
			//preferredHeight = textElement.cachedTextGenerator.GetPreferredHeight (textElement.text, textElement.GetGenerationSettings (textElement.rectTransform.rect.size));
			imageElement.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, imageElement.sprite.rect.size.y);
			//Debug.Log ("ToolTipElement::UpdateElement: Image element rect size: "+imageElement.rectTransform.rect.size);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: This should not be a mono behavior
public class BtnWithData : MonoBehaviour 
{
	public string btnID;

	public string logToShow;

	//TODO: This sould not be public instead get it from a reference manager
	public ToolTipController toolTipController;


	// HACK: Ideally the Tool Tip data object should be editable in the editor but since it is using dictionaries
	// It is currently not possible to do that. Will add support for that later. This is a short cut.

	public string smallToolTipText;
    [TextArea(3, 10)]
    public string detailedToolTipText;
    public string smallToolTipDescriptionImage;


    // The generic tool tip data object which holds the data. This will be passed to the Tool tip object
    // Since this based on dictionaries it is not useful to keep this public
    private ToolTipData toolTipData;

	// Use this for initialization
	void Start () 
	{
        toolTipData = new ToolTipData();
		if (smallToolTipText != null && smallToolTipText.Length > 0) 
		{
            // populate the tool tip data object manually
            // TODO: This should be editable in the inspector
            toolTipData.AddElement(ToolTipElementID.SmallToolTipText, smallToolTipText);
            if(detailedToolTipText != null && detailedToolTipText.Length > 0)
            {
                toolTipData.AddElement(ToolTipElementID.DetailedToolTipText, detailedToolTipText);
            }
            if (smallToolTipDescriptionImage != null && smallToolTipDescriptionImage.Length > 0)
            {
                toolTipData.AddElement(ToolTipElementID.SmallDescriptionImage, smallToolTipDescriptionImage);
            }
        }

		if (toolTipController == null) 
		{
			Debug.LogError ("BtnWithData::toolTipController was null, it should never be NULL");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnClick()
	{
		Debug.Log (logToShow);
	}

	public void OnReceiveFocus()
	{
		Debug.Log ("<color=green>" + btnID + ": received focus</color>");
        // TODO: Add check if we have some data that needs to be shown
		if (toolTipController != null && toolTipData.HasElement(ToolTipElementID.SmallToolTipText)) 
		{
			//toolTipController.ToolTipNeedsToBeShown (gameObject.transform.position, toolTipData);
			toolTipController.ToolTipNeedsToBeShown ( toolTipData);
		}
	}

	public void OnLostFocus()
	{
		Debug.Log ("<color=yellow>" + btnID + ": lost focus</color>");
		if (toolTipController != null && toolTipData.HasElement(ToolTipElementID.SmallToolTipText)) 
		{
			toolTipController.RemoveToolTip ();
		}
	}
}

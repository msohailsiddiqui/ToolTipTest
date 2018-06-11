using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//************************************************************************************
// This class will serve as the base class for holding data for showing a tool tip.
// At the minium a SmallToolTipText needs to be present
// Will be adding more data later
//
// Keeping it as a class so that we can add some functions here later 
// that might do some validation/conversion etc
//************************************************************************************


public class ToolTipData  
{
	

	//This contains different text data that we might need to show
	//Different tool tip types will use this as per their own implementation
	//SmallToolTipText is the bare minimum data we need to show a tool tip
	public Dictionary<ToolTipElementID, string> toolTipElementsDict;
	//Add further data here later


	//default contructor
	public ToolTipData()
	{
		toolTipElementsDict = new Dictionary<ToolTipElementID, string>();
	}

    public ToolTipData(ToolTipData other)
    {
        foreach(KeyValuePair<ToolTipElementID, string> entry in other.toolTipElementsDict)
        {
            toolTipElementsDict.Add(entry.Key, entry.Value);
        }   
    }

	//Simple constructor to add just a basic small tool tip text
	public ToolTipData(string _smallToolTipText)
	{
		toolTipElementsDict = new Dictionary<ToolTipElementID, string>();
		toolTipElementsDict.Add (ToolTipElementID.SmallToolTipText, _smallToolTipText);
	}

	public ToolTipData(string _smallToolTipText, string _detailedToolTipText)
	{
		toolTipElementsDict = new Dictionary<ToolTipElementID, string>();
		toolTipElementsDict.Add (ToolTipElementID.SmallToolTipText, _smallToolTipText);
		toolTipElementsDict.Add (ToolTipElementID.DetailedToolTipText, _detailedToolTipText);
	}

	public ToolTipData(string _smallToolTipText, string _detailedToolTipText, string _smallDescriptionImage)
	{
		toolTipElementsDict = new Dictionary<ToolTipElementID, string>();
		toolTipElementsDict.Add (ToolTipElementID.SmallToolTipText, _smallToolTipText);
		toolTipElementsDict.Add (ToolTipElementID.DetailedToolTipText, _detailedToolTipText);
		toolTipElementsDict.Add (ToolTipElementID.SmallDescriptionImage, _smallDescriptionImage);
	}

	// Methods to populate a tool tip data object
	// Tool tip data cannot have two elements of the same type
	// By default overwriting is also disabled, so that elements are not overwritten by mistake
	// Returns true or false based on whether data was succssfully added or not
	// TODO: need to replace bool with status codes
	public bool AddElement(ToolTipElementID elementID, string elementToAdd, bool overwrite = false)
	{
		if (toolTipElementsDict == null) 
		{
				//Invalid data
				return false;
		}

		if (!overwrite && toolTipElementsDict.ContainsKey (elementID)) {
			return false;
		}

		toolTipElementsDict.Add (elementID, elementToAdd);
		return true;

	}

	//Methods to query and get data elements
	public bool HasElement(ToolTipElementID elementID)
	{
		return toolTipElementsDict.ContainsKey (elementID);
	}

	public string GetElement(ToolTipElementID elementID)
	{
		return toolTipElementsDict [elementID];
	}
}

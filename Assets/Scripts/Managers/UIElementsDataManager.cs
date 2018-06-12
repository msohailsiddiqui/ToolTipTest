using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;


//    SmallToolTipText = 0,
//    DetailedToolTipText,
//    SmallDescriptionImage,
//    LargeDescriptionImage,
//    HelpLinkURL

public class UIElementsDataManager : MonoBehaviour
{
    private string uiElementsDataPath;
    private string uiElementsDataFile;
    private string tooltipsDataFile;
    private XMLDataHandler xmlDataHandler;
    private Dictionary<string, string> uiElementToToolTipIDsDict;
    private Dictionary<string, ToolTipData> toolTipIDToDataDict;

    // Use this for initialization
    public void Initialize ()
    {
        uiElementsDataFile = "UIElementsData";
        tooltipsDataFile = "ToolTipsData";
        uiElementsDataPath = "XML\\";
        string currentLocalizationPreset = ReferenceManager.Instance.localizationManagerRef.GetCurrentLocalizationPreset().ToString();
        Debug.Log("UIElementsDataManager::Initialize:currentLocalizationPreset: " + currentLocalizationPreset);
        uiElementsDataPath += currentLocalizationPreset + "\\";
        uiElementsDataFile += "_" + currentLocalizationPreset;
        Debug.Log("UIElementsDataManager::Initialize:uiElementsDataPath: " + uiElementsDataPath);
        xmlDataHandler = new XMLDataHandler();
        xmlDataHandler.loadXMLFromResources(uiElementsDataFile, uiElementsDataPath);

        uiElementToToolTipIDsDict = new Dictionary<string, string>();
        ParseUIElementsData();
        tooltipsDataFile += "_" + currentLocalizationPreset;
        xmlDataHandler.loadXMLFromResources(tooltipsDataFile, uiElementsDataPath);
        toolTipIDToDataDict = new Dictionary<string, ToolTipData>();
        ParseToolTipsData();
    }

    public void GetUIElementsData()
    {

    }

    private void LoadUIElementsData()
    {

    }

    private void ParseUIElementsData()
    {
        string uiElementID = "";
        string toolTipID = "";
        if (xmlDataHandler != null && xmlDataHandler.GetXMLDoc() != null)
        {
            foreach (XmlElement node in xmlDataHandler.GetXMLDoc().SelectNodes("UIElementsData/UIElementData"))
            {
                
                uiElementID = node.GetAttribute("UIElementID");
                toolTipID = node.SelectSingleNode("ToolTipID").InnerText;
                Debug.Log("UIElementsDataManager::ParseUIElementsData:Found uiElement: " + uiElementID + ", "+toolTipID);
                uiElementToToolTipIDsDict.Add(uiElementID, toolTipID);
            }
        }
    }

    private void ParseToolTipsData()
    {
        string ToolTipID = "";
        string SmallToolTipText = "";
        string DetailedToolTipText = "";
        string SmallDescriptionImage = "";
        string LargeDescriptionImage = "";
        string HelpLinkURL = "";

        if (xmlDataHandler != null && xmlDataHandler.GetXMLDoc() != null)
        {
            foreach (XmlElement node in xmlDataHandler.GetXMLDoc().SelectNodes("ToolTipsData/ToolTipData"))
            {
                ToolTipData temp = new ToolTipData();

                ToolTipID = node.GetAttribute("ToolTipID");
                SmallToolTipText = node.SelectSingleNode("SmallToolTipText").InnerText;
                if(!string.IsNullOrEmpty(SmallToolTipText))
                {
                    temp.AddElement(ToolTipElementID.SmallToolTipText, SmallToolTipText);
                    DetailedToolTipText = node.SelectSingleNode("DetailedToolTipText").InnerText;
                    if (!string.IsNullOrEmpty(DetailedToolTipText))
                        temp.AddElement(ToolTipElementID.DetailedToolTipText, DetailedToolTipText);
                    SmallDescriptionImage = node.SelectSingleNode("SmallDescriptionImage").InnerText;
                    if (!string.IsNullOrEmpty(SmallDescriptionImage))
                        temp.AddElement(ToolTipElementID.SmallDescriptionImage, SmallDescriptionImage);
                    LargeDescriptionImage = node.SelectSingleNode("LargeDescriptionImage").InnerText;
                    if (!string.IsNullOrEmpty(LargeDescriptionImage))
                        temp.AddElement(ToolTipElementID.LargeDescriptionImage, LargeDescriptionImage);
                    HelpLinkURL = node.SelectSingleNode("HelpLinkURL").InnerText;
                    if (!string.IsNullOrEmpty(HelpLinkURL))
                        temp.AddElement(ToolTipElementID.HelpLinkURL, HelpLinkURL);
                    toolTipIDToDataDict.Add(ToolTipID, temp);
                }
            }

            foreach (KeyValuePair<string, ToolTipData> entry in toolTipIDToDataDict)
            {
                Debug.Log("<color=purple>UIElementsDataManager::ParseToolTipsData:Found ToolTip: " + entry.Key +
                    " \n" + entry.Value.GetElement(ToolTipElementID.SmallToolTipText)
                    + " \n" + entry.Value.GetElement(ToolTipElementID.DetailedToolTipText) 
                    + " \n" + entry.Value.GetElement(ToolTipElementID.SmallDescriptionImage) 
                    + " \n" + entry.Value.GetElement(ToolTipElementID.LargeDescriptionImage) 
                    + " \n" + entry.Value.GetElement(ToolTipElementID.HelpLinkURL) + "</color>");
            }

        }
    }

    public ToolTipData GetToolTipForUIElement(string _elementID)
    {
        if(!string.IsNullOrEmpty(_elementID))
        {
            string _toolTipID = "";
            uiElementToToolTipIDsDict.TryGetValue(_elementID, out _toolTipID);
            if (!string.IsNullOrEmpty(_toolTipID))
            {
                ToolTipData temp = new ToolTipData();
                toolTipIDToDataDict.TryGetValue(_toolTipID, out temp);
                return temp;
            }
        }
        return null;
    }

}

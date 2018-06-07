using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipObjectPlacementTester : MonoBehaviour
{

    public ToolTipObjTest toolTipTestObject;
    public RectTransform UIElement1;

    //Different Types of data that the tool tip can handle
    ToolTipData smallTextData;
    ToolTipData detailedTextData;
    ToolTipData smallImageData;
    ToolTipData smallTextWithImageData;

    ToolTipPlacementData TLTR;
    ToolTipPlacementData TRTR;
    ToolTipPlacementData TRTL;
    ToolTipPlacementData BLTR;
    ToolTipPlacementData BRTR;
    ToolTipPlacementData BRTL;

    // Use this for initialization
    void Start ()
    {
        InitializeToolTipData();
        InitializeToolTipPlacements();

    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            //Populate the Tool Tip Obj
            toolTipTestObject.SetupToolTip(smallTextData);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            //Populate the Tool Tip Obj
            toolTipTestObject.SetupToolTip(smallTextWithImageData);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            //Populate the Tool Tip Obj
            toolTipTestObject.SetupToolTip(detailedTextData);
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            toolTipTestObject.SetupToolTip(smallImageData);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            toolTipTestObject.ResetToolTip();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            toolTipTestObject.UpdateToolTipAnchor(TLTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            toolTipTestObject.UpdateToolTipAnchor(TRTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            toolTipTestObject.UpdateToolTipAnchor(TRTL);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            toolTipTestObject.UpdateToolTipAnchor(BLTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            toolTipTestObject.UpdateToolTipAnchor(BRTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            toolTipTestObject.UpdateToolTipAnchor(BRTL);
        }

        if (Input.GetKeyUp(KeyCode.Alpha7))
        {

        }

        if (Input.GetKeyUp(KeyCode.Alpha8))
        {

        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            DebugUIInfo();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            if (toolTipTestObject.GetRectTransform() != null && UIElement1 != null)
            {
                toolTipTestObject.GetRectTransform().SetParent(UIElement1, false);
            }
        }


        // Implement Test cases for
        // 1. Button on top right of screen
        // 2. Button on top left of screen
        // 3. Button on bottom right of screen
        // 4. Button on bottom left of screen
        // 5. Check recursion where tip is not placeable in Left to right or Right to Left direction
        // 6. Check recursion where tip is not placeable in Left to right or Right to Left direction
    }

    private void InitializeToolTipData()
    {
        // Create a new Tool Tip Data
        // In the actual code this will be passed
        smallTextData = new ToolTipData("This yqgpj is a small tooltip only");
        detailedTextData = new ToolTipData("This yqgpj is the small tooltip with detailed text",
            "This is the large multiline Tool tip Text\n The quick brown fox jumped over the lazy dog");

        smallImageData = new ToolTipData("This yqgpj is the small tooltip text",
            "This is the large multiline Tool tip Text\n The quick brown fox jumped over the lazy dog",
        "smallImageTestRect64");

        smallTextWithImageData = new ToolTipData("This yqgpj is a small tooltip with image only");
        smallTextWithImageData.AddElement(ToolTipElementID.SmallDescriptionImage, "smallImageTestRect");

    }

    private void InitializeToolTipPlacements()
    {
        TLTR = new ToolTipPlacementData(UIElementHorizontalPlacement.Left, UIElementVerticalPlacement.Top, UIElementHorizontalDirection.LeftToRight);
        TRTR = new ToolTipPlacementData(UIElementHorizontalPlacement.Right, UIElementVerticalPlacement.Top, UIElementHorizontalDirection.LeftToRight);
        TRTL = new ToolTipPlacementData(UIElementHorizontalPlacement.Right, UIElementVerticalPlacement.Top, UIElementHorizontalDirection.RightToLeft);
        BLTR = new ToolTipPlacementData(UIElementHorizontalPlacement.Left, UIElementVerticalPlacement.Bottom, UIElementHorizontalDirection.LeftToRight);
        BRTR = new ToolTipPlacementData(UIElementHorizontalPlacement.Right, UIElementVerticalPlacement.Bottom, UIElementHorizontalDirection.LeftToRight);
        BRTL = new ToolTipPlacementData(UIElementHorizontalPlacement.Right, UIElementVerticalPlacement.Bottom, UIElementHorizontalDirection.RightToLeft);
    }

    private void DebugUIInfo()
    {
        Debug.Log("<color=magenta>***************************</color>", toolTipTestObject.gameObject);
        Debug.Log("UI Element Name: " + toolTipTestObject.gameObject.name);
        Debug.Log("Go Position: " + toolTipTestObject.gameObject.transform.position);
        RectTransform myRect = toolTipTestObject.gameObject.GetComponent<RectTransform>();
        Debug.Log("Rect is Available: " + myRect);
        if (myRect != null)
        {
            Debug.Log("Rect Pos: " + myRect.position);
            Debug.Log("Rect localPos: " + myRect.localPosition);
            Debug.Log("Rect Anchored Pos: " + myRect.anchoredPosition);
            Debug.Log("Rect Info: " + myRect.rect);
        }
    }
}

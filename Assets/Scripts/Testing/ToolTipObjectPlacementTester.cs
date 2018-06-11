using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipObjectPlacementTester : MonoBehaviour
{

    public ToolTipObj toolTipObject;
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
        if(toolTipObject != null )
        {
            toolTipObject.Initialize();
        }
        InitializeToolTipData();
        InitializeToolTipPlacements();
        toolTipObject.GetRectTransform().localScale = new Vector3(1, 1, 1);

    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            //Populate the Tool Tip Obj
            toolTipObject.SetupToolTip(smallTextData, TLTR);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            //Populate the Tool Tip Obj
            toolTipObject.SetupToolTip(smallTextWithImageData, TLTR);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            //Populate the Tool Tip Obj
            toolTipObject.SetupToolTip(detailedTextData, TLTR);
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            toolTipObject.SetupToolTip(smallImageData, TLTR);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            toolTipObject.ResetToolTip();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            toolTipObject.UpdateToolTipPlacement(TLTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            toolTipObject.UpdateToolTipPlacement(TRTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            toolTipObject.UpdateToolTipPlacement(TRTL);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            toolTipObject.UpdateToolTipPlacement(BLTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            toolTipObject.UpdateToolTipPlacement(BRTR);
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            toolTipObject.UpdateToolTipPlacement(BRTL);
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
            if (toolTipObject.GetRectTransform() != null && UIElement1 != null)
            {
                toolTipObject.GetRectTransform().SetParent(UIElement1, false);
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
        Debug.Log("<color=magenta>***************************</color>", toolTipObject.gameObject);
        Debug.Log("UI Element Name: " + toolTipObject.gameObject.name);
        Debug.Log("Go Position: " + toolTipObject.gameObject.transform.position);
        RectTransform myRect = toolTipObject.gameObject.GetComponent<RectTransform>();
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

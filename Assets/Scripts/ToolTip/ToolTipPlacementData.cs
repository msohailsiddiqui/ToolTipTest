using System;
using UnityEngine;


//************************************************************************************
// This class holds the data that is needed to place the tool tip on a Unity UI Object
// Currently it handles placing the tool tip at:
// 1. Top Left of the UI object - Tool Tip grows upwards vertically and towards right horizontally
//                                In this case the tool tip is placed above the object and the pivot is at the bottom left
// 2. Top Right of the UI object - Tool Tip grows upwards vertically and towards right horizontally
//                                In this case the tool tip is placed above the object and the pivot is at the bottom left
// 3. Top Right of the UI object - Tool Tip grows upwards vertically and towards left horizontally
//                                In this case the tool tip is placed above the object and the pivot is at the bottom right
// 4. Bottom Left of the UI object - Tool Tip grows downwards vertically and towards right horizontally
//                                In this case the tool tip is placed below the object and the pivot is at the top left
// 5. Bottom Right of the UI object - Tool Tip grows downwards vertically and towards right horizontally
//                                In this case the tool tip is placed below the object and the pivot is at the top left
// 6. Bottom Left of the UI object - Tool Tip grows downwards vertically and towards right horizontally
//                                In this case the tool tip is placed below the object and the pivot is at the top right
// This class automatically sets the values for the pivot and anchor based on the specified placement data
//************************************************************************************

public class ToolTipPlacementData : UIElementPlacementData
{
    public ToolTipPlacementData()
    {
        // by default we want the tool tip to be anchored at the bottom left (0,0)
        // and pivoted at the top left (0,1), this means that horizontal placement will be Left, 
        // Vertical Placement to be bottom and direction to be Left To Right

        HorizontalPlacement = UIElementHorizontalPlacement.Left;
        VerticalPlacement = UIElementVerticalPlacement.Bottom;
        HorizontalDirection = UIElementHorizontalDirection.LeftToRight;

        UpdatePivotAndAnchors();
    }
    public ToolTipPlacementData(UIElementHorizontalPlacement _horizontalPlacement, UIElementVerticalPlacement _verticalPlacement,
        UIElementHorizontalDirection _horizontalDirection)
    {
        HorizontalPlacement = _horizontalPlacement;
        VerticalPlacement = _verticalPlacement;
        HorizontalDirection = _horizontalDirection;

        UpdatePivotAndAnchors();
    }

    private void UpdatePivotAndAnchors()
    {
        float anchorHorizontal = 0;
        float anchorVertical = 0;
        float pivotHorizontal = 0;
        float pivotVertical = 0;

        switch (HorizontalPlacement)
        {
            case UIElementHorizontalPlacement.Left:
                anchorHorizontal = 0;
                pivotHorizontal = 0;
                break;
            case UIElementHorizontalPlacement.Right:
                anchorHorizontal = 1;
                pivotHorizontal = 0;
                break;
            default:
                Debug.LogError("ToolTipPlacementData: Invalid Horizontal Placement");
                break;
        }

        switch (VerticalPlacement)
        {
            case UIElementVerticalPlacement.Top:
                anchorVertical = 1;
                pivotVertical = 0;
                break;
            case UIElementVerticalPlacement.Bottom:
                anchorVertical = 0;
                pivotVertical = 1;
                break;
            default:
                Debug.LogError("ToolTipPlacementData: Invalid Vertical Placement");
                break;
        }

        switch (HorizontalDirection)
        {
            case UIElementHorizontalDirection.LeftToRight:
                pivotHorizontal = 0;
                break;
            case UIElementHorizontalDirection.RightToLeft:
                pivotHorizontal = 1;
                break;
            default:
                Debug.LogError("ToolTipPlacementData: Invalid direction");
                break;
        }

        RequiredAnchorMin = new Vector2(anchorHorizontal, anchorVertical);
        RequiredAnchorMax = new Vector2(anchorHorizontal, anchorVertical);
        RequiredPivot = new Vector2(pivotHorizontal, pivotVertical);
    }

    public void UpdateToolTipHorizontalDirection(UIElementHorizontalDirection _updatedHorizontalDirection)
    {
        HorizontalDirection = _updatedHorizontalDirection;
        UpdatePivotAndAnchors();
    }

    public void UpdateToolTipVerticalPlacement(UIElementVerticalPlacement _updatedVerticalPlacement)
    {
        VerticalPlacement = _updatedVerticalPlacement;
        UpdatePivotAndAnchors();
    }
}

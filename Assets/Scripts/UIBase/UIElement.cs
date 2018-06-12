using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIElementVerticalPlacement
{
    Top = 0,
    Bottom
}

public enum UIElementHorizontalPlacement
{
    Left = 0,
    Right
}

public enum UIElementHorizontalDirection
{
    LeftToRight = 0,
    RightToLeft
}

public class UIElementPlacementData
{
    private UIElementVerticalPlacement verticalPlacement;
    private UIElementHorizontalPlacement horizontalPlacement;
    private UIElementHorizontalDirection horizontalDirection;

    private Vector2 requiredPivot;
    private Vector2 requiredAnchorMax;
    private Vector2 requiredAnchorMin;

    public UIElementVerticalPlacement VerticalPlacement
    {
        get
        {
            return verticalPlacement;
        }

        set
        {
            verticalPlacement = value;
        }
    }

    public UIElementHorizontalPlacement HorizontalPlacement
    {
        get
        {
            return horizontalPlacement;
        }

        set
        {
            horizontalPlacement = value;
        }
    }

    public UIElementHorizontalDirection HorizontalDirection
    {
        get
        {
            return horizontalDirection;
        }

        set
        {
            horizontalDirection = value;
        }
    }

    public Vector2 RequiredPivot
    {
        get
        {
            return requiredPivot;
        }

        set
        {
            requiredPivot = value;
        }
    }

    public Vector2 RequiredAnchorMax
    {
        get
        {
            return requiredAnchorMax;
        }

        set
        {
            requiredAnchorMax = value;
        }
    }

    public Vector2 RequiredAnchorMin
    {
        get
        {
            return requiredAnchorMin;
        }

        set
        {
            requiredAnchorMin = value;
        }
    }
}

public class UIElement : MonoBehaviour
{
    public string elementID;
}


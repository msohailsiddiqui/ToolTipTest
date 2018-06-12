using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementWithToolTip : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        ReferenceManager.Instance.toolTipControllerRef.ToolTipNeedsToBeShown(elementID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ReferenceManager.Instance.toolTipControllerRef.RemoveToolTip();
    }

}

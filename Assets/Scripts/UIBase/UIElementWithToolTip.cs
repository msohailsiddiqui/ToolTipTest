using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementWithToolTip : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("<color=green>UIElementWithToolTip::OnPointerEnter: " + gameObject.name + "<color>");
        ReferenceManager.Instance.toolTipControllerRef.ToolTipNeedsToBeShown(elementID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("<color=blue>UIElementWithToolTip::OnPointerExit: " + gameObject.name + "<color>");
        ReferenceManager.Instance.toolTipControllerRef.RemoveToolTip();
    }

}

using UnityEngine.EventSystems;

public interface ICanShowToolTip : IPointerEnterHandler, IPointerExitHandler
{
    void OnReceiveFocus();

    void OnLostFocus();
}
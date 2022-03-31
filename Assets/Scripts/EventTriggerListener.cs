using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    #region 变量
    //带参数是为了方便取得绑定了UI事件的对象
    public delegate void UIDelegate(GameObject go);
    public delegate void UIDelegateWithData(GameObject go, PointerEventData eventData);

    public UIDelegate onPointerEnter = delegate { };
    public UIDelegate onPointerExit = delegate { };
    public UIDelegate onPointerDown = delegate { };
    public UIDelegate onPointerUp = delegate { };
    public UIDelegate onPointerClick = delegate { };
    public UIDelegate onInitializePotentialDrag = delegate { };
    public UIDelegate onBeginDrag = delegate { };
    public UIDelegate onDrag = delegate { };
    public UIDelegate onEndDrag = delegate { };
    public UIDelegate onDrop = delegate { };
    public UIDelegate onScroll = delegate { };
    public UIDelegate onUpdateSelected = delegate { };
    public UIDelegate onSelect = delegate { };
    public UIDelegate onDeselect = delegate { };
    public UIDelegate onMove = delegate { };
    public UIDelegate onSubmit = delegate { };
    public UIDelegate onCancel = delegate { };
    public UIDelegateWithData onInitializePotentialDragData = delegate{};
    public UIDelegateWithData onBeginDragData = delegate{};
    public UIDelegateWithData onDragData = delegate{};
    public UIDelegateWithData onEndDragData = delegate{};
    #endregion

    public static EventTriggerListener GetListener(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }

    #region 方法
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onPointerEnter != null) onPointerEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onPointerExit != null) onPointerExit(gameObject);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onPointerDown != null) onPointerDown(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onPointerUp != null) onPointerUp(gameObject);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onPointerClick != null) onPointerClick(gameObject);
    }
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (onInitializePotentialDrag != null) onInitializePotentialDrag(gameObject);
        if(onInitializePotentialDragData != null)onInitializePotentialDragData(gameObject,eventData);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(gameObject);
        if(onBeginDragData != null) onBeginDragData(gameObject,eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject);
        if(onDragData != null)onDragData(gameObject,eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(gameObject);
        if(onEndDragData != null) onEndDragData(gameObject,eventData);
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null) onDrop(gameObject);
    }
    public override void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null) onScroll(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelected != null) onUpdateSelected(gameObject);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        if (onDeselect != null) onDeselect(gameObject);
    }
    public override void OnMove(AxisEventData eventData)
    {
        if (onMove != null) onMove(gameObject);
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(gameObject);
    }
    public override void OnCancel(BaseEventData eventData)
    {
        if (onCancel != null) onCancel(gameObject);
    }
    #endregion

    public void RemoveOnPointerEnter()
    {
        if (onPointerEnter != null)
            onPointerEnter = delegate { };
    }

    public void RemoveOnPointerExit()
    {
        if (onPointerExit != null)
            onPointerExit = delegate { };
    }

    public void RemoveOnPointerUp()
    {
        if (onPointerUp != null)
            onPointerUp = delegate { };
    }

    public void RemoveOnPointerDown()
    {
        if (onPointerDown != null)
            onPointerDown = delegate { };
    }

    public void RemoveOnPointerClick()
    {
        if (onPointerClick != null)
            onPointerClick = delegate { };
    }

    public void RemoveOnInitializePotentialDrag()
    {
        if (onInitializePotentialDrag != null)
            onInitializePotentialDrag = delegate { };
    }

    public void RemoveOnBeginDrag()
    {
        if (onBeginDrag != null)
            onBeginDrag = delegate { };
    }

    public void RemoveOnBeginDragData()
    {
        if(onBeginDragData != null)
            onBeginDragData = delegate{};
    }

    public void RemoveOnDrag()
    {
        if (onDrag != null)
            onDrag = delegate { };
    }

    public void RemoveOnDragData()
    {
        if(onDragData != null)
            onDragData = delegate{};
    }

    public void RemoveOnEndDrag()
    {
        if (onEndDrag != null)
            onEndDrag = delegate { };
    }

    public void RemoveOnEndDragData()
    {
        if(onEndDragData != null)
            onEndDragData = delegate{};
    }

    public void RemoveOnDrop()
    {
        if (onDrop != null)
            onDrop = delegate { };
    }

    public void RemoveOnScroll()
    {
        if (onScroll != null)
            onScroll = delegate { };
    }

    public void RemoveOnUpdateSelected()
    {
        if (onUpdateSelected != null)
            onUpdateSelected = delegate { };
    }

    public void RemoveOnSelect()
    {
        if (onSelect != null)
            onSelect = delegate { };
    }

    public void RemoveOnDeselect()
    {
        if (onDeselect != null)
            onDeselect = delegate { };
    }

    public void RemoveOnMove()
    {
        if (onMove != null)
            onMove = delegate { };
    }

    public void RemoveOnSubmit()
    {
        if (onSubmit != null)
            onSubmit = delegate { };
    }

    public void RemoveOnCancel()
    {
        if (onCancel != null)
            onCancel = delegate { };
    }
}

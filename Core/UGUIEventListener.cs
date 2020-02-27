using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UKit.Utils.Output;

public class UGUIEventListener : EventTrigger
{
    public UnityAction<GameObject, PointerEventData> onClick;
    public UnityAction<GameObject, PointerEventData> onEnter;
    public UnityAction<GameObject, PointerEventData> onExit;

    public override void OnPointerClick(PointerEventData eventData){
        base.OnPointerClick(eventData);
        if(onClick != null){
            onClick(gameObject, eventData);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData){
        base.OnPointerEnter(eventData);
        if(onEnter != null){
            onEnter(gameObject, eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData){
        base.OnPointerExit(eventData);
        if(onExit != null){
            onExit(gameObject, eventData);
        }
    }

    /// <summary>
    /// 获取或者添加UGUIEventListener脚本来实现对游戏对象的监听
    /// </summary>
    public static UGUIEventListener Get(GameObject go){
        UGUIEventListener listener = go.GetComponent<UGUIEventListener>();
        if(listener == null){
            listener = go.AddComponent<UGUIEventListener>();
        }
        return listener;
    }
}

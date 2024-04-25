using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressableElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    bool isPressing;
    public bool IsPressing => isPressing;

    public void OnPointerDown(PointerEventData eventData) {
        isPressing = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPressing = false;
    }

}
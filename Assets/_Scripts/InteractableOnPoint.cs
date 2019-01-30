using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableOnPoint : EventTrigger
{
    private void Start() {
        enabled = false;
    }
    public override void OnPointerEnter(PointerEventData data) {
        //Debug.Log("OnPointerEnter called.");
    }

 
}

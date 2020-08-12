using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementDragger : EventTrigger 
{
    private bool dragging;
    public void Update() 
    {
        if (dragging) {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // Debug.LogFormat("({0},{1})",transform.position.x, transform.position.y);
        }
    }
    public override void OnPointerDown(PointerEventData eventData) 
    {
        dragging = true;
    }
    public override void OnPointerUp(PointerEventData eventData) 
    {
        dragging = false;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VCast.UI.DragDrop
{

    [RequireComponent(typeof(RectTransform))]
    public class UIElementDropAnchor : EventTrigger
    {    
        RectTransform m_RectTransform;
        UIElementCornerAnchors m_CornerAnchors;
        List<DropAnchor> m_DropAnchors = new List<DropAnchor>();
        
        public override void OnPointerUp(PointerEventData eventData) 
        {
            CheckAnchors();
        }
        private void CheckAnchors ()
        {
            foreach (DropAnchor da in m_DropAnchors)
            {
                if (transform.position.x > da.area.x && 
                    transform.position.x < (da.area.x + da.area.width) &&
                    transform.position.y > da.area.y && 
                    transform.position.y < (da.area.y + da.area.height))            
                {
                    transform.position = new Vector2(da.anchor.x, da.anchor.y);
                }
            }
        }
        private void PrintAnchors ()
        {
            foreach (DropAnchor da in m_DropAnchors)
            {
                Debug.Log(da.anchor.x);
                Debug.Log(da.anchor.y);            
            }
        }
        private void Start() 
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_CornerAnchors = GetComponent<UIElementCornerAnchors>();
            m_DropAnchors = m_CornerAnchors.anchors;            
            CheckAnchors();
        }
    }
}
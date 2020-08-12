using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropAnchor 
{
    public Vector2 anchor;
    public Rect area;
    public DropAnchor(Vector2 anchor, Rect area)
    {
        this.anchor = anchor;
        this.area = area;
    }
}

[RequireComponent(typeof(RectTransform))]
public class UIElementCornerAnchors : MonoBehaviour
{   
    [SerializeField] Vector2 offset; 
    public List<DropAnchor> anchors =  new List<DropAnchor>();
    RectTransform m_RectTransform;
    Vector2 m_Offset;

    private void Awake() 
    {
        m_RectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable() 
    {
        m_Offset = new Vector2(m_RectTransform.rect.width/2 + offset.x, m_RectTransform.rect.height/2 + offset.y);
        Debug.LogFormat("({0},{1})", m_Offset.x, m_Offset.y);

        DropAnchor bottomLeft = new DropAnchor(new Vector2(m_Offset.x, m_Offset.y), new Rect(0,0,Screen.width/2, Screen.height/2));
        DropAnchor bottomRight = new DropAnchor(new Vector2(Screen.width - m_Offset.x, m_Offset.y), new Rect(Screen.width/2, 0,Screen.width/2, Screen.height/2));
        DropAnchor topLeft = new DropAnchor(new Vector2(m_Offset.x, Screen.height - m_Offset.y), new Rect(0,Screen.height/2, Screen.width/2, Screen.height/2));
        DropAnchor topRight = new DropAnchor(new Vector2(Screen.width - m_Offset.x, Screen.height - m_Offset.y), new Rect(Screen.width/2, Screen.height/2, Screen.width/2, Screen.height/2));

        anchors.Add(bottomLeft);
        anchors.Add(bottomRight);
        anchors.Add(topLeft);
        anchors.Add(topRight);
    }
    private void OnDisable() 
    {
        anchors.Clear();    
    }
}

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class ListCellBase : MonoBehaviour,IPointerClickHandler {
    public Action<int> onClicked;

    public bool m_selected;

    private int m_index;
    public int index
    {
        get { return m_index; }
        set
        {
            m_index = value;
            name = value.ToString();
        }
    }

    private RectTransform m_trans;
    public RectTransform rectTrans
    {
        get
        {
            if (m_trans == null)
                m_trans = transform.GetComponent<RectTransform>();
            return m_trans;
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (onClicked != null)
            onClicked(m_index);
    }

    public virtual void SetData(object data)
    {
        if(data==null)
        {
            if(gameObject.activeSelf)
                gameObject.SetActive(false);
            return;
        }
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public virtual void SetSelected(bool selected)
    {
        m_selected = selected;
    }
}

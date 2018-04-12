using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComputeImage : MaskableGraphic {

    public CanvasRenderer cr;
    //public RectTransform rectTrans;
    public Canvas m_canvas;

    //public Vector3 left;
    //public Vector3 right;
    //public bool drawing = false;

	// Use this for initialization
	protected override void Start () {
        cr = GetComponent<CanvasRenderer>();
        //rectTrans = transform as RectTransform;
        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();


        rectTransform.anchorMin = Vector2.zero; // 锚点左下角
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.pivot = Vector2.zero; // 轴心左下角
        rectTransform.offsetMin = Vector2.zero; // 相对锚点偏移
        rectTransform.offsetMax = Vector2.zero;
    }
	

	// Update is called once per frame
	void Update () {
        //Debug.DrawLine(rectWorldCorner[0], rectWorldCorner[2], Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            //drawing = true;
            //left = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("鼠标："+Input.mousePosition.ToString() + "  scale:" + m_canvas.scaleFactor.ToString());
            //rectTrans.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //rectTrans.offsetMin=new Vector2(Input.mousePosition.x/canvas.scaleFactor, Input.mousePosition.y/canvas.scaleFactor);

            p1 = new Vector3(Input.mousePosition.x / m_canvas.scaleFactor, Input.mousePosition.y / m_canvas.scaleFactor, 0);
        }
        if (Input.GetMouseButton(0))
        {
            p2=new Vector3(Input.mousePosition.x / m_canvas.scaleFactor, Input.mousePosition.y / m_canvas.scaleFactor, 0);
            SetVerticesDirty();
        }
    }

    Vector3 p1;
    Vector3 p2;
    private void DrawMesh(VertexHelper vh)
    {
        vh.Clear();

        vh.AddVert(p1, color, new Vector2(0, 0));
        vh.AddVert(new Vector3(p1.x, p2.y, 0), color, new Vector2(0, 1));
        vh.AddVert(p2, color, new Vector2(1, 1));
        vh.AddVert(new Vector3(p2.x, p1.y, 0), color, new Vector2(1, 0));
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);

    }

    public virtual void SetMesh()
    {
        VertexHelper vh = new VertexHelper();

        Color color = Color.green;
        Rect rect = rectTransform.rect;
        vh.AddVert(new Vector3(rect.xMin, rect.yMin,0), color, new Vector2(0, 0));
        vh.AddVert(new Vector3(rect.xMin, rect.yMax, 0), color, new Vector2(0, 1));
        vh.AddVert(new Vector3(rect.xMax, rect.yMax, 0), color, new Vector2(1, 1));
        vh.AddVert(new Vector3(rect.xMax, rect.yMin, 0), color, new Vector2(1, 0));
        vh.AddTriangle(0, 3, 2);
        vh.AddTriangle(0, 2, 1);

        Mesh mesh = new Mesh();
        vh.FillMesh(mesh);
        cr.SetMesh(mesh);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        //Color color = Color.green;
        //Rect rect = rectTrans.rect;
        //vh.Clear();
        //vh.AddVert(new Vector3(rect.xMin, rect.yMin, 0), color, new Vector2(0, 0));
        //vh.AddVert(new Vector3(rect.xMin, rect.yMax, 0), color, new Vector2(0, 1));
        //vh.AddVert(new Vector3(rect.xMax, rect.yMax, 0), color, new Vector2(1, 1));
        //vh.AddVert(new Vector3(rect.xMax, rect.yMin, 0), color, new Vector2(1, 0));

        //vh.AddVert(new Vector3(0, 0, 0), color, new Vector2(0, 0));
        //vh.AddVert(new Vector3(0, 100, 0), color, new Vector2(0, 1));
        //vh.AddVert(new Vector3(100, 100, 0), color, new Vector2(1, 1));
        //vh.AddVert(new Vector3(100, 0, 0), color, new Vector2(1, 0));
        //vh.AddTriangle(0, 1, 2);
        //vh.AddTriangle(2, 3, 0);

        //Debug.Log(rect);

        DrawMesh(vh);
    }
}


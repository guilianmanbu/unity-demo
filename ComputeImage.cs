using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComputeImage : MaskableGraphic {

    public Canvas m_canvas;
    Vector3 p1;
    Vector3 p2;

    // Use this for initialization
    protected override void Start () {
        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        // 父级铺满屏幕
        rectTransform.anchorMin = Vector2.zero; // 锚点左下角
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.pivot = Vector2.zero; // 轴心左下角
        rectTransform.offsetMin = Vector2.zero; // 相对锚点偏移
        rectTransform.offsetMax = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            p1 = new Vector3(Input.mousePosition.x / m_canvas.scaleFactor, Input.mousePosition.y / m_canvas.scaleFactor, 0);
        }
        if (Input.GetMouseButton(0))
        {
            p2=new Vector3(Input.mousePosition.x / m_canvas.scaleFactor, Input.mousePosition.y / m_canvas.scaleFactor, 0);
            SetVerticesDirty();
        }
    }
    // Canva的RenderMode设为ScreenSpace后，即屏幕空间，Canvas下的child的坐标单位为像素。 Canvas的尺寸一般设置与屏幕的分辨率一致而正好铺满屏幕。
    // Input.mousePosition 是鼠标的屏幕空间像素坐标，与Canvas的child处于同一空间，所以可以直接用鼠标位置来计算UI元素的位置而不需做空间变换。
    // 需注意的是，鼠标位置需要除以canvas.scaleFactor这一缩放因子，来得到实际的游戏窗口内坐标。
    // canvas.scaleFactor 缩放因子 = （using resolution) / (refference resolution)
    // using resolution 是当前调试窗口内游戏的实际分辨率（调试窗口的左上角），unity对游戏窗口保持宽高比不变进行缩放以使游戏窗口能适应调试窗口。

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        vh.AddVert(p1, color, new Vector2(0, 0));
        vh.AddVert(new Vector3(p1.x, p2.y, 0), color, new Vector2(0, 1));
        vh.AddVert(p2, color, new Vector2(1, 1));
        vh.AddVert(new Vector3(p2.x, p1.y, 0), color, new Vector2(1, 0));
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}


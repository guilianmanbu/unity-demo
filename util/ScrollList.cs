using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;



public class ScrollList : MonoBehaviour {

    public Action<int> onCellClicked;

    [SerializeField]
    private bool vertical = true;
    [SerializeField]
    private int row = 1;
    [SerializeField]
    private int column = 1;
    //private float cellScale = 1;  // 现在做法是不改变cell的大小。如果要改为cell去适应 ScrollRect 区域，可以使用cellScale缩放。

    private ScrollRect scrollRect;
    private RectTransform rectTransform;
    [Header("可省略，会自动创建")]
    public RectTransform content;
    public RectTransform cellPrefab;

    private List<ListCellBase> cells = new List<ListCellBase>();
    private List<object> datas = new List<object>();

    private float height_scroll;
    private float height_content;
    private float height_cell;

    private float width_scroll;
    private float width_content;
    private float width_cell;

    private float extraLength;  // content 长度超出 rect 的长度.  normalizedPosition 就是 相对于这个长度的比例。

    private int prevRow;
    private int currentRow;
    private int row_data;
    private int prevColumn;
    private int currentColumn;
    private int column_data;

    public int currentSelectedIndex
    {
        get;
        protected set;
    }

    private void Awake()
    {
        SetUp();
    }

    //private void Start()
    //{
    //    // test
    //    for (int i = 0; i < 13; i++)
    //    {
    //        datas.Add(i);
    //    }
    //    SetDatas(datas);
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.LeftArrow))
    //        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - 0.1f);
    //    if (Input.GetKeyUp(KeyCode.RightArrow))
    //        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + 0.1f);

    //    if (Input.GetKeyDown(KeyCode.S))
    //        ScrollToCell(0);
    //    if (Input.GetKeyDown(KeyCode.E))
    //        ScrollToCell(datas.Count * 2);

    //    if (Input.GetKeyDown(KeyCode.U))
    //        UpdateAllCells();

    //    if (Input.GetKeyDown(KeyCode.A))
    //        UpdateDatasAndKeepLocation(datas);

    //    if (Input.GetKeyDown(KeyCode.C))
    //        Clear();
    //}

    private void OnDestroy()
    {
        
    }

    // Initialize
    public void SetUp()
    {
        rectTransform = transform as RectTransform;

        // scrollrect
        scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null) scrollRect = gameObject.AddComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(OnValueChanged);

        // content
        Transform contentTrans = transform.FindChild("content");
        if (contentTrans == null)
        {
            contentTrans = new GameObject("content", typeof(RectTransform)).transform;
            contentTrans.SetParent(transform, false);
        }
        content = contentTrans as RectTransform;
        content.anchorMax = new Vector2(0, 1);
        content.anchorMin = new Vector2(0, 1);
        content.pivot = new Vector2(0, 1);
        content.anchoredPosition = Vector2.zero;

        scrollRect.content = content;
        scrollRect.vertical = vertical;
        scrollRect.horizontal = !vertical;

        // rect2D mask
        if (transform.GetComponent<RectMask2D>() == null)
            gameObject.AddComponent<RectMask2D>();
        // canvasGroup
        if (gameObject.GetComponent<CanvasGroup>() == null)
            gameObject.AddComponent<CanvasGroup>();

        // height  width
        height_scroll = rectTransform.rect.height;
        height_cell = cellPrefab.rect.height;

        width_scroll = rectTransform.rect.width;
        width_cell = cellPrefab.rect.width;

        // row and column
        if (vertical)
        {
            row = Mathf.CeilToInt(height_scroll / height_cell) + 1;
            column = Mathf.Clamp(column, 1, Mathf.FloorToInt(width_scroll / width_cell));
        }
        else
        {
            row = Mathf.Clamp(row, 1, Mathf.FloorToInt(height_scroll / height_cell));
            column = Mathf.CeilToInt(width_scroll / width_cell) + 1;
        }

        // create cells
        if (cells.Count == 0)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    GameObject clone = Instantiate<GameObject>(cellPrefab.gameObject);
                    RectTransform rectTrans = clone.GetComponent<RectTransform>();
                    rectTrans.SetParent(content, false);
                    rectTrans.localScale = Vector3.one;
                    rectTrans.anchorMax = new Vector2(0, 1);
                    rectTrans.anchorMin = new Vector2(0, 1);
                    rectTrans.pivot = new Vector2(0, 1);

                    ListCellBase cell = clone.GetComponent<ListCellBase>();
                    cell.onClicked = OnCellClicked;

                    cells.Add(cell);
                }
            }
        }
        cellPrefab.gameObject.SetActive(false);
    }

    public void SetDatas(List<object> _datas)
    {
        datas = _datas != null ? _datas : new List<object>();
        scrollRect.StopMovement();
        SetContentSize();
        InitCells();
    }

    public void Clear()
    {
        SetDatas(null);
    }

    public void SetSelectedIndex(int index)
    {
        currentSelectedIndex = index;

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].SetSelected(cells[i].index == index);
        }
    }

    public void ScrollToCell(int index)
    {
        // all in show
        if (extraLength <= 0)
            return;

        if (vertical)
        {
            //  标准化位置 = 目标位置之前的长度 / 可滑动长度（即 extraLength)
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01((1f - (index / column) * height_cell / extraLength));
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01((index / row) * width_cell / extraLength);
        }
    }

    public void UpdateAllCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            ListCellBase cell = cells[i];
            object data = (cell.index >= 0 && cell.index < datas.Count) ? datas[cell.index] : null;
            cell.SetData(data);
            cell.SetSelected(cell.index == currentSelectedIndex);
        }
    }

    public void UpdateDatasAndKeepLocation(List<object> _newDatas)
    {
        float prevLocation = vertical ? scrollRect.verticalNormalizedPosition : scrollRect.horizontalNormalizedPosition;

        SetDatas(_newDatas);
        
        if (vertical)
            scrollRect.verticalNormalizedPosition = prevLocation;
        else
            scrollRect.horizontalNormalizedPosition = prevLocation;

        prevRow = prevColumn = -1;  // 之前的记录失效
        OnValueChanged(Vector2.zero);
    }

    private void SetContentSize()
    {
        if (vertical)
        {
            // set content size
            row_data = Mathf.CeilToInt(1f * datas.Count / column);

            height_content = row_data * height_cell;
            width_content = column * width_cell;

            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(width_content, height_content);

            extraLength = height_content - height_scroll;

            prevRow = currentRow = 0;
        }
        else
        {
            column_data = Mathf.CeilToInt(1f * datas.Count / row);

            height_content = row * height_cell;
            width_content = column_data * width_cell;

            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(width_content, height_content);

            extraLength = width_content - width_scroll;

            prevColumn = currentColumn = 0;
        }
    }

    private void InitCells()
    {
        if (vertical)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    ListCellBase cell = cells[i * column + j];
                    cell.index = i * column + j;
                    cell.rectTrans.anchoredPosition = new Vector2(j * width_cell, -i * height_cell);

                    object data = (cell.index >= 0 && cell.index < datas.Count) ? datas[cell.index] : null;
                    cell.SetData(data);
                    cell.SetSelected(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    ListCellBase cell = cells[i * row + j];
                    cell.index = i * row + j;
                    cell.rectTrans.anchoredPosition = new Vector2(i * width_cell, -j * height_cell);

                    object data = (cell.index >= 0 && cell.index < datas.Count) ? datas[cell.index] : null;
                    cell.SetData(data);
                    cell.SetSelected(false);
                }
            }
        }
        currentSelectedIndex = -1;
    }

    private void OnValueChanged(Vector2 value)
    {
        //print("value change:" + value.ToString());
        //print("change:" + value.y + "  normalVertical:" + scrollRect.verticalNormalizedPosition+" calcul:"+(1-content.anchoredPosition.y/(height_content-height_scroll)) );
        //print("bottom:" + (height_content - content.anchoredPosition.y - height_scroll) / (height_content - height_scroll));

        if (vertical)
        {
            if (row >= row_data)  // 全部显示了，不需要循环
                return;

            // 行索引从0开始 。 当前行索引取上部超出框的行数，也就是上部的完整行将循环到底部
            currentRow = Mathf.FloorToInt(((1 - scrollRect.verticalNormalizedPosition) * extraLength) / height_cell);

            currentRow = Mathf.Clamp(currentRow, 0, row_data - row);  // 可滑动行区间 [0,row_data-row]

            if (prevRow == currentRow)
                return;

            prevRow = currentRow;

            LoopCellsVertical();
        }
        else
        {
            if (column >= column_data)
                return;

            currentColumn = Mathf.FloorToInt(scrollRect.horizontalNormalizedPosition * extraLength / width_cell);

            currentColumn = Mathf.Clamp(currentColumn, 0, column_data - column);  // 钳制在可动行区间 [0,column_data-column] , 此区间之外的行不需要循环

            if (prevColumn == currentColumn)    // 优化
                return;

            prevColumn = currentColumn;

            LoopCellsHorizontal();
        }
    }

    private void LoopCellsVertical()
    {
        for (int i = 0; i < row; i++)
        {
            ListCellBase firstCellOfThisRow = cells[i * column + 0];

            int rowNumOfThisRow = firstCellOfThisRow.index / column;
            int newRowNum = rowNumOfThisRow;
            if (rowNumOfThisRow<currentRow)
            {
                newRowNum += row * ((currentRow - rowNumOfThisRow) / row + 1);  // 中间隔的页数 * 每页行数
            }
            else if (rowNumOfThisRow >= currentRow + row)  // 下一页 或 之后的页
            {
                newRowNum -= row * ((rowNumOfThisRow - currentRow) / row);      // 隔的页数 * 每页行数
            }

            if (newRowNum != rowNumOfThisRow)
            {
                for (int j = 0; j < column; j++)
                {
                    ListCellBase cell = cells[i * column + j];
                    cell.index = newRowNum * column + j;
                    cell.rectTrans.anchoredPosition = new Vector2(j * width_cell, -newRowNum * height_cell);

                    object data = (cell.index >= 0 && cell.index < datas.Count) ? datas[cell.index] : null;
                    cell.SetData(data);
                    cell.SetSelected(cell.index == currentSelectedIndex);
                }
            }
        }
    }

    private void LoopCellsHorizontal()
    {
        for (int i = 0; i < column; i++)
        {
            ListCellBase firstCellOfThisColumn = cells[i * row + 0];
            int columnNumOfThisColumn = firstCellOfThisColumn.index / row;
            int newColumnNum = columnNumOfThisColumn;
            if (columnNumOfThisColumn < currentColumn)
                newColumnNum += column * ((currentColumn - columnNumOfThisColumn) / column + 1);
            else if (columnNumOfThisColumn >= currentColumn + column)
                newColumnNum -= column * ((columnNumOfThisColumn - currentColumn) / column);

            if (newColumnNum != columnNumOfThisColumn)
            {
                for (int j = 0; j < row; j++)
                {
                    ListCellBase cell = cells[i * row + j];
                    cell.index = newColumnNum * row + j;
                    cell.rectTrans.anchoredPosition = new Vector2(newColumnNum * width_cell, -j * height_cell);

                    object data = (cell.index >= 0 && cell.index < datas.Count) ? datas[cell.index] : null;
                    cell.SetData(data);
                    cell.SetSelected(cell.index == currentSelectedIndex);
                }
            }
        }
    }

    private void OnCellClicked(int index)
    {
        SetSelectedIndex(index);

        if(onCellClicked!=null) onCellClicked(index);
    }
}

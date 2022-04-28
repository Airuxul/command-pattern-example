using UnityEngine;
/// <summary>
/// 地图类，绘制地图
/// </summary>
public class CellMaker : MonoBehaviour
{
    public int xCount=10;
    public int yCount = 10;
    public Sprite cellSprite;
    public Color spriteColor = new Color(1, 1, 1, 1);
    private void DeleteCells()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            DestroyImmediate( transform.GetChild(0).gameObject);
        }
    }
    /// <summary>
    /// 绘制地图
    /// </summary>
    public void CreatCells()
    {
        DeleteCells();
        GameObject gb;
        SpriteRenderer sr;
        for (int i = 0; i < yCount; ++i)
        {
            for (int j = 0; j < xCount; ++j)
            {
                gb = new GameObject("Cell-" + i + "-" + j);
                gb.transform.SetParent(transform);
                gb.transform.position =transform.position+ new Vector3(j, i);
                sr = gb.AddComponent<SpriteRenderer>();
                sr.sprite = cellSprite;
                sr.color = spriteColor;
            }
        }
    }

    //获取最大Pos
    public Vector3 GetMaxPos()
    {
        return transform.position + new Vector3(xCount-1, yCount-1);
    }
}

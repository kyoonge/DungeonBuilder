using Model;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Tiles : MonoBehaviour {

    public static Tiles Instance;
    public int width = 10;
    public int length = 10;
    public Tile[,] tiles;
    private int autoIncrement = 1;

    public Sprite tileSprite;
    public Sprite trackSprite;
    private Sprite curTile;
    public int curX, curY;
    public int firstX, firstY;
    public bool isDrag =false;
    public List<Vector2> dragTiles = new List<Vector2>();
    [SerializeField]private bool isTrackTileMode = false;
    public List<Vector2> pastPointsInRectangle = new List<Vector2>();

    private Vector3 dragStartPos;
    private Vector3 dragEndPos;

    private int countTrackTile = 0;

    void Awake()
    {
        Instance = this;
    }
        void Start()
    {
        tiles = new Tile[width, length];

        // generate a grid.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                tiles[x,y] = GenerateTile(x, y);
            }
        }

        Debug.Log(tiles);
    }



    private Tile GenerateTile(int x, int y)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { new Vector3(-2, 0, 0), new Vector3(0, 1, 0), new Vector3(2, 0, 0), new Vector3(0, -1, 0) };
        mesh.triangles = new int[] { 1, 2, 3, 0, 1, 3 };

        GameObject obj = new GameObject("Tile"+ autoIncrement++);
        obj.AddComponent<MeshFilter>().mesh = mesh;
        obj.AddComponent<MeshCollider>();
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = new Vector3(((y - x) * 2), (x + y), 0);
        obj.layer = LayerMask.NameToLayer("Tile");
        var tile = obj.AddComponent<Tile>();
        tile.Set(x, y);

        GameObject visualObject = new GameObject("Visual");
        visualObject.transform.SetParent(obj.transform);
        visualObject.transform.position = tile.transform.position;
        SpriteRenderer spriteRenderer = visualObject.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = tileSprite;

        return tile;
    }


    public Tile GetTileByCoordinate(int x, int y)
    {
        if (x < 0 || y < 0)
            return null;
        if (x >= width || y >= length)
            return null;

        return tiles[x, y];
    }

    public Tile GetTileByPoint(Vector2 point)
    {
        int y = (int)Math.Round((point.y / 2f) + (point.x / 4f));
        int x = (int)Math.Round((point.y / 2f) - (point.x / 4f));
        
        if (x < 0 || x >= width)
            return null;

        if (y < 0 || y >= length)
            return null;

        return tiles[x, y];
    }

    public void DragTileStart()
    {
        Debug.Log("DragTileStart()");

        firstX = curX;
        firstY = curY;

        isDrag = true; // 호버링 색상 변경 중단
        //리스트에 넣기
        countTrackTile = 0;
    }

    public void DragTile()
    {
        //Debug.Log("DragTile() "+curX+", "+curY);
        //Tile nowTile = tiles[curX,curY];
        //nowTile.Visual.GetComponent<SpriteRenderer>().color = Color.red;
        ColorTilesInRectangle(new Vector2(firstX,firstY), new Vector2(curX,curY), Color.green);
    }

    public void DragTileEnd()
    {
        Debug.Log("DragTileEnd()");
        isDrag = false;

        foreach (Vector2 point in pastPointsInRectangle)
        {
            Tile tile = tiles[(int)point.x, (int)point.y];
            if (tile != null)
            {
                tile.Visual.GetComponent<SpriteRenderer>().color = Color.white;
                tile.Visual.GetComponent<SpriteRenderer>().sprite = curTile;

                //길 타일 표시
                if(curTile == trackSprite)
                {
                    tile.isTrack = true;
                    countTrackTile += 0;
                }
                else
                {
                    tile.isTrack = false;
                }
            }
        }

    }

    public void UndoTile()
    {
        Debug.Log("UndoTile()");
    }

    public void SetTileType(bool _isTrack)
    {
        isTrackTileMode = _isTrack;
        curTile = isTrackTileMode ? trackSprite : tileSprite;
    }

    private void CheckTileType()
    {
    }

    private void ChangeTile()
    {

    }

    public void ColorTilesInRectangle(Vector2 start, Vector2 end, Color color)
    {
        List<Vector2> pointsInRectangle = GetPointsInRectangle(start, end);

        // 이전에 색상이 변경된 좌표와 비교하여 색상을 원래 색으로 되돌림
        foreach (Vector2 point in pastPointsInRectangle)
        {
            if (!pointsInRectangle.Contains(point))
            {
                Tile tile = tiles[(int)point.x, (int)point.y];
                if (tile != null)
                {
                    tile.Visual.GetComponent<SpriteRenderer>().color = Color.white; // 원래 색상
                }
            }
        }

        foreach (Vector2 point in pointsInRectangle)
        {
            Tile tile = tiles[(int)point.x,(int)point.y];
            if (tile != null)
            {
                tile.Visual.GetComponent<SpriteRenderer>().color = color;
            }
        }

        pastPointsInRectangle.Clear();
        pastPointsInRectangle = pointsInRectangle;
    }

    private List<Vector2> GetPointsInRectangle(Vector2 start, Vector2 end)
    {
        List<Vector2> points = new List<Vector2>();

        float minX = Mathf.Min(start.x, end.x);
        float maxX = Mathf.Max(start.x, end.x);
        float minY = Mathf.Min(start.y, end.y);
        float maxY = Mathf.Max(start.y, end.y);

        for (float x = minX; x <= maxX; x++)
        {
            for (float y = minY; y <= maxY; y++)
            {
                points.Add(new Vector2(x, y));
            }
        }

        return points;
    }
}

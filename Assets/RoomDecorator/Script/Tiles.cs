using Model;
using System;
using UnityEngine;

public class Tiles : MonoBehaviour {

    public int width = 10;
    public int length = 10;
    private Tile[,] tiles;
    private int autoIncrement = 1;

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

        var tile = obj.AddComponent<Tile>();
        tile.Set(x, y);

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
}

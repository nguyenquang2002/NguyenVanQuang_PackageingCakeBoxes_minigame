using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; set; }

    public TileCell[] cells { get; set;}

    public int size => cells.Length;

    public int height => rows.Length;

    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        for(int y = 0; y < rows.Length; y++)
        {
            for(int x = 0 ; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].address = new Vector2Int(x, y);
            }
        }
    }

    public TileCell GetRandomCell()
    {
        int index = Random.Range(0, cells.Length);
        
        while (cells[index].occupied)
        {
            index++;
            if(index >= cells.Length)
            {
                index = 0;
            }
            
        }
        return cells[index];
    }

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int address = cell.address;
        address.x += direction.x;
        address.y += direction.y;
        return GetCell(address.x, address.y);
    }
}

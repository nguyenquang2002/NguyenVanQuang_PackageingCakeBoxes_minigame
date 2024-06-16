using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public TileController cake, gift;

    private TileGrid grid;

    private Vector2 startTouchPosition;

    private Vector2 endTouchPosition;

    private float minSwipeDistance = 20f;

    private bool waiting = false;

    public Text timerText;

    public float timerLeft = 45f;

    private bool gameEnd = false;

    private bool result = false;

    public GameObject menuWin, menuFalse, star2, star3;

    public Sprite starWinSprite;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cake.Spawn(cake.startCell);
        gift.Spawn(gift.startCell);
        timerText.text = timerLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)
        {
            timerLeft -= Time.deltaTime;
            if(timerLeft >= 0)
            {
                UpdateTimerText();
            }
            
            if (!waiting)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    MoveTiles(Vector2Int.up, 0, 1, grid.height - 2, -1);
                    WinGame(true);
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    MoveTiles(Vector2Int.down, 0, 1, 1, 1);
                    WinGame(false);
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    MoveTiles(Vector2Int.left, 1, 1, 0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
                }

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            startTouchPosition = touch.position;
                            break;

                        case TouchPhase.Ended:
                            endTouchPosition = touch.position;
                            Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                            if (swipeDirection.magnitude >= minSwipeDistance)
                            {
                                swipeDirection.Normalize();

                                if (swipeDirection.y > 0 && Mathf.Abs(swipeDirection.x) < Mathf.Abs(swipeDirection.y))
                                {
                                    // Lướt lên
                                    MoveTiles(Vector2Int.down, 0, 1, 1, 1);
                                    WinGame(false);
                                }
                                else if (swipeDirection.y < 0 && Mathf.Abs(swipeDirection.x) < Mathf.Abs(swipeDirection.y))
                                {
                                    // Lướt xuống
                                    MoveTiles(Vector2Int.up, 0, 1, grid.height - 2, -1);
                                    WinGame(true);
                                }
                                else if (swipeDirection.x < 0 && Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                                {
                                    // Lướt trái
                                    MoveTiles(Vector2Int.left, 1, 1, 0, 1);
                                }
                                else if (swipeDirection.x > 0 && Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                                {
                                    // Lướt phải
                                    MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
                                }
                            }
                            break;
                    }
                }
            }
        }
        if(timerLeft <= 0)
        {
            result = false;
            StartCoroutine(WaitForEnd());
        }
        
    }
    void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(timerLeft);
        int milliseconds = Mathf.FloorToInt((timerLeft - seconds) * 100);
        timerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }
    private void CreateCake()
    {
        Instantiate(cake, grid.transform);
        TileCell newcell = grid.GetRandomCell();
        cake.transform.position = new Vector2((newcell.address.x - 1) * 320, (1 - newcell.address.y) * 320);
        
        Debug.Log(cake.cell.address);
    }

    private void MoveTiles(Vector2Int direction, int startX, int increX, int startY, int increY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < grid.width; x += increX)
        {
            for(int y = startY; y >= 0 && y < grid.height; y += increY)
            {
                TileCell cell = grid.GetCell(x, y);

                if(cell.occupied)
                {
                    changed = MoveTile(cell.tileController, direction);
                }
            }
        }
        if (changed)
        {
            StartCoroutine(WaitForMove());
        }
    }

    private bool MoveTile(TileController tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);
        while (adjacent != null)
        {
            if (adjacent.occupied || adjacent.candy)
            {
                break;
            }
            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
            
        }
        return false;
    }
    private void WinGame(bool down)
    {
        if(cake.cell.address.y < gift.cell.address.y && cake.cell.address.x == gift.cell.address.x)
        {
            bool canWin = false;
            for(int y = cake.cell.address.y + 1; y <= gift.cell.address.y; y++)
            {
                if(grid.GetCell(cake.cell.address.x, y).candy)
                {
                    break;
                }
                else
                {
                    canWin = true;
                }
            }
            if(canWin)
            {
                if (down)
                {
                    cake.MoveTo(gift.cell);
                }
                else
                {
                    gift.MoveTo(cake.cell);
                }
                result = true;
                StartCoroutine(WaitForEnd());
                
            }
        }
    }
    
    private void Result()
    {
        if (result)
        {
            
            if(timerLeft >= 15f)
            {
                Image star2Image = star2.GetComponent<Image>();
                star2Image.sprite = starWinSprite;
            }
            if (timerLeft >= 30f)
            {
                Image star3Image = star3.GetComponent<Image>();
                star3Image.sprite = starWinSprite;
            }
            menuWin.SetActive(true);
        }
        else
        {
            menuFalse.SetActive(true);
        }
    }
    private IEnumerator WaitForMove()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;
    }
    private IEnumerator WaitForEnd()
    {
        
        gameEnd = true;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0;
        Result();
    }
}

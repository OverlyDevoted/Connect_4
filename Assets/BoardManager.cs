using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BoardManager : MonoBehaviour
{
    public List<GameObject> columns;
    private Vector2 leftCorner;
    private Vector2 rightCorner;
   [SerializeField] GameObject circle;
    public EventHandler OnGameEnd;
    int filledColumns;
    // Start is called before the first frame update
    void Start()
    {
        leftCorner = columns[0].transform.position;
        rightCorner = columns[columns.Count - 1].transform.position;
        foreach(GameObject column in columns)
        {
            column.GetComponent<ColumnManager>().OnFilled += CheckIfEnd;
        }
        //Debug.Log(leftCorner + " " + rightCorner);
    }
    void CheckIfEnd(object obj, EventArgs e)
    {
        filledColumns++;
        
        if (filledColumns == columns.Count)
        {
            Debug.Log("Nobody won");
            OnGameEnd?.Invoke(this, EventArgs.Empty);
        }
    }
    public Vector2 GetLeftCorner()
    {
        return leftCorner;
    }

    public Vector2 GetRightCorner()
    {
        return rightCorner;
    }

    public void PlaceObject()
    {
        //Debug.Log("Should place " + circle.name+"in the board column " + (circle.transform.position.x + 3));
        int columnIndex = (int)circle.transform.position.x + 3;
        int rowIndex = columns[columnIndex].GetComponent<ColumnManager>().PlaceCircle(circle, GetPlayer());
        Vector2 start = new Vector2(columnIndex, rowIndex);
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                Vector2 direction = new Vector2(i, j);
                CheckForWin(start, direction, GetPlayer(), 0);
            }
        }

        
    }

    void CheckForWin(Vector2 start, Vector2 direction, SlotManager.Player player, int iteration)
    {
        
        if (iteration == 3)
        {
            OnGameEnd?.Invoke(this, EventArgs.Empty);
            return;
        }
        start += direction;
        if (start.x < 0 || start.x > 6)
        {
            return;
        }
        if (start.y < 0 || start.y > 5)
        {
            return;
        }
        //Debug.ClearDeveloperConsole();
        //Debug.Log(start + " " +player + " " + iteration);
        if (GetOccupantAtPos((int)start.x, (int)start.y) != player)
            return;
        iteration++;
        CheckForWin(start, direction, player, iteration);
    }
    public SlotManager.Player GetPlayer()
    {
        if(circle.CompareTag("Red"))
        {
            return SlotManager.Player.Red;
        }
        return SlotManager.Player.Yellow;
    }
    public void SetCircle(GameObject newCircle)
    {
        circle = newCircle;
    }
    public bool CanPlaceInColumn(int index)
    {
        return columns[index].GetComponent<ColumnManager>().CanPlaceCircle();    
    }
    public SlotManager.Player GetOccupantAtPos(int x, int y)
    {
        return columns[x].GetComponent<ColumnManager>().GetSlot(y).GetComponent<SlotManager>().GetOccupator();
    }
    public void EmptySlots()
    {
        foreach(GameObject column in columns)
        {
            column.GetComponent<ColumnManager>().EmptySlots();
        }
    }
}



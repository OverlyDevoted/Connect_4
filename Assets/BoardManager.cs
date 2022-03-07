using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class BoardManager : MonoBehaviour
{
    public Test thing;
    public List<ColumnManager> columns;
    private Vector2 leftCorner;
    private Vector2 rightCorner;
   [SerializeField] GameObject circle;
    public EventHandler OnGameEnd;
    int filledColumns;
    [SerializeField] PlayerLogic player;
    public Color firstP;
    public Color secondP;
    // Start is called before the first frame update
    void Start()
    {
        leftCorner = columns[0].transform.position;
        rightCorner = columns[columns.Count - 1].transform.position;
        foreach(ColumnManager column in columns)
        {
            column.OnFilled += CheckIfEnd;
        }
        //EmptySlots();
        //StartCoroutine(Clear());
        //Debug.Log(leftCorner + " " + rightCorner);
    }
    IEnumerator Clear()
    {
        yield return new WaitForSeconds(1f);
        EmptySlots(); 
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

    public void PlaceObject(int columnIndex)
    {
        //Debug.Log("Should place " + circle.name+"in the board column " + (circle.transform.position.x + 3));
        Color colorToUse;
        if (player.isMyTurn())
        {
            Debug.Log("My turn place");
            colorToUse = firstP;
        }
        else
        {
            Debug.Log("Not my turn");
            colorToUse = secondP;
            
        }
            
        int rowIndex = columns[columnIndex].PlaceCircle(GetPlayer(), colorToUse);
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
        if (player.isMyTurn())
            return SlotManager.Player.Red;
        return SlotManager.Player.Yellow;
    }
    public void SetCircle(GameObject newCircle)
    {
        circle = newCircle;
    }
    public bool CanPlaceInColumn(int index)
    {
        return columns[index].CanPlaceCircle();    
    }
    public SlotManager.Player GetOccupantAtPos(int x, int y)
    {
        return columns[x].GetSlot(y).GetOccupator();
    }
    public void EmptySlots()
    {
        int columnsLenght = columns.Count;
        for (int i=0;i<columnsLenght;i++)
        {
            columns[i].EmptySlots();
        }
    }
}



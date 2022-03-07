using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerLogic : MonoBehaviour
{
    public enum States
    {
        MyTurn,
        NotMyTurn
    }
    public States state;
    InputController input;
    BoardManager board;
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;

    public GameObject circle;
    public delegate void EventDelegate();
    public delegate void EventDelegateInt(int i);
    public EventDelegate onTurnEnd;
    public EventDelegateInt onTurnEndInt;
    bool isEnd;
    private void Awake()
    {
        state = States.NotMyTurn;
    }
    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Board").GetComponent<BoardManager>();
        board.OnGameEnd += EndGame;
        input = GetComponentInChildren<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case States.MyTurn:
                circle.transform.position = CalculateMouse();
                int currentIndex = (int)circle.transform.position.x + 3;
                if(board.CanPlaceInColumn(currentIndex))
                {
                    if (input.OnClickedLeftClick == null)
                    {
                        input.OnClickedLeftClick += PassTurn;
                    }
                    return;
                }
                input.OnClickedLeftClick = null;
                break;
            case States.NotMyTurn:
                break;
        }
    }
    Vector2 CalculateMouse()
    {
        Vector2 current = input.GetMousePos();
        Vector2 newMouse = new Vector2(Mathf.Round(Mathf.Clamp(current.x, board.GetLeftCorner().x, board.GetRightCorner().x)), 1.6f);
        return newMouse;
    }

    public void StartTurn()
    {
        if (isEnd)
            return;
        SpawnCircle();
        board.SetCircle(circle);
        state = States.MyTurn;
    }
    private void SpawnCircle()
    {
        circle = Instantiate(playerOnePrefab, CalculateMouse(), Quaternion.identity);
    }
    private void DespawnCircle()
    {
        if(circle!=null)
            Destroy(circle);
    }
    private void PassTurn(object obj, EventArgs e)
    {
        int placeX = (int)circle.transform.position.x + 3;
        Debug.Log(onTurnEndInt);
        onTurnEndInt(placeX);
        DespawnCircle();
        state = States.NotMyTurn;
        input.OnClickedLeftClick = null;
    }
    private void EndGame(object obj, EventArgs e)
    {
        isEnd = true;
       // onTurnEnd = null;
        onTurnEndInt = null;
        input.OnClickedLeftClick = null;
        DespawnCircle();
        state = States.NotMyTurn;
    }
    public void AllowToMove()
    {
        isEnd = false;
    }
    public bool isMyTurn()
    {
        if (state == States.MyTurn)
            return true;
        return false;
    }
}

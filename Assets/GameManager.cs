using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public enum ScreenState
    {
        GAME,
        INITIAL,
        SERVER,
        J_LOBBY,
        LOBBY,
        ACTIVE_GAME
    }
    public GameObject initialScreen;
    public GameObject serverScreen;
    public GameObject joinLobbyScreen;
    public GameObject lobbyScreen;
    public GameObject uiCanvas;

    private GameObject[] players;
    public BoardManager board;
    public WebSocketReceiver webSocket;

    public EventHandler OnTurnEnd;
    public EventHandler OnGameStart;

    [SerializeField] GameObject winParticles;
    [SerializeField] Material redMaterial;
    [SerializeField] Material yellowMaterial;
    private ScreenState screenState;

    public int playerNumber;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        webSocket.OnConnect += SuccessfullyConnected;
        webSocket.OnJoin += ChangeScreen;
        webSocket.OnLostConnection += LostConnection;
        webSocket.onStartGame += DisableUI;
        webSocket.onStartGame += GetPlayerNumber;
        webSocket.onStartGame += StartGame;
    }
    private void Start()
    {
        board.OnGameEnd += EndGame;
        OnGameStart += players[1].GetComponent<PlayerLogic>().AllowToMove;
        OnGameStart += players[0].GetComponent<PlayerLogic>().AllowToMove;
        OnGameStart += players[1].GetComponent<PlayerLogic>().StartTurn;
        screenState = ScreenState.INITIAL;
    }

    private void Update()
    {
        switch (screenState) {
            case ScreenState.INITIAL:
                if (initialScreen.activeInHierarchy == false)
                {
                    initialScreen.SetActive(true);
                    serverScreen.SetActive(false);
                    lobbyScreen.SetActive(false);
                    joinLobbyScreen.SetActive(false);   
                }
                break;

            case ScreenState.GAME:
                if (lobbyScreen.activeInHierarchy == true)
                {
                    initialScreen.SetActive(false);
                    serverScreen.SetActive(false);
                    lobbyScreen.SetActive(false);
                    joinLobbyScreen.SetActive(false);
                }
                break;

            case ScreenState.J_LOBBY:
                if (joinLobbyScreen.activeInHierarchy == false)
                {
                    initialScreen.SetActive(false);
                    serverScreen.SetActive(false);
                    lobbyScreen.SetActive(false);
                    joinLobbyScreen.SetActive(true);
                }
                break;

            case ScreenState.LOBBY:
                if (lobbyScreen.activeInHierarchy == false)
                {
                    initialScreen.SetActive(false);
                    serverScreen.SetActive(false);
                    lobbyScreen.SetActive(true);
                    joinLobbyScreen.SetActive(false);
                }
                break;

            case ScreenState.SERVER:
                if (serverScreen.activeInHierarchy == false)
                {
                    initialScreen.SetActive(false);
                    serverScreen.SetActive(true);
                    lobbyScreen.SetActive(false);
                    joinLobbyScreen.SetActive(false);
                }
                break;
            case ScreenState.ACTIVE_GAME:
                uiCanvas.SetActive(false);
                break;
        }

    }

    private void EndGame(object obj, EventArgs e)
    {
        SlotManager.Player player = board.GetPlayer();
        Debug.Log("Player " + player + " won");
        winParticles.SetActive(true);
        
        if (player == SlotManager.Player.Red)
        {
            winParticles.GetComponent<ParticleSystemRenderer>().material = redMaterial;
            return;
        }
        winParticles.GetComponent<ParticleSystemRenderer>().material = yellowMaterial;
    }

    public void StartGame()
    {
        board.EmptySlots();
        players[0].GetComponent<PlayerLogic>().OnTurnEnd += TurnEnds;
        players[1].GetComponent<PlayerLogic>().OnTurnEnd += TurnEnds;
        players[0].GetComponent<PlayerLogic>().OnTurnEnd += players[1].GetComponent<PlayerLogic>().StartTurn;
        players[1].GetComponent<PlayerLogic>().OnTurnEnd += players[0].GetComponent<PlayerLogic>().StartTurn;
        OnGameStart?.Invoke(this, EventArgs.Empty);
        winParticles.SetActive(false);
        //board.EmptySlots();
    }
    
    private void TurnEnds(object obj, EventArgs e)
    {
        board.PlaceObject();
        OnTurnEnd?.Invoke(this, EventArgs.Empty);
    }
    public void GoLobby()
    {
        screenState = ScreenState.LOBBY;       
    }
    public void GoServer()
    {
        screenState = ScreenState.SERVER;
    }
    public void GoJoinLobby()
    {
        screenState = ScreenState.J_LOBBY;
    }
    public void GoGame()
    {
        screenState = ScreenState.GAME;
    }
    public void GoInitial()
    {
        screenState = ScreenState.INITIAL;
    }
    private void SuccessfullyConnected(object obj, EventArgs e)
    {
        GoServer();
    }
    private void ChangeScreen(object obj, EventArgs e)
    {
        GoLobby();
    }
    private void LostConnection(object obj, EventArgs e)
    {
        GoInitial();
    }
    private void DisableUI()
    {
        screenState = ScreenState.ACTIVE_GAME;
    }
    private void GetPlayerNumber()
    {
        playerNumber = webSocket.GetPrio();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using TMPro;

public class WebSocketReceiver : MonoBehaviour
{
    public enum WebState
    {
        TYPE,
        CLASS
    }
    WebSocket ws;
    public delegate void OnStartGame();

    public WebState webState;
    public string methodType;
    public Client client;
    public Game lobby;
    public EventHandler OnConnect;
    public EventHandler OnJoin;
    public EventHandler OnLostConnection;
    public OnStartGame onStartGame;
    public TextMeshProUGUI log;
    public TMP_InputField inputField;
    public TMP_InputField serverCode;
    public TextMeshProUGUI notification;
    private void Start()
    {
        log.text = "";
        webState = WebState.TYPE;
    }

    
    public void ConnectServer()
    {
        if (ws != null)
            ws.Close();
        ws = new WebSocket("ws://127.0.01:9090");
        ws.Connect();
        ws.OnClose += (sender, e) =>
        {
            client = new Client();
            lobby = new Game();
            OnLostConnection?.Invoke(this, EventArgs.Empty);
            Debug.Log("Lost connection");
        };
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log(e.Data);
            /*if (e.Data == "__ping__")
            {
                ws.SendAsync("{" +
                    "\"method\":\"pong\","+
                    "\"userId\":\""+ client.guid +"\"}", null);
                
                return;
            }*/
            switch (webState) {
                case WebState.TYPE:
                    Response response = JsonUtility.FromJson<Response>(e.Data);
                    switch (response.method)
                    {
                        case "connect":
                            methodType = response.method;
                            break;
                        case "create":
                            methodType = response.method;
                            break;
                        case "join":
                            methodType = response.method;
                            break;
                        case "noGame":
                            methodType = response.method;
                            break;
                        case "update":
                            methodType = response.method;
                            break;
                        case "startgame":
                            Debug.Log("Wrok2");
                            methodType = response.method;
                            break;
                    }
                    Debug.Log("to class");
                    webState = WebState.CLASS;
                    break;
                case WebState.CLASS:
                    switch (methodType)
                    {
                        case "connect":
                            client = JsonUtility.FromJson<Client>(e.Data);
                            Debug.Log(client.guid);
                            break;
                        case "create":
                            Game game = JsonUtility.FromJson<Game>(e.Data);
                            lobby = game;
                            Debug.Log("E");
                            JoinGame();
                            break;
                        case "join":
                            lobby = JsonUtility.FromJson<Game>(e.Data);
                            OnJoin?.Invoke(this, EventArgs.Empty);
                            break;
                        case "update":
                            Client disconnected = JsonUtility.FromJson<Client>(e.Data);
                            log.text += disconnected.guid + " has disconnected";
                            break;
                        case "noGame":
                            lobby = new Game();
                            ReasonMessage noGameReason = JsonUtility.FromJson<ReasonMessage>(e.Data);
                            //notification.text = noGameReason.reason;
                            StartCoroutine(ShowNotification(noGameReason.reason));
                            break;
                        case "startgame":
                            Debug.Log("WORK");
                            onStartGame();
                            Debug.Log("DO THEHING");
                            ReasonMessage gameReason = JsonUtility.FromJson<ReasonMessage>(e.Data);
                            StartCoroutine(ShowNotification(gameReason.reason));
                            break;
                    }
                    Debug.Log("to type");
                    webState = WebState.TYPE;
                    methodType = null;
                    break;

            }     
        };

        if (ws.IsAlive)
        {
            OnConnect?.Invoke(this, EventArgs.Empty);
            return;
        }
        Debug.Log("Service is down");
    }
    IEnumerator ShowNotification(string message)
    {
        notification.text = message;
        Debug.Log(message);
        yield return new WaitForSeconds(10f);
        Debug.Log(message);
        notification.text = "";
    }
    public void CreateGame()
    {
        ws.Send("{" +
            "\"method\":\"create\"," +
            "\"clientId\":\"" + client.guid +
            "\"" +
            "}");
    }
    public void JoinGame()
    {
        webState = WebState.TYPE;
        ws.Send("{" +
            "\"method\":\"join\"," +
            "\"clientId\":\"" + client.guid +
            "\",\"guid\":\"" + lobby.guid +
            "\""+
            "}");
    }
    public void JoinGameThroughButton()
    {
        webState = WebState.TYPE;
        ws.Send("{" +
            "\"method\":\"join\"," +
            "\"clientId\":\"" + client.guid +
            "\",\"guid\":\"" + inputField.text +
            "\"" +
            "}");

    }
    public void Send()
    {
        ws.Send("{" +
            "\"method\":\"send\"," +
            "\"clientId\":\"" + client.guid+
            "\"}"
            ); 
    }
    public void LeaveLobby()
    {
        ws.Send("{" +
            "\"method\":\"leaveGame\"," +
            "\"clientId\":\"" + client.guid +
            "\"}"
        );
    }
    public void Ready()
    {
        ws.Send("{" +
            "\"method\":\"ready\"," +
            "\"clientId\":\"" + client.guid +
            "\"}"
        );
    }
    public int GetPrio()
    {
        return client.prio;
    }
    [Serializable]
    public class ReasonMessage {
        public string reason;
        public ReasonMessage(string reason)
        {
            this.reason = reason;
        }
    }

    [Serializable]
    public class Response {
        public string method;
        public Response(string method)
        {
            this.method = method;
        }

    }
    [Serializable]
    public class Client {
        public string guid;
        public int prio;
        public string ready;
        public Client (string guid)
        {
            this.guid = guid;
        }
        public Client(string guid, int prio, string ready)
        {
            this.guid = guid;
            this.prio = prio;
            this.ready = ready;
        }
        public Client()
        {
        }
    }
    [Serializable]
    public class Game
    {
        public string guid;
        public Client[] clients;
        public Game(string guid)
        {
            this.guid = guid;
        }
        public Game(string guid, Client[]clients)
        {
            this.guid = guid;
            this.clients = clients;
        }
        public Game()
        {

        }
    }


}

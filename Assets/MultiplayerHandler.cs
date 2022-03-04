using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerHandler : MonoBehaviour
{
    public bool isPrint = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if(isPrint)
            GetData();
        else
            Debug.Log("Message");
    }
    public void GetData()
    {
        StartCoroutine(GetData_Coroutine());
    }
    IEnumerator GetData_Coroutine()
    {
        string uri = "http://127.0.0.1:8000/learn/unity";
        using(UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}

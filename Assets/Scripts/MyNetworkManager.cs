using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : MonoBehaviour
{
    public bool isServer = false;
    public bool isAtStartup = false;
    NetworkClient myClient;
    private Scores scores;

    void Start()
    {
        Application.runInBackground = true;
        scores = gameObject.GetComponent<Scores>();  

        if (isServer)
        {
            scores.SetupServer();
        }
        else
        {
            StartCoroutine(SendScoreMessage());
        }           
        
        scores.SetupClient();
    }

    IEnumerator SendScoreMessage()
    {
        yield return new WaitForSeconds(1.0f);
        scores.SendScore(10, new Vector3(1, 1, 1), 5);
        StartCoroutine(SendScoreMessage());
    }

    void Update()
    {
        
    }
    
}
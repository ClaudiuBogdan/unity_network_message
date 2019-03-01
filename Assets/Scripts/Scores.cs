using UnityEngine;
using UnityEngine.Networking;

public class Scores : MonoBehaviour
{
    NetworkClient myClient;

    public class MyMsgType
    {
        public static short Score = MsgType.Highest + 1;
    };

    public class ScoreMessage : MessageBase
    {
        public int score;
        public Vector3 scorePos;
        public int lives;
    }

    public void SendScore(int score, Vector3 scorePos, int lives)
    {
        ScoreMessage msg = new ScoreMessage();
        msg.score = score;
        msg.scorePos = scorePos;
        msg.lives = lives;

        Debug.Log("Sending score...");
        myClient.Send(MyMsgType.Score, msg);
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MyMsgType.Score, OnScore);
        myClient.Connect("127.0.0.1", 4444);
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.RegisterHandler(MyMsgType.Score, onMessageReceived);
        NetworkServer.Listen(4444);
    }

    public void onMessageReceived(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        NetworkServer.SendToAll(MyMsgType.Score, msg);
    }

    public void OnScore(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        Debug.Log("OnScoreMessage " + msg.score);
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}
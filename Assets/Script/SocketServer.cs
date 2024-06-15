using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SocketServer : MonoBehaviour
{
    private TcpListener server;
    private bool isRunning;
    private int Port = 11000;
    private string message;
    private static string tMsg;
    
    private static bool isConnected;

    private static SocketServer instance;

    private bool isAcceptingClients;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(ipAddress, Port);
            StartCoroutine(StartServer());
            tMsg = "noPose";
            isRunning = true;
            
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private IEnumerator StartServer()
    {
        while (true)
        {
            try
            {
                server.Start();
                isAcceptingClients = true;
                yield break; // 成功後退出協程
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    Debug.LogWarning("Address already in use, retrying in 1 second...");
                }
                else
                {
                    Debug.LogError($"SocketException: {ex.Message}");
                    yield break; // 如果是其他異常則退出協程
                }
            }
            yield return new WaitForSeconds(1); // 每隔 1 秒重試一次
        }
    }

    private void FixedUpdate()
    {
        if (server != null && isAcceptingClients)
        {
            server.BeginAcceptTcpClient(HandleClientAccepted, server);
        }
        video.Move = tMsg;
    }


    private void HandleClientAccepted(IAsyncResult result)
    {
        TcpClient client = server.EndAcceptTcpClient(result);
        NetworkStream stream = client.GetStream();

        while (isRunning)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            byte[] response = Encoding.ASCII.GetBytes("Server response : " + message);
            stream.Write(response, 0, response.Length);
            if (message != "")
            {
                isConnected = true;
                tMsg = message;
            }
            else
            {
                isConnected = false;
            }
        }
    }
    
    private void OnApplicationQuit()
    {
        StopServer();
    }

    public static bool IsConnected()
    {
        return isConnected;
    }

    public static bool IsCaptureCamera()
    {
        return tMsg != "NotCaptureCamera";
    }

    private void StopServer()
    {
        isRunning = false;
        if (server != null)
        {
            server.Stop();
        }
    }
}
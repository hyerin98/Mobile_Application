using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;
using TMPro;

public class Client : MonoBehaviour
{
    public TMP_InputField IPInput, PortInput;
    string clientName;

    bool socketReady;
    TcpClient socket;
    NetworkStream stream;
    StreamWriter writer;
    StreamReader reader;

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button ConnectedButton;

    void Start()
    {
        Button1.onClick.AddListener(() => SendVideoCommand("VIDEO1"));
        Button2.onClick.AddListener(() => SendVideoCommand("VIDEO2"));
        Button3.onClick.AddListener(() => SendVideoCommand("VIDEO3"));
    }

    public void ConnectToServer()
    {
        if (socketReady) return;

        string ip = IPInput.text == "" ? "127.0.0.1" : IPInput.text;
        int port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);

        try
        {
            socket = new TcpClient(ip, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;

            // 연결에 성공하면 안보이게하기
            IPInput.gameObject.SetActive(false);
            PortInput.gameObject.SetActive(false);
            ConnectedButton.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log($"소켓에러 : {e.Message}");
        }
    }

    void Update()
    {
        if (socketReady && stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if (data != null)
                OnIncomingData(data);
        }
    }

    void OnIncomingData(string data)
    {
        if (data == "%NAME")
        {
            Send($"&NAME|{clientName}");
            return;
        }

        Debug.Log(data);
    }

    void Send(string data)
    {
        if (!socketReady) return;

        writer.WriteLine(data);
        writer.Flush();
    }

    void SendVideoCommand(string command)
    {
        Send(command);
    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void CloseSocket()
    {
        if (!socketReady) return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}

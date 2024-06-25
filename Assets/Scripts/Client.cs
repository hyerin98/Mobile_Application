using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;
using TMPro;
using DG.Tweening;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public TMP_InputField IPInput, PortInput;
    string clientName = "Mobile";

    bool socketReady;
    TcpClient socket;
    NetworkStream stream;
    StreamWriter writer;
    StreamReader reader;

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button ConnectedButton;
    public Button ExitButton;
    public bool isPressed = false;
    [SerializeField] CanvasGroup[] textCGs;
    [SerializeField] int slideYAmount = 40; // y축으로 슬라이드 되는 값
    [SerializeField] float slideDuration = 1; // 슬라이드 모션 시간
    [SerializeField] float waitDelayToHide = 0; // 텍스트가 사라지기 전까지 대기 시간

    Sequence textSequence;
    void Awake()
    {
        textSequence = DOTween.Sequence().SetAutoKill(false);
        for (int i = 0; i < textCGs.Length; i++)
        {
            Transform textT = textCGs[i].transform;
            float hideDuration = slideDuration * .1f;
            float startTime = i * (slideDuration + waitDelayToHide + hideDuration);

            textSequence.Insert(startTime, textCGs[i].DOFade(1, slideDuration).From(0, true).SetEase(Ease.OutQuart))
            .Join(textT.DOLocalMoveY(0, slideDuration).From(-slideYAmount, true, true).SetEase(Ease.OutQuart));
            // .AppendInterval(waitDelayToHide)
            // .Append(textCGs[i].DOFade(0, hideDuration).SetEase(Ease.OutQuart))
            // .Join(textT.DOLocalMoveY(slideYAmount, hideDuration).SetRelative().SetEase(Ease.OutQuart));
        }
        textSequence.Play(); // 시퀀스 실행
    }

    void OnDestroy()
    {
        textSequence.Kill();
    }

    void Start()
    {
        {
            Button1.onClick.AddListener(() =>
            {
                SendVideoCommand("VIDEO1");
                NotPressedButton();
                Invoke("CanPressedButton", 8f);
            });
            Button2.onClick.AddListener(() =>
            {
                SendVideoCommand("VIDEO2");
                NotPressedButton();
                Invoke("CanPressedButton", 8f);
            });
            Button3.onClick.AddListener(() =>
            {
                SendVideoCommand("VIDEO3");
                NotPressedButton();
                Invoke("CanPressedButton", 8f);
            });
            ExitButton.onClick.AddListener(() => 
            {
                SendVideoCommand("EXIT4");
                CanPressedButton();
            });
        }
    }

    void NotPressedButton()
    {
        Button1.interactable = false;
        Button2.interactable = false;
        Button3.interactable= false;
    }

    void CanPressedButton()
    {
        Button1.interactable = true;
        Button2.interactable = true;
        Button3.interactable= true;
    }

    public void ConnectToServer()
    {
        if (socketReady) return;
        
        string ip = IPInput.text == "" ? "192.168.1.12" : IPInput.text;
        int port = PortInput.text == "" ? 7777 : int.Parse(PortInput.text);

        try
        {
            socket = new TcpClient(ip, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
            SceneManager.LoadScene("test 1");
            // 연결에 성공하면 안보이게하기
            // IPInput.gameObject.SetActive(false);
            // PortInput.gameObject.SetActive(false);
            // ConnectedButton.gameObject.SetActive(false);
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
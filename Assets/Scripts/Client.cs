using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;
using TMPro;
using DG.Tweening;

public class Client : MonoBehaviour
{
    public InputField IPInput, PortInput;
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
    public TextMeshProUGUI mainText;
    public bool isPressed = false;
    public bool isExit = false;
    public bool isExited = false;
    [SerializeField] CanvasGroup[] firstCGs;
    [SerializeField] CanvasGroup[] secondCGs;
    [SerializeField] int slideYAmount = 100; // y축으로 슬라이드 되는 값
    [SerializeField] float slideDuration = 0.01f; // 슬라이드 모션 시간
    [SerializeField] float waitDelayToHide = 0.01f; // 텍스트가 사라지기 전까지 대기 시간

    Sequence sequence1;
    Sequence sequence2;

    void Awake()
    {
        sequence1 = DOTween.Sequence().SetAutoKill(false);
        for (int i = 0; i < firstCGs.Length; i++)
        {
            Transform textT = firstCGs[i].transform;
            float startTime = i * (slideDuration + waitDelayToHide);

            sequence1.Insert(startTime, firstCGs[i].DOFade(1, slideDuration).From(0, true).SetEase(Ease.OutQuart))
            .Join(textT.DOLocalMoveY(0, slideDuration).From(-slideYAmount, true, true).SetEase(Ease.OutQuart));
        }
        sequence1.Play(); // 시퀀스 실행
    }

    void SecondCanvasGroup()
    {
        sequence2 = DOTween.Sequence().SetAutoKill(false);
        for (int i = 0; i < secondCGs.Length; i++)
        {
            Transform textT = secondCGs[i].transform;
            float startTime = i * (slideDuration + waitDelayToHide);

            sequence2.Insert(startTime, secondCGs[i].DOFade(1, slideDuration).From(0, true).SetEase(Ease.OutQuart))
            .Join(textT.DOLocalMoveY(0, slideDuration).From(-slideYAmount, true, true).SetEase(Ease.OutQuart));
        }
        sequence2.Play();
    }

    void OnDestroy()
    {
        sequence1.Kill();
        sequence2.Kill();
    }

    void Start()
    {
        // 버튼 이벤트 리스너 설정
        Button1.onClick.AddListener(() =>
        {
            SendVideoCommand("VIDEO2");
        });
        Button2.onClick.AddListener(() =>
        {
            SendVideoCommand("VIDEO3");
        });
        Button3.onClick.AddListener(() =>
        {
            SendVideoCommand("VIDEO4");
        });
        ExitButton.onClick.AddListener(() =>
        {
            isExit = true;
            SendVideoCommand("EXIT1");
        });
    }

    void NotPressedButton()
    {
        isPressed = false;
        Button1.interactable = false;
        Button2.interactable = false;
        Button3.interactable = false;
    }

    void CanPressedButton()
    {
        isPressed = true;
        Button1.interactable = true;
        Button2.interactable = true;
        Button3.interactable = true;
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
            // 연결에 성공하면 안보이게하기
            IPInput.interactable = false;
            PortInput.interactable = false;
            ConnectedButton.interactable = false;

            SecondCanvasGroup();
            mainText.gameObject.SetActive(true);
            Button1.gameObject.SetActive(true);
            Button2.gameObject.SetActive(true);
            Button3.gameObject.SetActive(true);

        }
        catch (Exception e)
        {
            TraceBox.Log($"소켓에러 : {e.Message}");
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
        if(Input.GetKeyDown(KeyCode.Return))
        {
            PortInput.ActivateInputField();
            ConnectToServer();
        }

        if(isPressed && isExit) // 만약 버튼이 활성화되어있고 exit버튼도 같이 눌렀다면
        {
            isExited = true; // 어플리케이션 나가지기 true
        }

        if(!isPressed && isExit) // 만약 버튼이 비활성화되어있고 exit버튼도 같이 눌렀다면
        {
            isExited = false; // 어플리케이션 나가지기 false. -> 버튼 활성화로 되어야함
        }

        if(isExited)
        {
            Application.Quit();
            OnApplicationQuit(); // 6.28 추가
        }
    }

    public void OnIncomingData(string data)
    {
        if (data == "%NAME")
        {
            Send($"&NAME|{clientName}");
            return;
        }

        if (data == "ENABLE_BUTTONS")
        {
            CanPressedButton();
        }
        if (data == "DISABLE_BUTTONS")
        {
            NotPressedButton();
        }
        if(data.StartsWith("VIDEO_FINISHED"))
        {
            CanPressedButton();
        }

        TraceBox.Log(data);
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
        Application.Quit();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using TMPro;
using UnityEngine.UI;

public class VideoPlayerController : MonoBehaviour
{
    public static VideoPlayerController instance;
    public MediaPlayer mediaPlayer;
    public TextMeshProUGUI messageText;

    void Awake()
    {
        instance = this;
    }

    public void ShowMessage(string data)
    {
        if (messageText != null)
        {
            messageText.text += messageText.text == "" ? data : "\n" + data;
        }
        Debug.Log(data);

        Invoke("DelayHideMessage", 2f);
    }

    void DelayHideMessage()
    {
        messageText.gameObject.SetActive(false);
    }

    public void PlayVideo(string videoName)
    {
        if (mediaPlayer != null)
        {
            string videoPath = $"Assets/Videos/{videoName}.mp4";

            mediaPlayer.OpenMedia(MediaPathType.RelativeToProjectFolder, videoPath, false);
            mediaPlayer.Play();
        }
        else
        {
            Debug.LogError("MediaPlayer is not assigned!");
        }
    }

    public void OnIncomingData(string data)
    {
        if (data.StartsWith("VIDEO"))
        {
            string videoName = data.Substring(5); // "VIDEO1" -> "1"
            PlayVideo(videoName);
        }
        else if (data.StartsWith("EXIT"))
        {
            string videoName = data.Substring(4);
            PlayVideo(videoName);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Video : MonoBehaviour
{
    public static Video instance;
	void Awake() => instance = this;

	public InputField SendInput;
	public RectTransform VideoContent;
	public Text VideoText;
	public ScrollRect VideoScrollRect;


	public void ShowMessage(string data)
	{
		VideoText.text += VideoText.text == "" ? data : "\n" + data;
		
		Fit(VideoText.GetComponent<RectTransform>());
		Fit(VideoContent);
		Invoke("ScrollDelay", 0.03f);
	}

	void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

	void ScrollDelay() => VideoScrollRect.verticalScrollbar.value = 0;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCanvas : MonoBehaviour
{
	Canvas canvas = null;
    CanvasScaler scaler = null;

	private void Awake()
    {
        Managers.UI.Init();
    }

    public virtual void Init()
    {
        scaler = GetComponent<CanvasScaler>();
		canvas = GetComponent<Canvas>();

		SetCanvas();
        SetCanvasScaler();
		SetResolution();
	}

	private void SetCanvas()
	{
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.worldCamera = Camera.main;
		canvas.sortingOrder = 100;
	}

    private void SetCanvasScaler()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1.0f;
    }

	public void SetResolution()
	{
		int setWidth = 1920; // 사용자 설정 너비
		int setHeight = 1080; // 사용자 설정 높이

		int deviceWidth = Screen.width; // 기기 너비 저장
		int deviceHeight = Screen.height; // 기기 높이 저장

		Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

		if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
		{
			float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
			Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
		}
		else // 게임의 해상도 비가 더 큰 경우
		{
			float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
			Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
		}

		OnPreCull();
	}

	private void OnPreCull()
	{
		GL.Clear(true, true, Color.black);
	}

	public void OnClick_Activate(GameObject g) => g.SetActive(true);
    public void OnClick_Deactivate(GameObject g) => g.SetActive(false);

    public virtual void Open<T>(UIParam param = null) { }
    public virtual void Close<T>() { }

    public virtual T GetUIComponent<T>() { return default(T); }
}


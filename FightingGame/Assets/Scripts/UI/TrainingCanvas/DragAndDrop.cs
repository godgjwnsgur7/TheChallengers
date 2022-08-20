using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Image image1;
    [SerializeField] Canvas canvas;

    public bool isUpdate = false;
    public float AlphaThreshold = 0.1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Buttom Click 가능 범위 설정
        image1 = GetComponent<Image>();
        image1.alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isUpdate)
            return;

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isUpdate)
            return;

        // 이전 이동과 비교해서 얼마나 이동했는지를 보여줌
        // 캔버스의 스케일과 맞춰야 하기 때문에
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isUpdate)
            return;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] GameObject parent;
    RectTransform parentRectTransform;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Image image1;

    Vector2 currentPosition;
    Vector2 beforeVec;

    float parentHalfWidth;
    float parentHalfHeight;
    float halfWidth;
    float halfHeight;
    float xRange;
    float yRange;

    public bool isUpdate = false;
    public float AlphaThreshold = 0.1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentRectTransform = parent.GetComponent<RectTransform>();

        beforeVec = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        // Buttom Click 가능 범위 설정
        image1 = GetComponent<Image>();
        image1.alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isUpdate)
            return;

        parentHalfWidth = parentRectTransform.sizeDelta.x / 2;
        parentHalfHeight = parentRectTransform.sizeDelta.y / 2;
        halfWidth = rectTransform.sizeDelta.x / 2;
        halfHeight = rectTransform.sizeDelta.y / 2;

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isUpdate)
            return;

        // 이전 이동과 비교해서 얼마나 이동했는지를 보여줌
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isUpdate)
            return;

        xRange = Mathf.Clamp(rectTransform.anchoredPosition.x, -parentHalfWidth + halfWidth, parentHalfWidth - halfWidth);
        yRange = Mathf.Clamp(rectTransform.anchoredPosition.y, -parentHalfHeight + halfHeight, parentHalfHeight - halfHeight);

        currentPosition = new Vector2(xRange, yRange);
        rectTransform.anchoredPosition = currentPosition;

        beforeVec = currentPosition;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
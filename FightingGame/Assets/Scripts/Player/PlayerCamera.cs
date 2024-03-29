using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class PlayerCamera : MonoBehaviour
{
    public Camera cam;
    public Transform target;

    public float halfHeight;
    public float halfWidth;
    float clampedX, clampedY;

    public Vector2 minBound;
    public Vector2 maxBound;

    float mapSize;
    float playerCamSize = 5.5f;
    float zoomSpeed = 0.1f;

    Coroutine cameraZoomInCoroutine;
    Coroutine cameraMovingCoroutine;

    private void LateUpdate()
    {
        if (target == null || cameraMovingCoroutine != null)
            return;

        FollowingCamera();
    }

    public void Init(BaseMap _map)
    {
        cam = Camera.main;
        cam.transform.position = new Vector3(0, 0, -10);

        Set_CameraBounds(_map.maxBound, _map.minBound);

        playerCamSize = 5.5f;
        mapSize = _map.maxBound.x / cam.aspect;

        cam.orthographicSize = mapSize;

        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    public void Following_Target(Transform target)
    {
        Set_Target(target);
        Camera_ZoomIn();
    }

    public void Set_CameraBounds(Vector2 _maxBound, Vector2 _minBound)
    {
        minBound = _minBound;
        maxBound = _maxBound;
    }

    private void FollowingCamera()
    {
        clampedX = Mathf.Clamp(target.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        clampedY = Mathf.Clamp(target.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, -10);
    }

    public void Camera_Moving()
    {
        if (cameraMovingCoroutine != null)
            StopCoroutine(cameraMovingCoroutine);

        cameraMovingCoroutine = StartCoroutine(ICamera_Moving());
    }

    public void Camera_ZoomIn()
    {
        if (cameraZoomInCoroutine != null)
            StopCoroutine(cameraZoomInCoroutine);

        if (this.playerCamSize < cam.orthographicSize)
            cameraZoomInCoroutine = StartCoroutine(ICamera_ZoomIn(zoomSpeed * -1f));
    }

    public void Set_Target(Transform _transform)
    {
        this.target = _transform;
    }

    IEnumerator ICamera_ZoomIn(float _zoomSpeed)
    {
        while (cam.orthographicSize > playerCamSize)
        {
            cam.orthographicSize += _zoomSpeed;
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * cam.aspect;
            yield return new WaitForSeconds(0.01f);
        }

        cam.orthographicSize = playerCamSize;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    IEnumerator ICamera_Moving()
    {
        yield return new WaitUntil(() => target != null);

        while(Mathf.Clamp(target.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth) != transform.position.x)
        {
            clampedX = Mathf.Clamp(Mathf.Lerp(transform.position.x, target.position.x, 0.03f), minBound.x + halfWidth, maxBound.x - halfWidth);
            clampedY = Mathf.Clamp(Mathf.Lerp(transform.position.y, target.position.y, 0.03f), minBound.y + halfHeight, maxBound.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, -10);

            yield return null;
        }

        cameraMovingCoroutine = null;
    }
}

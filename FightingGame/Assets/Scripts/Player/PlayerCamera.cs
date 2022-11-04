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

    public float clampedX;
    public float clampedY;

    public Vector2 minBound;
    public Vector2 maxBound;
    ENUM_MAP_TYPE mapType;
    public Vector3 mapTarget;
    float mapSize;
    float playerCamSize;

    private void LateUpdate()
    {
        if (target == null)
            return;

        FollowingCamera();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Transform target)
    {
        cam = Camera.main;
        Set_ZoomIn();
        Set_target(target);
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

    public void Set_OrthographicSize(float _size) => cam.orthographicSize = _size;
    public void Set_CameraZoomIn(float _zoomSpeed) 
        => StartCoroutine(Zoom_In(_zoomSpeed));
    public void Set_CameraZoomOut(float _zoomSpeed) 
        => StartCoroutine(Zoom_Out(_zoomSpeed));

    IEnumerator Zoom_In(float _zoomSpeed)
    {
        while (cam.orthographicSize > playerCamSize)
        {
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * Screen.width / Screen.height;

            cam.orthographicSize += _zoomSpeed;
            Set_OrthographicSize(cam.orthographicSize);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Zoom_Out(float _zoomSpeed)
    {
        while (cam.orthographicSize < mapSize)
        {
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * Screen.width / Screen.height;

            transform.position = Vector3.MoveTowards(transform.position, mapTarget, 0.5f);

            cam.orthographicSize += _zoomSpeed;
            Set_OrthographicSize(cam.orthographicSize);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Set_MapData(BaseMap _map)
    {
        Set_Maptarget(_map.transform);
        Set_CameraBounds(_map.maxBound, _map.minBound);

        mapType = (ENUM_MAP_TYPE)Enum.Parse(typeof(ENUM_MAP_TYPE), _map.name);
        playerCamSize = Managers.Battle.Get_playerCamSizeDict(mapType);
        mapSize = Managers.Battle.Get_MapSizeDict(mapType);
        Set_OrthographicSize(mapSize);
    }

    // 맵의 위치값
    public void Set_Maptarget(Transform _transform)
    {
        this.mapTarget = _transform.position;
        this.mapTarget.z = this.transform.position.z;
    }

    public void Set_ZoomOut()
    {
        if (this.mapSize > cam.orthographicSize)
            StartCoroutine(Zoom_Out(0.1f));
    }

    public void Set_ZoomIn()
    {
        if (this.playerCamSize < cam.orthographicSize)
            StartCoroutine(Zoom_In(-0.1f));
    }

    public void Set_target(Transform _transform) => this.target = _transform;
}

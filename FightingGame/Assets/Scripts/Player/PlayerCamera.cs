using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 maptarget;

    private void LateUpdate()
    {
        if (target == null)
            return;

        FollowingCamera();
    }

    public void Init(Transform target)
    {
        cam = Camera.main;
        Set_CameraZoomIn(-0.1f, cam.orthographicSize, 5);
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

    private void Set_OrthographicSize(float _size) => cam.orthographicSize = _size;
    public void Set_CameraZoomIn(float _zoomSpeed, float _currSize, float _zoomSize) => StartCoroutine(Zoom_In(_zoomSpeed, _currSize, _zoomSize));
    public void Set_CameraZoomOut(float _zoomSpeed, float _currSize, float _zoomSize) => StartCoroutine(Zoom_Out(_zoomSpeed, _currSize, _zoomSize));

    IEnumerator Zoom_In(float _zoomSpeed, float _currSize, float _zoomSize)
    {
        while (_currSize >= _zoomSize)
        {
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * Screen.width / Screen.height;

            _currSize += _zoomSpeed;
            Set_OrthographicSize(_currSize);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Zoom_Out(float _zoomSpeed, float _currSize, float _zoomSize)
    {
        while (_currSize <= _zoomSize)
        {
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * Screen.width / Screen.height;

            transform.position = Vector3.MoveTowards(transform.position, maptarget, 0.12f);

            _currSize += _zoomSpeed;
            Set_OrthographicSize(_currSize);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Set_target(Transform _transform) => this.target = _transform;

    // 맵의 위치값
    public void Map_target(Vector3 _position)
    {
        maptarget = _position;
        maptarget.z = transform.position.z;
    }
}

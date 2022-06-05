using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera cam;
    public Transform target;

    private Vector3 cameraSubPos;

    private void LateUpdate()
    {
        FollowingCamera();
    }

    public void Init(Transform target)
    {
        cam = Camera.main;
        cameraSubPos = new Vector3(0, 0, -10);

        this.target = target;
    }

    private void FollowingCamera()
    {
        transform.position = target.position + cameraSubPos;
    }
}

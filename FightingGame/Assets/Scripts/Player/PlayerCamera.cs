using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera cam;
    public Transform target;

    private void LateUpdate()
    {
        FollowingCamera();
    }

    public void Init(Transform target)
    {
        cam = Camera.main;

        this.target = target;
    }

    private void FollowingCamera()
    {
        transform.position = target.position;
    }
}

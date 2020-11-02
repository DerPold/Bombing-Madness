using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float lookSmooth = 0.01f;
    public Vector3 offsetFromTarget = new Vector3(0, 15, -14);
    public Vector3 offset;
    public float xTilt = 10;

    Vector3 destination = Vector3.zero;
    PlayerControlsUpdatet charController;
    float rotateVel = 0;

    void Start()
    {
        SetCameraTarget(target);
        //offsetFromTarget = transform.position - target.position;
        offset = offsetFromTarget;
    }

    void SetCameraTarget(Transform t)
    {
        target = t;

        if (target != null)
        {
            if(target.GetComponent<PlayerControlsUpdatet>())
            {
                charController = target.GetComponent<PlayerControlsUpdatet>();
            }
            else Debug.LogError("Your cameras target needs a character controller.");
        }
        else Debug.LogError("Your camera needs a target.");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        MoveToTarget();
        LookAtTarget();
    }

    void MoveToTarget()
    {
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 4.0f, Vector3.right) * offset;
        destination = charController.TargetRotation * offsetFromTarget;
        destination += target.position;
        //destination += offset;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        /*float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        float eulerXAngle = transform.eulerAngles.x;
        float mouseY = Input.GetAxis("Mouse Y") * 4.0f;
        xTilt = eulerXAngle;
        if (eulerXAngle - mouseY > 285 || eulerXAngle - mouseY < 35)
        {
            eulerXAngle -= mouseY;
        }
        transform.rotation = Quaternion.Euler(eulerXAngle, eulerYAngle, 0);
        */
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 4.0f, Vector3.right) * offset;
        //transform.position = target.position + offset;
        Vector3 lookPosition = target.position;
        transform.LookAt(lookPosition);
    }
}

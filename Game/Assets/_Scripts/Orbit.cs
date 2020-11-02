using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{

    public float turnSpeed = 4.0f;
    public Transform player;

    public Vector3 offset;
    float rotateVel = 0;
    public float lookSmooth = 0.09f;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right) * offset;
        transform.position = player.position + offset;
        Vector3 lookPosition = player.position;
        transform.LookAt(lookPosition);
    }
}
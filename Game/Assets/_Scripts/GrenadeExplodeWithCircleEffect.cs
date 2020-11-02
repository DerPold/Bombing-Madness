using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplodeWithCircleEffect : GrenadeExploding
{
    public GameObject circleEffect;
    private Rigidbody rb;
    private Vector3 oldPos;

    private void Awake()
    {
        delay = 2.0f;
        radius = 5.0f;
        force = 700f;
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player") && !collider.gameObject.CompareTag("Enemy"))
        {
            if (!rocketsStarted)
            {
                rocketsStarted = true;
                StartRockets();
            }
            if (!hasExploded)
            {
                hasExploded = true;
                oldPos = transform.position;
                Explode(10, true);

                Invoke("inst1", 0.15f);
                Invoke("inst2", 0.3f);
                Invoke("inst3", 0.45f);
                Invoke("inst4", 0.6f);
                Invoke("inst5", 0.75f);
                Invoke("inst6", 0.9f);
                Invoke("inst7", 1.05f);
                Invoke("inst8", 1.2f);
            }
        }
    }

    private Vector3 FindPointInCircle(Vector3 c, float r, int i)
    {
        return c + Quaternion.AngleAxis(45.0f * i, Vector3.up) * (Vector3.right * r);
    }

    void inst1()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 0), transform.rotation);
        Destroy(obj);
    }

    void inst2()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 1), transform.rotation);
        Destroy(obj, 10.0f);
    }

    void inst3()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 2), transform.rotation);
        Destroy(obj, 10.0f);
    }

    void inst4()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 3), transform.rotation);
        Destroy(obj, 10.0f);
    }

    void inst5()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 4), transform.rotation);
        Destroy(obj, 10.0f);
    }

    void inst6()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 5), transform.rotation);
        Destroy(obj, 10.0f);
    }

    void inst7()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 6), transform.rotation);
        Destroy(obj, 10.0f);
    }

    void inst8()
    {
        GameObject obj = Instantiate(circleEffect, FindPointInCircle(oldPos, 5.0f, 7), transform.rotation);
        Destroy(obj, 10.0f);
    }

    public override void Update()
    {
        base.Update();
    }
}

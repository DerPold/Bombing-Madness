using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplodeOnContact : GrenadeExploding
{
    private Rigidbody rb;

    private void Awake()
    {
        delay = 2.0f;
        radius = 5.0f;
        force = 700f;
        rb = GetComponent<Rigidbody>();        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(!collider.gameObject.CompareTag("Player") && !collider.gameObject.CompareTag("Enemy"))
        {
            if (!rocketsStarted)
            {
                rocketsStarted = true;
                StartRockets();
            }
            if (!hasExploded)
            {
                hasExploded = true;
                Explode(10,true);
            }
        }
    }

    public override void Update()
    {
        base.Update();
    }
        
}

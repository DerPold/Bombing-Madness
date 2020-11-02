using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeControlled1 : GrenadeExploding
{
    float countdown;
    bool countdownActivated = false;

    void Start()
    {
         delay = 0.2f;
         radius = 5.0f;
         force = 700f;
         countdown = delay;
    }

    public void StartCountdown()
    {
        countdownActivated = true;
    }

    public override void Update()
    {
        base.Update();
        if (countdownActivated)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0.1f && !rocketsStarted && !hasExploded)
            {
                rocketsStarted = true;
                StartRockets();
            }
            if (countdown <= 0f && !hasExploded)
            {
                hasExploded = true;
                Explode(20,false);
            }
        }
    }
}

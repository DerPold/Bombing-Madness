 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMultipleExplosions : GrenadeExploding
{
    public int explosionCount;

    float countdown;
    bool hasExplodedTwo = false;
    bool hasExplodedThree = false;
    bool hasExplodedFour = false;

    void Start()
    {
        delay = 5.0f;
        radius = 5.0f;
        force = 700f;
        countdown = delay;
    }

    public override void Update()
    {
        base.Update();
        countdown -= Time.deltaTime;
        if (countdown <= 1f && !rocketsStarted && !hasExploded)
        {
            rocketsStarted = true;
            StartRockets();
        }
        if (countdown <= 0.75f && !hasExplodedFour && explosionCount >= 4)
        {
            hasExplodedFour = true;
            Explode(40,false);
        }
        if (countdown <= 0.5f && !hasExplodedThree && explosionCount >= 3)
        {
            hasExplodedThree = true;
            Explode(30,false);
        }
        if (countdown <= 0.25f && !hasExplodedTwo && explosionCount >= 2)
        {
            hasExplodedTwo = true;
            Explode(20,false);
        }
        if (countdown <= 0f && !hasExploded)
        {
            hasExploded = true;
            Explode(10,false);
        }
    }
}
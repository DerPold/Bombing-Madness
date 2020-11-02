using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : GrenadeExploding
{
    float countdown;

	void Start ()
    {
         delay = 3.0f;
         radius = 5.0f;
         force = 700f;
         countdown = delay;
    }
	
	public override void Update ()
    {
        base.Update();
        countdown -= Time.deltaTime;
        if(countdown <= 1f)
        {
            if (!rocketsStarted && !hasExploded)
            {
                rocketsStarted = true;
                StartRockets();
            }
        }
        if(countdown <= 0f && !hasExploded)
        {
            hasExploded = true;
            Explode(15,false);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWithPowerupDropping : Destruct {

    public GameObject destroyedVersion;
    public GameObject powerup1;
    public GameObject powerup2;
    public GameObject powerup3;
    public GameObject powerup4;
    public GameObject powerup5;
    public GameObject powerup6;

    override public void Destroy()
    {
        if (destroyedVersion != null)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
        }

        GameObject powerupToInstantiate = null;
        int z = Random.Range(0, 6);
        if (z == 0) powerupToInstantiate = powerup1;
        if (z == 1) powerupToInstantiate = powerup2;
        if (z == 2) powerupToInstantiate = powerup3;
        if (z == 3) powerupToInstantiate = powerup4;
        if (z == 4) powerupToInstantiate = powerup5;
        if (z == 5) powerupToInstantiate = powerup6;

        if (powerupToInstantiate != null)
        {
            Instantiate(powerupToInstantiate, transform.position + new Vector3(0f,0f,0f), transform.rotation);
        }

        DecisionTree.instance.reloadCollectables();

        Destroy(gameObject);
    }
}

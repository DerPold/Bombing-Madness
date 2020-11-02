using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : Destruct {

    public GameObject destroyedVersion;
    private AscendingWater aw;
    void Start()
    {
        if (this.gameObject.tag == "Tree")
        {
            GameObject water = GameObject.Find("Water4Simple");
            aw = water.GetComponent<AscendingWater>();
        }
    }
	override public void Destroy()
    {
        string tag = this.gameObject.tag;
        if (destroyedVersion != null)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
        }
        if (tag == "Tree")
        {
            Debug.Log("Water rises now faster :D");
            aw.increaseRiseFactor();
            DecisionTree.instance.reloadCollectables();
        }
        Destroy(gameObject);
    }
}

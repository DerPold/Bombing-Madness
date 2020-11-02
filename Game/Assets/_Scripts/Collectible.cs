using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public int collectValue;

    void Awake()
    {
        //++objects;
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.playFoodCollectSound();
            PointCounter pc = player.GetComponent<PointCounter>();           
            pc.AddValue(collectValue);

            Destroy(gameObject);
        }
        if (player.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

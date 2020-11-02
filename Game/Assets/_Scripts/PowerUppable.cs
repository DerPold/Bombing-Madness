using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUppable : MonoBehaviour {

    public int actionTag = 0;
    bool bonusGranted;

    void Awake()
    {
        //
    }

     void OnTriggerEnter(Collider player)
     {
        if (player.gameObject.CompareTag("Player") && !bonusGranted)
        {
            int value = 0;
            if (player.gameObject.CompareTag("Player"))
            {
                AudioManager.instance.playPowerupCollectSound();
            }
            PointCounter pc = player.GetComponent<PointCounter>();
            PlayerControlsUpdatet pcu = player.GetComponent<PlayerControlsUpdatet>();
            if(actionTag == 0)
            {
                value = 5;
                pcu.increaseSpeed();
                bonusGranted = true;
            }
            if (actionTag == 1)
            {
                value = 7;
                BombThrower bt = Camera.main.gameObject.GetComponent<BombThrower>();
                bt.increaseCountForMode(4, 10);
                bt.decreaseCooldown();
                bonusGranted = true;
            }
            if (actionTag == 2)
            {
                value = 6;
                pcu.increaseJumpHeightForDuration();
                bonusGranted = true;
            }
            if (actionTag == 3)
            {
                value = 8;
                BombThrower bt = Camera.main.gameObject.GetComponent<BombThrower>();
                bt.increaseCountForMode(3, 10);
                bonusGranted = true;
            }
            if (actionTag == 4)
            {
                value = 9;
                BombThrower bt = Camera.main.gameObject.GetComponent<BombThrower>();
                bt.increaseCountForMode(5, 10);
                bonusGranted = true;
            }
            if (actionTag == 5)
            {
                value = 10;
                BombThrower bt = Camera.main.gameObject.GetComponent<BombThrower>();
                bt.increaseCountForMode(2, 10);
                bonusGranted = true;
            }


            pc.AddValue(value);

            Destroy(gameObject);
        }
        if (player.gameObject.CompareTag("Enemy")) Destroy(gameObject);
    }
}

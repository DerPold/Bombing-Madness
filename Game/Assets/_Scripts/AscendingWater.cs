using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendingWater : MonoBehaviour {

    public GameObject waterfall;
    public GameObject waterfall2;
    public GameObject waterfall3;
    public GameObject waterfall4;
    public GameObject bubbleParticleSystem;
    public GameObject bubbleParticleSystemElf;
    public GameObject bubbleParticleSystemIzzy;
    private int treeCounter = 0;
    private float counter;
    private float counterThreshold;
    private float risingWorking = 0.001f;
    private GameObject player;
    private GameObject izzy;
    private GameObject elf;
    void Start ()
    {
        counterThreshold = 0.1f;
        counter = 0.1f;
        treeCounter = 0;
        risingWorking = 0.005f;
        player = PlayerManager.instance.player;
        izzy = PlayerManager.instance.izzy;
        elf = PlayerManager.instance.elf; 
        bubbleParticleSystem.SetActive(false);
        bubbleParticleSystemIzzy.SetActive(false);
        bubbleParticleSystemElf.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        actualizeParticleSystems();

        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = counterThreshold;
            transform.Translate(0f,this.risingWorking, 0f);
            waterfall.transform.Translate(0f, this.risingWorking, 0f);
            waterfall2.transform.Translate(0f, this.risingWorking, 0f);
            waterfall3.transform.Translate(0f, this.risingWorking, 0f);
            waterfall4.transform.Translate(0f, this.risingWorking, 0f);
        }
	}

    public void increaseRiseFactor()
    {
        this.risingWorking += 0.0003f;
        if (++treeCounter >= 25) risingWorking = 0.5f;
    }

    private void actualizeParticleSystems()
    {
        if (transform.position.y >= player.transform.position.y + 1.8f)
        {
            bubbleParticleSystem.SetActive(true);
        }
        else
        {
            bubbleParticleSystem.SetActive(false);
        }

        if ( !IsDestroyed(izzy) && transform.position.y >= izzy.transform.position.y + 1.8f)
        {
            bubbleParticleSystemIzzy.SetActive(true);
        }
        else if(!IsDestroyed(izzy))
        {
            bubbleParticleSystemIzzy.SetActive(false);
        }

        if (!IsDestroyed(elf) && transform.position.y >= elf.transform.position.y + 1.8f)
        {
            bubbleParticleSystemElf.SetActive(true);
        }
        else if(!IsDestroyed(elf))
        {
            bubbleParticleSystemElf.SetActive(false);
        }
    }

    private bool IsDestroyed(GameObject gameObject)
    {
        // UnityEngine overloads the == opeator for the GameObject type
        // and returns null when the object has been destroyed, but 
        // actually the object is still there but has not been cleaned up yet
        // if we test both we can determine if the object has been destroyed.
        return gameObject == null && !ReferenceEquals(gameObject, null);
    }
}

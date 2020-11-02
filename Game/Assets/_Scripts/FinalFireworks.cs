using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFireworks : MonoBehaviour {

    public int particleSystemId;


    private ParticleSystem rocketInstantiated;
    private int numberOfParticles = 0;
    // Use this for initialization
    void Start () {
        rocketInstantiated = transform.GetChild(particleSystemId).GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rocketInstantiated == null) return;
        int count = rocketInstantiated.particleCount;
        if (count < numberOfParticles)
        { //particle has died
            playBurstSound();
            int z = Random.Range(0, 10);
            if (z < 2) playRumbleSound();
        }
        else if (count > numberOfParticles)
        { //particle has been born
            //playAscendingSound();
        }
        numberOfParticles = count;
    }

    void playBurstSound()
    {
        AudioManager.instance.playBurstSound();
    }

    void playAscendingSound()
    {
        AudioManager.instance.playAscendingSound();
    }

    void playRumbleSound()
    {
        AudioManager.instance.playRumbleSound();
    }
}

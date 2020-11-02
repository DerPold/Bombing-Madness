using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExploding : MonoBehaviour {

    public GameObject explosionEffect;
    public GameObject rocketEffect;
    public AudioClip explosionSound;
    public AudioClip rocketSoundStarting;
    public AudioClip rocketSoundExploding;
    public AudioClip rocketRumbleSound;
    public AudioSource source;

    public bool hasExploded = false;
    public bool rocketsStarted = false;
    public float delay;
    public float radius;
    public float force;
    public string throwTag;

    private ParticleSystem rocketInstantiated;
    private int numberOfParticles = 0;
    
    public void StartRockets()
    {
        Vector3 rocketPosition = transform.position + new Vector3(0, 0, 0);
        Quaternion rocketRotation = Quaternion.Euler(-90, 0, 0);
        GameObject rocket = Instantiate(rocketEffect, rocketPosition, rocketRotation);
        rocketInstantiated = rocket.GetComponent<ParticleSystem>();
        Destroy(rocket, 20.0f);
    }

    void playBurstSound()
    {
        //AudioManager.instance.playBurstSound(sourceId);
        source.PlayOneShot(rocketSoundExploding,1.5f);
    }

    void playAscendingSound()
    {
        //AudioManager.instance.playAscendingSound(sourceId);
        source.PlayOneShot(rocketSoundStarting, 0.3f);
    }

    void playRumbleSound()
    {
        //AudioManager.instance.playRumbleSound(sourceId);
        source.PlayOneShot(rocketRumbleSound, 1.2f);
    }

    void playBombExplosionSound()
    {
        //AudioManager.instance.playBombExplosionSound(sourceId);
        source.PlayOneShot(explosionSound, 1.0f);
    }

    public virtual void Update()
    {
        if (rocketInstantiated == null) return;
        int count = rocketInstantiated.particleCount;
        if (count < numberOfParticles)
        { //particle has died
            //AudioManager.instance.stopSource(sourceId);
            source.Stop();
            playBurstSound();
            int z = Random.Range(0, 10);
            if (z < 2) playRumbleSound();
        }
        else if (count > numberOfParticles)
        { //particle has been born
            playAscendingSound();
        }
        numberOfParticles = count;
    }

    public void Explode(int pointBonus, bool throwEnemyInAir)
    {
        //sourceId = AudioManager.instance.getFreeSourceId();
        //Debug.Log("Got this SourceId: " + sourceId);
        Vector3 bombPosition = transform.position + new Vector3(0, 2, 0);
        Quaternion bombRotation = transform.rotation;
        GameObject explosion = Instantiate(explosionEffect, bombPosition, bombRotation);

        playBombExplosionSound();
        // Destroy Objects
        Collider[] objectsToDestroy = Physics.OverlapSphere(bombPosition, radius);
        foreach (Collider nearbyObject in objectsToDestroy)
        {
            Destruct dest = nearbyObject.GetComponent<Destruct>();
            if (dest != null)
            {
                dest.Destroy();
            }
            if (nearbyObject.gameObject.CompareTag("Player"))
            {
                PlayerControlsUpdatet pcu = nearbyObject.gameObject.GetComponent<PlayerControlsUpdatet>();
                if (throwTag.Equals("Player")) pcu.setStunned(true);
                else pcu.setStunned(false);
            }
            if (nearbyObject.gameObject.CompareTag("Enemy"))
            {
                if (throwTag.Equals("Player"))
                {
                    PointCounter pc = PlayerManager.instance.pointCounter;
                    pc.AddValue(pointBonus);
                }
                SimpleAI ai = nearbyObject.gameObject.GetComponent<SimpleAI>();
                ai.SetStunned();
                if (throwEnemyInAir)
                {
                    Rigidbody rigidBody = GetComponent<Rigidbody>();
                    ai.AddImpact(rigidBody.velocity, 150f);
                }
            }
        }

        // Add force
        Collider[] nearbyObjects = Physics.OverlapSphere(bombPosition, radius);
        foreach (Collider nearbyObject in nearbyObjects)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && !nearbyObject.CompareTag("Collectable"))
            {
                rb.AddExplosionForce(force, bombPosition, radius);
            }
        }
        Destroy(explosion, 20.0f);
        if (hasExploded)
        {
            foreach (Transform child in transform)
            {
                if(child.gameObject.CompareTag("BombPart")) child.GetComponent<MeshRenderer>().enabled = false;
            }
            //Invoke("releaseSourceId", 10.0f);
            Destroy(gameObject, 20.0f);
        }
    }

    /*void releaseSourceId()
    {
        Debug.Log("Source Id: " + sourceId + " got freed");
        AudioManager.instance.releaseSourceId(sourceId);
    }*/
}

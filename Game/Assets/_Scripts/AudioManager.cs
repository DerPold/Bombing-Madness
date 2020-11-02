using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Singleton

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
        allSources = GetComponents<AudioSource>();
        playGameSong();
    }

    void Update()
    {
        if(!allSources[0].isPlaying && !gameOver) playGameSong();
    }

    #endregion

    public AudioClip explosionSound;
    public AudioClip rocketSoundStarting;
    public AudioClip rocketSoundExploding;
    public AudioClip rocketRumbleSound;
    public AudioClip collectSoundPowerup;
    public AudioClip collectSoundFood;
    public AudioClip stunnedSound;
    public AudioClip winningSound;
    public AudioClip looseSound;
    public AudioClip gameSong;


    private AudioSource[] allSources;
    private bool gameOver = false;
    private GameObject finalFireworks;

    public void playFinalFireworks(GameObject fireworks)
    {
        finalFireworks = fireworks;
    }

    public void playPowerupCollectSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(collectSoundPowerup, 1.5f);
    }

    public void playFoodCollectSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(collectSoundFood, 1.3f);
    }

    public void playStunnedSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(stunnedSound, 1.0f);
    }

    public void playWinningSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(winningSound, 1.0f);
    }

    public void playLooseSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(looseSound, 1.0f);
    }

    private void playGameSong()
    {
        allSources[0].PlayOneShot(gameSong, 0.6f);
    }

    public int getFreeSourceId()
    {
        for(int i= 1; i < allSources.Length; ++i)
        {
            if (!allSources[i].isPlaying)
            {
                return i;
            }
        }
        return 0;
    }

    public void stopSource(int id)
    {
        allSources[id].Stop();
        if (id == 0) gameOver = true;
    }

    public void playBurstSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(rocketSoundExploding, 1.5f);
    }

    public void playAscendingSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(rocketSoundStarting, 0.3f);
    }

    public void playRumbleSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(rocketRumbleSound, 1.2f);
    }

    public void playBombExplosionSound()
    {
        allSources[getFreeSourceId()].PlayOneShot(explosionSound, 1.0f);
    }
}

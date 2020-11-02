using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {


    public GameObject water;
    public GameObject bigFireWorks;
    public GameObject player;
    public GameObject izzy;
    public GameObject elf;
    public float restartDelay = 5f;
    public float drunkDelay = 5f;
    private PointCounter pc;
    private PlayerControlsUpdatet pcu;

    Animator anim;
    float restartTimer;
    float drunkTimer;
    float drunkTimerIzzy;
    float drunkTimerElf;
    bool dead;
    bool won;
    bool elfIsDead;
    bool izzyIsDead;

	void Awake ()
    {
        anim = GetComponent<Animator>();
        pc = player.GetComponent<PointCounter>();
        pcu = player.GetComponent<PlayerControlsUpdatet>();
	}
	
	void Update ()
    {
        if (!IsDestroyed(izzy) && water.transform.position.y >= izzy.transform.position.y + 2.2f)
        {
            drunkTimerIzzy += Time.deltaTime;
            if (drunkTimerIzzy >= drunkDelay)
            {
                SimpleAI simpleai = izzy.GetComponent<SimpleAI>();
                izzyIsDead = true;
                simpleai.die();
            }
        }
        else drunkTimerIzzy = 0f;

        if (!IsDestroyed(elf) && water.transform.position.y >= elf.transform.position.y + 2.2f)
        {
            drunkTimerElf += Time.deltaTime;
            if (drunkTimerElf >= drunkDelay)
            {
                SimpleAI simpleai = elf.GetComponent<SimpleAI>();
                elfIsDead = true;
                simpleai.die();
            }
        }
        else drunkTimerElf = 0f;

        if (dead || won)
        {
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene("Arena_2");
            }
        }
        else if (pc.getCountedValue() >= 1300 || (izzyIsDead && elfIsDead) )
        {
            anim.SetTrigger("GameWon");
            restartDelay = 10f;
            won = true;
            AudioManager.instance.stopSource(0);
            AudioManager.instance.playWinningSound();
            Instantiate(bigFireWorks, player.transform.position, player.transform.rotation);
        }
        else if(water.transform.position.y >= player.transform.position.y + 2f)
        {
            drunkTimer += Time.deltaTime;
            if (drunkTimer >= drunkDelay)
            {
                pcu.playDeathAnimation();
                anim.SetTrigger("GameOver");
                dead = true;
                AudioManager.instance.stopSource(0);
                AudioManager.instance.playLooseSound();
            }
        }
        else
        {
            drunkTimer = 0;
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

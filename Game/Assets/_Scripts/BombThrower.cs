using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombThrower : MonoBehaviour
{
    public float throwForce = 15f;
    public Slider slider;
    public Image image;
    public Sprite lightGray;
    public Sprite red;
    public Sprite orange;
    public Sprite green;
    public Sprite lila;
    public GameObject bombPrefabBig;
    public GameObject bombPrefabSmall;
    public GameObject bombPrefabDoubleExplosion;
    public GameObject bombExplodeOnContact;
    public GameObject bombControlledExplosion;
    public Transform playerTransform;
    public Text bombCount;
    public Image bombiconTemp;
    public Text bombcountTemp;

    private int countBig = 5;
    private int countSmall = 10000;
    private int countDoubleExplosion = 5;
    private int countExplodeOnContact = 5;
    private int countControlledExplosion = 5;
    private float tempCounter = 0.0f;

    public int mode = 1;
    float cooldown = 0f;
    float cooldownAmount = 5f;
    private Queue<GameObject> bombsControlled;

	void Start () {
        slider.maxValue = cooldownAmount;
        slider.value = slider.maxValue;
        bombsControlled = new Queue<GameObject>();
        setBombCountTextForMode();
        bombiconTemp.enabled = false;
        bombcountTemp.enabled = false;
	}

    void Update ()
    {
        if (tempCounter > 0)
        {
            tempCounter -= Time.deltaTime;
            if (tempCounter <= 0)
            {
                tempCounter = 0;
                bombiconTemp.enabled = false;
                bombcountTemp.enabled = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(mode == 1) ThrowBomb(bombPrefabSmall);
            if(mode == 2) ThrowBomb(bombPrefabBig);
            if(mode == 3) ThrowBomb(bombPrefabDoubleExplosion);
            if(mode == 4) ThrowBomb(bombExplodeOnContact);
            if(mode == 5) ThrowBomb(bombControlledExplosion);
        }
        if(Input.GetMouseButtonDown(1))
        {
            controlledBombExplode();
        }
        if(Input.GetKey(KeyCode.Alpha1))
        {
            mode = 1;
            setBombCountTextForMode();
        }
        if(Input.GetKey(KeyCode.Alpha2))
        {
            mode = 2;
            setBombCountTextForMode();
        }
        if(Input.GetKey(KeyCode.Alpha3))
        {
            mode = 3;
            setBombCountTextForMode();
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            mode = 4;
            setBombCountTextForMode();
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            mode = 5;
            setBombCountTextForMode();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            mode += 1;
            if (mode == 6) mode = 1;
            setBombCountTextForMode();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            mode -= 1;
            if (mode == 0) mode = 5;
            setBombCountTextForMode();
        }

        setBombIcon();

        if (cooldownAmount > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0) cooldown = 0;
        }
        slider.value = cooldownAmount - cooldown;
    }

    private void setBombIcon()
    {
        if (mode == 1) image.sprite = green;
        if (mode == 2) image.sprite = lightGray;
        if (mode == 3) image.sprite = red;
        if (mode == 4) image.sprite = orange;
        if (mode == 5) image.sprite = lila;
    }

    private void setBombIconTemp(int mode)
    {
        if (mode == 1) bombiconTemp.sprite = green;
        if (mode == 2) bombiconTemp.sprite = lightGray;
        if (mode == 3) bombiconTemp.sprite = red;
        if (mode == 4) bombiconTemp.sprite = orange;
        if (mode == 5) bombiconTemp.sprite = lila;
    }

    public void decreaseCooldown()
    {
        if (cooldownAmount > 1.0f) cooldownAmount -= 0.5f;
        else if (cooldownAmount > 0.5f) cooldownAmount -= 0.1f;
        slider.maxValue = cooldownAmount;
    }

    private void ThrowBomb(GameObject grenadeToThrow)
    {
        if (cooldown <= 0 && getCountForMode() > 0)
        {
            decreaseCountForMode();
            GameObject bomb = Instantiate(grenadeToThrow, playerTransform.position + new Vector3(0, 2, 0), playerTransform.rotation);
            GrenadeExploding ptr = bomb.GetComponent<GrenadeExploding>();
            if (ptr == null) Debug.Log("Fehler!");
            ptr.throwTag = "Player";
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(playerTransform.forward * throwForce, ForceMode.VelocityChange);
            cooldown = cooldownAmount;
            if (mode == 5) bombsControlled.Enqueue(bomb);
        }
    }

    private int getCountForMode()
    {
        if (mode == 1) return countSmall;
        if (mode == 2) return countBig;
        if (mode == 3) return countDoubleExplosion;
        if (mode == 4) return countExplodeOnContact;
        if (mode == 5) return countControlledExplosion;
        return -1;
    }

    private void decreaseCountForMode()
    {
        if (mode == 1) --countSmall;
        if (mode == 2) --countBig;
        if (mode == 3) --countDoubleExplosion;
        if (mode == 4) --countExplodeOnContact;
        if (mode == 5) --countControlledExplosion;
        setBombCountTextForMode();
    }

    public void increaseCountForMode(int mode, int value)
    {
        if (mode == 1) countSmall += value;
        if (mode == 2) countBig += value;
        if (mode == 3) countDoubleExplosion += value;
        if (mode == 4) countExplodeOnContact += value;
        if (mode == 5) countControlledExplosion += value;
        setBombCountTextForMode();
        bombcountTemp.text = "+ x10";
        bombcountTemp.enabled = true;
        setBombIconTemp(mode);
        bombiconTemp.enabled = true;
        tempCounter = 5.0f;
    }

    private void setBombCountTextForMode()
    {
        if (mode == 1) bombCount.text = "" + countSmall;
        if (mode == 2) bombCount.text = "" + countBig;
        if (mode == 3) bombCount.text = "" + countDoubleExplosion;
        if (mode == 4) bombCount.text = "" + countExplodeOnContact;
        if (mode == 5) bombCount.text = "" + countControlledExplosion;
    }

    private void controlledBombExplode()
    {
        if (bombsControlled.Count > 0)
        {
            GameObject bomb = bombsControlled.Dequeue();
            GrenadeControlled1 grenadeControlled = bomb.GetComponent<GrenadeControlled1>();
            if (grenadeControlled != null)
            {
                grenadeControlled.StartCountdown();
            }
        }
    }
}

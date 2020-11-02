using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadSSI : MonoBehaviour {

    public Text heartrate;

    public int PulseBuff;

    void Start()
    {

        InvokeRepeating("SendStuffToSSI", 4, 4);

        SSIBiosensor.instance.OnHeartRateUpdate += LogBVP;
        SSIBiosensor.instance.OnPulseUpdate += LogPulse;
        SSIBiosensor.instance.OnSkinConductanceUpdate += LogGSR;

    }

    void SendStuffToSSI()
    {
        SSIEventInterface.instance.SendFloatEvent("param", 42.42f);
        SSIEventInterface.instance.SendTextEvent("moretext");
    }

    void LogBVP(float value)
    {
        Debug.Log("BVP: " + value);        
    }

    void LogPulse(float value)
    {
        Debug.Log("Pulse: " + value);
        heartrate.text = "Pulse: " + value;
       if(value < 40)
        {
            PulseBuff = -3;
        }
        else if(40 <= value && value < 50)
        {
            PulseBuff = -2;
        }
        else if(50 <= value && value < 65)
        {
            PulseBuff = -1;
        }
        else if(65 <= value && value < 85)
        {
            PulseBuff = 0;
        }
        else if(85 <= value && value < 105)
        {
            PulseBuff = 1;
        }
        else if(105 <= value && value < 115)
        {
            PulseBuff = 2;
        }
        else
        {
            PulseBuff = 3;
        }
    }

    void LogGSR(float value)
    {
        Debug.Log("GSR: " + value);
    }
}

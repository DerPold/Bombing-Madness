using UnityEngine;
using System.Collections;

public class testSSI : MonoBehaviour {

    // Use this for initialization
    private float heartRate;

	void Start () {

        InvokeRepeating("SendStuffToSSI", 4, 4);
        
        SSIBiosensor.instance.OnHeartRateUpdate += LogBVP;
        SSIBiosensor.instance.OnPulseUpdate += LogPulse;
        SSIBiosensor.instance.OnSkinConductanceUpdate += LogGSR;
        
	}

    void SendStuffToSSI()
    {
        SSIEventInterface.instance.SendFloatEvent( "param", 42.42f);
        SSIEventInterface.instance.SendTextEvent( "moretext");
    }

    void LogBVP(float value)
    {
        Debug.Log("BVP: " + value);
        heartRate = value;
    }

    void LogPulse(float value)
    {
        Debug.Log("Pulse: " + value);
    }

    void LogGSR(float value)
    {
        Debug.Log("GSR: " + value);
    }
    
    public float getHeartrate()
    {
        return heartRate;
    }
}

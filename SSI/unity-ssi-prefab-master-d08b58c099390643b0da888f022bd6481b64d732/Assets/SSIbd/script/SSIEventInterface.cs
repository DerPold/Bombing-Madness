using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIEventSender;
using UnityEngine;
using System.Xml;

public class SSIEventInterface : MonoBehaviour
{
    private SSIEventSender.SSIEventSender sses;
    public static SSIEventInterface instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            initSSIEvent();
        }

        else
            Destroy(gameObject);
    }


    private void initSSIEvent()
    {
        XmlDocument document = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("SSIEventChannel", typeof(TextAsset));
        document.LoadXml(textAsset.text);
        this.sses = new SSIEventSender.SSIEventSender(document);
    }

    public void SendTextEvent( String message)
    {
        sses.sendText("myChannel", "unityEvents", "sender", message);
    }

    public void SendFloatEvent(String paramName, float value)
    {
        sses.sendEvent("myChannel", "unityEvents", "sender", new SSIEventMessage(paramName, value));
    }
}


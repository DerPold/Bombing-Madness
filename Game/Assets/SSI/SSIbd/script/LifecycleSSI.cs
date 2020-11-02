using UnityEngine;
using System.Collections;
using SSIEventSender;

public class LifecycleSSI : MonoBehaviour {
    
    private SSIStartup ssiStartup;
    public bool useFakePipeline;
    public bool dontDestroyOnLoad;

    void Awake()
    {
        if (dontDestroyOnLoad)
            DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start () {

        ssiStartup = new SSIStartup();
        string XMLPipeLineLocation = "\"" + Application.dataPath + @"/SSI/SSIbd/Resources/SSIConfig/biomonitor-procFake.pipeline" + "\"";
        if (!useFakePipeline)
            XMLPipeLineLocation = "\"" + Application.dataPath + @"/SSI/SSIbd/Resources/SSIConfig/biomonitor-live.pipeline" + "\"";
      
       ssiStartup.StartSSI(XMLPipeLineLocation);

      
    }

   
    void OnApplicationQuit()
    {
        ssiStartup.StopSSI();
        SSIDataUtil.CloseAllChannels();
    }
}

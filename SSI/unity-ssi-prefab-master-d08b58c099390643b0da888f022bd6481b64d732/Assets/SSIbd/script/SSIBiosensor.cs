using UnityEngine;
using System.Collections;

public class SSIBiosensor: MonoBehaviour {


    public static SSIBiosensor instance;

    public delegate void SensorUpdate(float val);
    public event SensorUpdate OnHeartRateUpdate;
    public event SensorUpdate OnSkinConductanceUpdate;
    public event SensorUpdate OnPulseUpdate;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        
        float heartRate = SSIDataUtil.GetHeartRate();
        if (!float.IsNaN(heartRate))
        {
            this.UpdateHeartRate(heartRate);
        }
        
        float skinConductance = SSIDataUtil.GetSkinConductance();
        if (!float.IsNaN(skinConductance))
        {
            this.UpdateSkinConductance(skinConductance);
        }

        float pulse = SSIDataUtil.GetPulse();
        if (!float.IsNaN(pulse))
        {
            this.UpdatePulse(pulse);
        }
        
    }

    private void UpdateHeartRate(float val = -1.0f)
    {
        if (OnHeartRateUpdate != null)
            OnHeartRateUpdate(val);
    }

    private void UpdatePulse(float val = -1.0f)
    {
        if (OnPulseUpdate != null)
            OnPulseUpdate(val);
    }

    private void UpdateSkinConductance(float val = -1.0f)
    {
        if (OnSkinConductanceUpdate != null)
            OnSkinConductanceUpdate(val);
    }
}

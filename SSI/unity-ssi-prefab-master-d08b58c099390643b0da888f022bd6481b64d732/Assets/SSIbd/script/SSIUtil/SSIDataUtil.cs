using UnityEngine;
using System.Collections;
using SSIParser.Channels;
using SSIParser.Util;
using Rug.Osc;

public class SSIDataUtil
{
    public SSIBiosensor biosensor;
    

    private static FloatChannel skinResponse;
    private static FloatChannel heartRate;
    private static FloatChannel bvp_hr;


    private static float GetFrameRate(FloatChannel channel)
    {
        float[][] data = null;

        if (channel != null)
            data = channel.GetCurrentData();

        if (data != null)
            return (float)data.Length / (float)channel.Rate;

        return float.NaN;
    }

    private static float GetNewValue(FloatChannel channel)
    {
        if (channel.IsRunning)
        {
            float[] value = channel.GetLastData();

            if (value != null)
                return value[0];
            else
                return float.NaN;
        }
        return float.NaN;
    }

    public static float GetHeartRate()
    {

          if (heartRate == null)
              heartRate = ReadersUtil.GetChannelByName("bvp") as FloatChannel;
        return GetNewValue(heartRate);
        
    }

    public static float GetHeartRateFrameRate()
    {
        return GetFrameRate(heartRate);
    }
    
    public static float GetSkinConductance()
    {
        if (skinResponse == null)
            skinResponse = ReadersUtil.GetChannelByName("gsr") as FloatChannel;

        return GetNewValue(skinResponse);
    }

    public static float GetSkinResponseFrameRate()
    {
        return GetFrameRate(skinResponse);
    }

    public static float GetPulse()
    {
        if (bvp_hr == null)
            bvp_hr = ReadersUtil.GetChannelByName("bvp_hr") as FloatChannel;

        return GetNewValue(bvp_hr);
    }

    public static float GetPulseFrameRate()
    {
        return GetFrameRate(bvp_hr);
    }

    public static void CloseAllChannels()
    {
        CloseChannel(heartRate);
        CloseChannel(skinResponse);
        CloseChannel(bvp_hr);
    }

    private static void CloseChannel(AbstractChannel channel)
    {
        if (channel != null)
            channel.CloseChannel();
    }


}
using SSIParser;
using System.Xml;
using SSIParser.Channels;
using UnityEngine;

public class ReadersUtil {
    private static bool isStarted = false;
    private static string xmlPath = @"/SSI/SSIbd/Resources/SSIConfig/SocketReader.xml";

    private static void StartChannels()
    {
        if(!isStarted)
        {
            XmlReader reader = XmlReader.Create(Application.dataPath + xmlPath);
            ReaderStarter.GenerateReaders(reader);
            Debug.Log("XML-Reader "+reader);
            isStarted = true;
        }
    }

    public static void StopChannels()
    {
        ReaderStarter.CloseReaders();
    }

    public static ChannelGroup GetChannelGroupById(long id)
    {
        StartChannels();
        return ReaderStarter.GetChannelGroupByID(id);
    }

    public static ChannelGroup GetChannelGroupByName(string name)
    {
        StartChannels();
        return ReaderStarter.GetChannelGroupByName(name);
    }

    public static AbstractChannel GetChannelByPort(int port)
    {
        StartChannels();
        return ReaderStarter.GetChannelByPort(port);
    }

    public static AbstractChannel GetChannelByPort(ChannelGroup channelGroup, int port)
    {
        AbstractChannel channel = channelGroup.GetChannel(port);
        return channel;
    }

    public static AbstractChannel GetChannelByName(string name)
    {
        StartChannels();
        return ReaderStarter.GetChannelByName(name);
    }

}

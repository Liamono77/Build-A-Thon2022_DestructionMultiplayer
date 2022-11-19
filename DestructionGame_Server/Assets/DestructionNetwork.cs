using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using System.Reflection;

public abstract class DestructionNetwork : MonoBehaviour
{
    public NetPeerConfiguration netConfiguration;
    public NetPeer netPeerKinda;

    // Update is called once per frame
    protected virtual void Update()
    {
        ProcessMessages();
    }
    public void ProcessMessages()
    {
        List<NetIncomingMessage> incomingMessages = new List<NetIncomingMessage>();
        int numberOfMessages = netPeerKinda.ReadMessages(incomingMessages);
        foreach (NetIncomingMessage message in incomingMessages)
        {
            Debug.Log($"Message recieved of type {message.MessageType.ToString()}");
            if (message.MessageType == NetIncomingMessageType.Data)
            {
                string theMessage = message.ReadString();
                Debug.Log($"data message had a string: {theMessage}");
            }
        }
    }


}

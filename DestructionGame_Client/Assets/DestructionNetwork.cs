using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using System.Reflection;

public abstract class DestructionNetwork : MonoBehaviour
{
    public NetPeerConfiguration netConfiguration;
    public NetPeer netPeerKinda;
    public bool isClient;

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
            //if (message.MessageType == NetIncomingMessageTyp)
        }
    }

    public void TestSendMessage(string theMessage)
    {
        NetOutgoingMessage message = netPeerKinda.CreateMessage();
        message.Write(theMessage);
        if (isClient)
        {
            //netPeerKinda = (NetClient)netPeerKinda;
            //netPeerKinda = netPeerKinda as NetClient;
            //netPeerKinda.SendMessage();
            NetClient netClient = netPeerKinda as NetClient;
            netClient.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }
        else
        {
            // netPeerKinda.SendMessage()
            //NetServer netServer = netPeerKinda as NetServer;
           // netServer.SendMessage(message, )
        }
        //netPeerKinda.SendMessage(message, )
        //NetClient net = new NetClient(netConfiguration);
        //net.SendMessage()
    }


}

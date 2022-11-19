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
            long senderID = message.SenderConnection.RemoteUniqueIdentifier;
            Debug.Log($"Message recieved of type {message.MessageType.ToString()} from sender of ID {senderID}");
            if (message.MessageType == NetIncomingMessageType.Data)
            {
                string functionName = message.ReadString();
                Component[] myScripts = gameObject.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in myScripts)
                {
                    MethodInfo methodInfo = script.GetType().GetMethod(functionName);
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(script, null);
                    }
                }
            }
        }
    }
}

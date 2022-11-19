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
    public void WriteRPCParameters(NetOutgoingMessage message, params object[] parameters)
    {
        Debug.Log($"Attempting to write RPC parameter definitions...");
        string parametersDefinition = "";
        foreach (object obj in parameters)
        {
            if (obj is int)
            {
                parametersDefinition = parametersDefinition + "I";
            }
            else if (obj is float)
            {
                parametersDefinition = parametersDefinition + "F";
            }
            else if (obj is string)
            {
                parametersDefinition = parametersDefinition + "S";
            }
            else if (obj is bool)
            {
                parametersDefinition = parametersDefinition + "B";
            }
            else
            {
                Debug.LogError($"Failed to determine object type when writig a parameter definition for object {obj}");
            }
        }
        Debug.Log($"Writing parameter definition as {parametersDefinition}");
        message.Write(parametersDefinition);

        Debug.Log($"attempting to write RPC parameter values...");
        foreach (object obj in parameters)
        {
            if (obj is int)
            {
                int theVariable = (int)obj;
                message.Write(theVariable);
                Debug.Log($"wrote an integer of value {theVariable}");

            }
            else if (obj is float)
            {
                float theVariable = (float)obj;
                message.Write(theVariable);
                Debug.Log($"wrote a float of value {theVariable}");

            }
            else if (obj is string)
            {
                string theVariable = (string)obj;
                message.Write(theVariable);
                Debug.Log($"wrote a string of value {theVariable}");

            }
            else if (obj is bool)
            {
                bool theVariable = (bool)obj;
                message.Write(theVariable);
                Debug.Log($"wrote a boolean of value {theVariable}");

            }
            else
            {
                Debug.LogError($"Failed to determine object type when writig a parameter value for object {obj}");
            }
        }
        Debug.Log($"Finished writing RPC parameters of definition {parametersDefinition}");
    }
}


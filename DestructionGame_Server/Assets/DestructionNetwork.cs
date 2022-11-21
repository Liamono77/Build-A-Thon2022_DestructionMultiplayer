using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using System.Reflection;
//DESTRUCTION NETWORK
//This my custom implementation of Lidgren. Takes inspiration from the UC Network framework we've been using in class, but made to be much simpler and focused on RPC use.
//WRITTEN BY LIAM SHELTON 
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

                Debug.Log($"Received an RPC call for function {functionName} from sender of ID {message.SenderConnection.RemoteUniqueIdentifier}");

                Component[] myScripts = gameObject.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in myScripts)
                {
                    MethodInfo methodInfo = script.GetType().GetMethod(functionName);
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(script, ReadRPCParameters(message));
                    }
                }
            }
        }
    }

    //The glorious Read/Write RPC Parameters functions. What more must be said?
    public object[] ReadRPCParameters(NetIncomingMessage message)
    {
 //       Debug.Log($"Attempting to read RPC parameters...");

        List<object> parameters = new List<object>();

        parameters.Add(message.SenderConnection); //this is to allow the server to have Power and Authority. Easily check RPCs before calling them using this first parameter

        string parametersDefinition = message.ReadString();
 //       Debug.Log($"Parameters definition was read as {parametersDefinition}");

        foreach (char character in parametersDefinition)
        {
            //Debug.Log($"Attempting to read character {character}..");
            if (character == 'I')
            {
                var parameter = message.ReadInt32();
                parameters.Add(parameter);
 //               Debug.Log($"added an integer of value {parameter}");
            }
            else if (character == 'F')
            {
                var parameter = message.ReadFloat();
                parameters.Add(parameter);
 //               Debug.Log($"added a float of value {parameter}");
            }
            else if (character == 'S')
            {
                var parameter = message.ReadString();
                parameters.Add(parameter);
//                Debug.Log($"added a string of value {parameter}");
            }
            else if (character == 'B')
            {
                var parameter = message.ReadBoolean();
                parameters.Add(parameter);
//                Debug.Log($"added a boolean of value {parameter}");
            }
            else
            {
                Debug.LogError($"Unrecognized parameter of character {character}");
            }
        }
        return parameters.ToArray();
    }

    public void WriteRPCParameters(NetOutgoingMessage message, params object[] parameters)
    {
//        Debug.Log($"Attempting to write RPC parameter definitions...");
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
//        Debug.Log($"Writing parameter definition as {parametersDefinition}");
        message.Write(parametersDefinition);

 //       Debug.Log($"attempting to write RPC parameter values...");
        foreach (object obj in parameters)
        {
            if (obj is int)
            {
                int theVariable = (int)obj;
                message.Write(theVariable);
 //               Debug.Log($"wrote an integer of value {theVariable}");

            }
            else if (obj is float)
            {
                float theVariable = (float)obj;
                message.Write(theVariable);
  //              Debug.Log($"wrote a float of value {theVariable}");

            }
            else if (obj is string)
            {
                string theVariable = (string)obj;
                message.Write(theVariable);
 //               Debug.Log($"wrote a string of value {theVariable}");

            }
            else if (obj is bool)
            {
                bool theVariable = (bool)obj;
                message.Write(theVariable);
 //               Debug.Log($"wrote a boolean of value {theVariable}");

            }
            else
            {
                Debug.LogError($"Failed to determine object type when writig a parameter value for object {obj}");
            }
        }
//        Debug.Log($"Finished writing RPC parameters of definition {parametersDefinition}");
    }
}


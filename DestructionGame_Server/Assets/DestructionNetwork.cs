using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using System.Reflection;

public class DestructionNetwork : MonoBehaviour
{
    public NetPeerConfiguration netConfiguration;// = new NetPeerConfiguration("DestructionNetwork")
   // {
   //     Port = 603,
   // };
    public NetPeer netPeerKinda;

  //  private void Start()
 //   {
  //      netPeerKinda = new NetServer(netConfiguration);
 //       netPeerKinda.Start();
  //  }

    // Update is called once per frame
    protected virtual void Update()
    {
        ProcessMessages();
    }
    public void ProcessMessages()
    {
        List<NetIncomingMessage> incomingMessages = new List<NetIncomingMessage>();
        int numberOfMessages = netPeerKinda.ReadMessages(incomingMessages);
        Debug.Log($"There are {numberOfMessages} new messages");
        foreach (NetIncomingMessage message in incomingMessages)
        {
            Debug.Log($"Message recieved of type {message.MessageType.ToString()}");
            //if (message.MessageType == NetIncomingMessageTyp)
        }
    }
    // Start is called before the first frame update
 //   void Start()
//    {
        
 //   }


}

using System;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using Common;
using Common.Net.Core;
using Net;
using ReliableNetcode;
using UnityEngine;

namespace Client.Net
{
    public class ClientToInstanceConnection : NetcodeClientBehaviour
    {
        public NetcodeClientStatus clientStatus; 
        private EventManager _eventManager = EventManager.instance;
        
        [SerializeField]
        private string connectToken;
        
        
        void Start()
        {
            if (!string.IsNullOrWhiteSpace(connectToken))
            {
                var token = Convert.FromBase64String(connectToken);
                StartClient(token);
                EventManager.Subscribe("SendReliable", SendReliable);
                EventManager.Subscribe("SendUnreliable", SendUnreliable);
            }
            else
            {
                Debug.Log($"No connectToken set");
            }
        }

        public override void OnClientReceiveMessage(byte[] data, int size)
        {
            // After Login -> Auth
            // After Auth -> Load Character
            // After Load Character -> 
            //throw new System.NotImplementedException();
        }

        public override void OnClientConnect()
        {
            // Connected -> Send Auth Request
            var op = SendOPCode(OP.CLIENT_CONNECT);
            SendReliable(op);
        }

        public override void OnClientDisconnect(byte[] data, int size)
        {
            //throw new NotImplementedException();
        }

        public override void OnClientNetworkStatus(NetcodeClientStatus status)
        {
            if (status == clientStatus) return;
            Debug.Log($"{DateTime.Now} [Client] Connection Status: {clientStatus}->{status}");
            clientStatus = status;
            

            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sends data to the Game Server
        /// </summary>
        public void SendReliable(NetworkPacket packet)
        {
            base.Send(packet._data, packet._length, QosType.Reliable);
        }
        
        public void SendUnreliable(NetworkPacket packet)
        {
            base.Send(packet._data, packet._length, QosType.Unreliable);
        }

        
        private void OnDestroy()
        {
            base.OnDestroy();
        }

        private NetworkPacket SendOPCode(OP code)
        {
            using(MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((short)code);            
                return new NetworkPacket(ms.ToArray());
            }
        }
    }
}
using System;
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
            //throw new System.NotImplementedException();
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
        public void SendReliable(BasePacket packet)
        {
            base.Send(packet.buffer, packet.size, QosType.Reliable);
        }
        
        public void SendUnreliable(BasePacket packet)
        {
            base.Send(packet.buffer, packet.size, QosType.Unreliable);
        }

        
        private void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
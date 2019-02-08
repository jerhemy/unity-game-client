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
        private ClientEntityManager _entityManager = ClientEntityManager.instance;
        
        [SerializeField]
        private string connectToken;
        
        
        void Start()
        {
            if (!string.IsNullOrWhiteSpace(connectToken))
            {
                var token = Convert.FromBase64String(connectToken);
                StartClient(token);
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
            var packet = new NetworkPacket(data); 
            Debug.Log($"[{DateTime.Now}] [Client] Received Server Message: {packet.type}");
        
            //EventManager.Publish(packet.type, packet);
        }

        public override void OnClientConnect()
        {
            // Connected -> Send Auth Request
            var op = new NetworkPacket(OP.ClientConnect);
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
            Debug.Log($"[{DateTime.Now}] [Client] Client Message: {packet.type}");
            base.Send(packet.data, packet.length, QosType.Reliable);
        }
        
        public void SendUnreliable(NetworkPacket packet)
        {
            Debug.Log($"[{DateTime.Now}] [Client] Client Message: {packet.type}");
            base.Send(packet.data, packet.length, QosType.Unreliable);
        }
    }
}
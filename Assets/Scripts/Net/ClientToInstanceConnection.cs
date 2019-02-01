using System;
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
        private string privateKey;
        [SerializeField]
        private ulong protocolID;
        
        void Start()
        {
            var token = GenerateToken(protocolID, privateKey, "127.0.0.1", 4000);
            StartClient(token);
            //EventManager.Subscribe("SendReliable", SendReliable);
            //EventManager.Subscribe("SendUnreliable", SendUnreliable);
        }

        public override void OnClientReceiveMessage(byte[] data, int size)
        {
            throw new System.NotImplementedException();
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
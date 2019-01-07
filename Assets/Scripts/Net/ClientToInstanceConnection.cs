using Common;
using Common.Net.Core;
using ReliableNetcode;
using UnityEngine;

namespace Client.Net
{
    public class ClientToInstanceConnection : NetcodeClientBehaviour
    {
        void Start()
        {
            var token = new byte[2048];
            StartClient(token);
            EventManager.Subscribe("SendReliable", SendReliable);
            EventManager.Subscribe("SendUnreliable", SendUnreliable);
        }
        
        public override void OnClientReceiveMessage(byte[] data, int size)
        {
            throw new System.NotImplementedException();
        }

        public override void OnClientNetworkStatus(NetcodeClientStatus status)
        {
            throw new System.NotImplementedException();
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
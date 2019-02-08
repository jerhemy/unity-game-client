using Common;
using UnityEngine;
using Client.Net;
using Net;

namespace Client.Entity
{
    public class Health : MonoBehaviour
    {
        public int currentHP;
        public int maxHP;
        
        void Start()
        {

        }

        private void SendReliable(NetworkPacket packet)

        {
            
        }
        
    }
}
using Common;
using UnityEngine;
using Client.Net;

namespace Client.Entity
{
    public class Health : MonoBehaviour
    {
        public int currentHP;
        public int maxHP;
        
        void Start()
        {
            EventManager.Subscribe("HP_UPDATE", SendReliable);   
        }

        private void SendReliable(BasePacket packet)

        {
            
        }
        
    }
}
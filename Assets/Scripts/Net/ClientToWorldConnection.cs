using Common;
using Common.Net.Core;
using ReliableNetcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Net
{
    public class ClientToWorldConnection : NetcodeClientBehaviour
    {
        void Awake()
        {
            // World connection should always be present
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        void Start()
        {
            var token = new byte[2048];
            StartClient(token);
        }
        
        public override void OnClientReceiveMessage(byte[] data, int size)
        {          
            
            throw new System.NotImplementedException();
        }

        public override void OnClientNetworkStatus(NetcodeClientStatus status)
        {
            throw new System.NotImplementedException();
        }

        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            // We prefix non-connected scenes with '_'
            if (scene.name.StartsWith("_")) {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
using System;
using Common;
using Common.Net.Core;
using Net;
using ReliableNetcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Net
{
    public class ClientToWorldConnection : NetcodeClientBehaviour
    {
        [SerializeField] private bool IsDead;
        [SerializeField] private long SelectedCharacter;
        
        [Header("World Connection Settings")]
        [SerializeField] private string worldIP;
        [SerializeField] private int worldPort;
        [SerializeField] private string privateKey;
        [SerializeField] private ulong protocolID = 1UL;
        
        void Awake()
        {
            // World connection should always be present
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        void Start()
        {
                var token = GenerateToken(protocolID, privateKey, worldIP, worldPort);
                StartClient(token);
        }
        

        public override void OnClientReceiveMessage(byte[] data, int size)
        {
            byte[] payload = new byte[size];

            Array.Copy(data, payload, size);
            
            //throw new System.NotImplementedException();
        }

        public override void OnClientNetworkStatus(NetcodeClientStatus status)
        {
            //throw new System.NotImplementedException();
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
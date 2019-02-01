using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Text;
using Common.Net.Core;
using Net;
using NetcodeIO.NET;
using ReliableNetcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityNetcodeIO;

namespace Common
{
    public abstract class NetcodeClientBehaviour : MonoBehaviour
    {
		private NetcodeClient client;
	    private ReliableEndpoint endpoint;
	       
	    public abstract void OnClientReceiveMessage(byte[] data, int size);
	    public abstract void OnClientNetworkStatus(NetcodeClientStatus status);
	    
	    protected void StartClient(byte[] connectToken)
	    {
		    UnityNetcode.QuerySupport((supportStatus) =>
		    {
			    if (supportStatus == NetcodeIOSupportStatus.Available)
			    {
				    UnityNetcode.CreateClient(NetcodeIOClientProtocol.IPv4, (client) =>
				    {
					    this.client = client;
					    endpoint = new ReliableEndpoint();
					    
					    client.Connect(connectToken, () =>
					    {
						    // add listener for network messages
						    client.AddPayloadListener(ReceivePacket);

						    // do stuff
						    StartCoroutine(StatusUpdate());
					    }, (err) =>
					    {

					    });
				    });
			    }
			    else if (supportStatus == NetcodeIOSupportStatus.Unavailable)
			    {
				    //logLine("Netcode.IO not available");
			    }
			    else if (supportStatus == NetcodeIOSupportStatus.HelperNotInstalled)
			    {
				    //logLine("Netcode.IO is available, but native helper is not installed");
			    }
		    });		    
	    }
		
	    private void ReceivePacket(NetcodeClient client, NetcodePacket packet)
	    {	  
		    endpoint.ReceiveCallback = (data, size) =>
		    {
			    
			    OnClientReceiveMessage(data, size);
		    };
		    endpoint.ReceivePacket(packet.PacketBuffer.InternalBuffer, packet.PacketBuffer.Length);
	    }

	    /// <summary>
	    /// Sends data to the Game Server
	    /// </summary>
	    
	    public void Send(byte[] payload, int size, QosType type)
	    {
		    endpoint.TransmitCallback = ( data, length ) =>
		    {
			    client.Send( data, length );
		    };

		    endpoint.SendMessage(payload, size, type);
	    }

		IEnumerator StatusUpdate()
		{
			while (true)
			{
				client.QueryStatus(OnClientNetworkStatus);
				endpoint.Update();
				yield return new WaitForSeconds(0.5f);
			}
		}

		public virtual void OnDestroy()
		{
			DestroyConnection();
		}

	    private void DestroyConnection()
	    {
		    if (client != null)
			    UnityNetcode.DestroyClient(client);
	    }

	    public byte[] GenerateToken(ulong protocolID, string serverKey, string ipAddress, int port)
	    {
		    ulong sequenceNumber = ulong.Parse(DateTime.Now.ToString("hhmmssffffff"));
		    Debug.Log($"Sequence #: {sequenceNumber}");
		    var pkey = serverKey.Substring(0, 16);
		    byte[] privateKey = Encoding.ASCII.GetBytes(pkey);
		    Debug.Log($"PrivateKey Length: {privateKey.Length}");
		    
//			var privateKey = new byte[]
//			{
//				0x60, 0x6a, 0xbe, 0x6e, 0xc9, 0x19, 0x10, 0xea,
//				0x9a, 0x65, 0x62, 0xf6, 0x6f, 0x2b, 0x30, 0xe4,
//				0x43, 0x71, 0xd6, 0x2c, 0xd1, 0x99, 0x27, 0x26,
//				0x6b, 0x3c, 0x60, 0xf4, 0xb7, 0x15, 0xab, 0xa1
//			};
	    
		    var worldIP = new IPEndPoint(IPAddress.Parse(ipAddress), port);	    
		    IPEndPoint[] addressList = {worldIP}; 
		    
		    TokenFactory tokenFactory = new TokenFactory(
			    protocolID,		// must be the same protocol ID as passed to both client and server constructors
			    privateKey		// byte[32], must be the same as the private key passed to the Server constructor
		    );

		    const ulong clientID = 1UL;
		    var userData = new byte[256];
		    
		    // ClientID will be AccountID as only clients will be connecting to the World Server
		    return tokenFactory.GenerateConnectToken(
			    addressList,		// IPEndPoint[] list of addresses the client can connect to. Must have at least one and no more than 32.
			    30,		// in how many seconds will the token expire
			    30,		// how long it takes until a connection attempt times out and the client tries the next server.
			    1UL,		// ulong token sequence number used to uniquely identify a connect token.
			    1UL,		// ulong ID used to uniquely identify this client
			    userData		// byte[], up to 256 bytes of arbitrary user data (available to the server as RemoteClient.UserData)
		    );

	    }
    }
}
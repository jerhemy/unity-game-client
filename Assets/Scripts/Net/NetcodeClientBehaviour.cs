using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Text;
using Common.Net.Core;
using NetcodeIO.NET;
using ReliableNetcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityNetcodeIO;

namespace Common
{
	public struct BasePacket
	{
		public short type;
		public byte[] data;	
	}
	
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
		    endpoint.ReceiveCallback = OnClientReceiveMessage;
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

	    private byte[] GenerateToken(ulong protocolID, string serverKey, string ipAddress, int port)
	    {
		    var sequenceNumber = ulong.Parse(DateTime.Now.ToString("hhmmssffffff"));
		    var privateKey = Encoding.ASCII.GetBytes(serverKey);

		    var worldIP = new IPEndPoint(IPAddress.Parse(ipAddress), port);	    
		    IPEndPoint[] addressList = {worldIP}; 
		    
		    TokenFactory tokenFactory = new TokenFactory(
			    protocolID,		// must be the same protocol ID as passed to both client and server constructors
			    privateKey		// byte[32], must be the same as the private key passed to the Server constructor
		    );

		    const ulong clientID = 0UL;
		    var userData = new byte[256];
		    
		    return tokenFactory.GenerateConnectToken(
			    addressList,		// IPEndPoint[] list of addresses the client can connect to. Must have at least one and no more than 32.
			    30,		// in how many seconds will the token expire
			    30,		// how long it takes until a connection attempt times out and the client tries the next server.
			    sequenceNumber,		// ulong token sequence number used to uniquely identify a connect token.
			    clientID,		// ulong ID used to uniquely identify this client
			    userData		// byte[], up to 256 bytes of arbitrary user data (available to the server as RemoteClient.UserData)
		    );

	    }
    }
}
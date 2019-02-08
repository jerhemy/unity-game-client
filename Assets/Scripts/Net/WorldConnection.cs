using System.Collections;
using System.Collections.Generic;
using Common.Net.Core;
using ReliableNetcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using UnityNetcodeIO;

namespace Client.Net
{
	public class WorldConnection : MonoBehaviour
	{
		public Text outputText;
		public Text StatusText;
		public Text SentText;
		public Text ReceivedText;

		public string TokenServer = "http://127.0.0.1:8080/token";

		protected NetcodeClient client;
		protected ReliableEndpoint reliableEndpoint;

		private void Start()
		{
			logLine("Checking for Netcode.IO support...");

			UnityNetcode.QuerySupport((supportStatus) =>
			{
				if (supportStatus == NetcodeIOSupportStatus.Available)
				{
					logLine("Netcode.IO available and ready!");

					UnityNetcode.CreateClient(NetcodeIOClientProtocol.IPv4, (client) =>
					{
						this.client = client;
						this.reliableEndpoint = new ReliableEndpoint();
						StartCoroutine(connectToServer());
					});
				}
				else if (supportStatus == NetcodeIOSupportStatus.Unavailable)
				{
					logLine("Netcode.IO not available");
				}
				else if (supportStatus == NetcodeIOSupportStatus.HelperNotInstalled)
				{
					logLine("Netcode.IO is available, but native helper is not installed");
				}
			});
		}

		IEnumerator connectToServer()
		{
			logLine("Obtaining connect token...");
			UnityWebRequest webRequest = UnityWebRequest.Get(TokenServer);
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError)
			{
				logLine("Failed to obtain connect token: " + webRequest.error);
				yield break;
			}

			byte[] connectToken = System.Convert.FromBase64String(webRequest.downloadHandler.text);
			client.Connect(connectToken, () =>
			{
				logLine("Connected to netcode.io server!");

				// add listener for network messages
				client.AddPayloadListener(ReceivePacket);

				// do stuff
				StartCoroutine(updateStatus());
				StartCoroutine(doStuff());
			}, (err) => { logLine("Failed to connect: " + err); });
		}

		int received = 0;

		void ReceivePacket(NetcodeClient client, NetcodePacket packet)
		{

			received++;
			ReceivedText.text = "Received: " + received;
		}

//		public void SendPacket(OpCode type)
//		{
//
//		}

		IEnumerator updateStatus()
		{
			while (true)
			{
				client.QueryStatus((status) => { StatusText.text = status.ToString(); });

				yield return new WaitForSeconds(0.1f);
			}
		}

		IEnumerator doStuff()
		{
			int sent = 0;

			while (true)
			{
				if (client.Status == NetcodeClientStatus.Connected)
				{
					// send a packet
					var packetStr = "pkt " + sent + "! " + System.DateTime.Now.ToString();
					sent++;
					SentText.text = "Sent: " + sent;

					var packetBuffer = System.Text.Encoding.ASCII.GetBytes(packetStr);

					client.Send(packetBuffer);
				}

				yield return new WaitForSeconds(0.25f);
			}
		}

		private void OnDestroy()
		{
			if (client != null)
				UnityNetcode.DestroyClient(client);
		}

		protected void log(string text)
		{
			outputText.text += text;
		}

		protected void logLine(string text)
		{
			log(text + "\n");
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using Client.Net;
using Net;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static readonly Dictionary<OP, Action<NetworkPacket>> eventDictionary = new Dictionary<OP, Action<NetworkPacket>>();
    
    public static void Subscribe(OP eventName, Action<NetworkPacket> listener)
    {
        Action<NetworkPacket> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void Unsubscribe(OP eventName, Action<NetworkPacket> listener)
    {
        Action<NetworkPacket> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            eventDictionary[eventName] = thisEvent;
        }
    }

    public static void Publish(OP eventName, NetworkPacket eventParam)
    {
        Action<NetworkPacket> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventParam);
            // OR USE  instance.eventDictionary[eventName](eventParam);
        }
    }
}
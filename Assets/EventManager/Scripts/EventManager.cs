using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace pkm.EventManager {
    public class EventManager : MonoBehaviour {
        private Dictionary<string, Action<dynamic>> eventDict;

        private static EventManager eventManager;

        public static EventManager instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDict == null)
            {
                eventDict = new Dictionary<string, Action<dynamic>>();
            }
        }

        public static void StartListening(string eventName, Action<dynamic> act)
        {
            Action<dynamic> item;
            if (instance.eventDict.TryGetValue(eventName, out item))
            {
                instance.eventDict[eventName] += act;
            }
            else
            {
                instance.eventDict.Add(eventName, item);
                instance.eventDict[eventName] += act;
            }
        }

        public static void StopListening(string eventName, Action<dynamic> act)
        {
            if (eventManager == null) return;

            Action<dynamic> item;
            if (instance.eventDict.TryGetValue(eventName, out item))
            {
                instance.eventDict[eventName] -= act;
            }
        }

        public static void TriggerEvent(string evt, object obj)
        {
            Action<dynamic> item;

            if (instance.eventDict.TryGetValue(evt, out item))
            {
                item.Invoke(obj);
            }
            
        }
    }
}
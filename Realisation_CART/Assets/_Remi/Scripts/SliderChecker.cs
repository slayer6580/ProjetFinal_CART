using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DiscountDelirium
{
    public class SliderChecker : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("Checking slider for event listeners...");
            if (GetComponent<Slider>().onValueChanged.GetPersistentTarget(0) == null) Debug.LogError("Slider has no OnValueChanged event set up!");

            //if (GetComponent<EventTrigger>().GetPersistentTarget(0) == null) Debug.LogError("Slider has no EventTrigger set up!");

            EventTrigger eventTrigger = GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                Debug.LogError("No EventTrigger component found!");
                return;
            }

            bool hasListeners = false;
            foreach (EventTrigger.Entry entry in eventTrigger.triggers)
            {
                if (entry.callback.GetPersistentEventCount() > 0)
                {
                    hasListeners = true;
                    break;
                }
            }

            if (!hasListeners)
            {
                Debug.LogError("EventTrigger has no persistent listeners set up!");
            }
            else
            {
                Debug.Log("EventTrigger has persistent listeners set up.");
            }

            this.enabled = false;
        }

        private void OnEnable()
        {
            Debug.Log("SliderChecker enabled.");
        }

        private void OnDisable()
        {
            Debug.Log("SliderChecker disabled.");
        }
    }
}

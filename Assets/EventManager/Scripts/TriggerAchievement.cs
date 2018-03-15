using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class TriggerAchievement : MonoBehaviour {
    
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
            EventManager.TriggerEvent("playerMove", new {name = "Foobar"} );
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            EventManager.TriggerEvent("openInventory", new {time = 1 } );
        }
    }
}

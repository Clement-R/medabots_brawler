using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour {
    public GameObject achievementPanel;
    Text panelText;

	void Start ()
    {
        panelText = achievementPanel.transform.GetChild(0).GetComponent<Text>();
    }
	
    void OnEnable()
    {
        /*
        EventManager.StartListening("playerMove", OnPlayerMove);
        EventManager.StartListening("openInventory", OnOpenInventory);
        */

        EventManager.StartListening("playerMove", OnPlayerMove);
        EventManager.StartListening("openInventory", OnOpenInventory);
    }

    void OnDisable()
    {
        /*
        EventManager.StopListening("playerMove", OnPlayerMove);
        EventManager.StopListening("openInventory", OnOpenInventory);
        */

        EventManager.StopListening("playerMove", OnPlayerMove);
        EventManager.StopListening("openInventory", OnOpenInventory);
    }

    void OnOpenInventory(dynamic obj)
    {
        Debug.Log("A player opened its inventory !");
        UnlockAchievement("Inventory opened " + obj.time);
    }

    void OnPlayerMove(dynamic obj)
    {
        Debug.Log("A player moved !");
        UnlockAchievement("You made it " + obj.name + " !");
    }

    void UnlockAchievement(string text)
    {
        achievementPanel.SetActive(true);
        panelText.text = text;
        StartCoroutine("hidePanel");
    }

    IEnumerator hidePanel()
    {
        yield return new WaitForSeconds(2.0f);
        achievementPanel.SetActive(false);
    }
}

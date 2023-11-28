using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject mapPanel, charPanel, weaponPanel;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        OpenMapPanel();
    }

    public void OpenCharPanel()
    {

         
        charPanel.SetActive(true);
        mapPanel.SetActive(false);
        weaponPanel.SetActive(false);
    }
    public void OpenMapPanel()
    {
        // Set level text
        int currentLevel = PlayerPrefs.GetInt("level", 0);
        mapPanel.transform.Find("LevelTx").GetComponent<Text>().text = (currentLevel+1).ToString() + ". " + gameManager.levels[currentLevel].name;

        // Set stage text
        int currentStage = PlayerPrefs.GetInt("levelStage", 0);
        mapPanel.transform.Find("StageTx").GetComponent<Text>().text = "STAGE: " + (currentStage + 1).ToString() + "/" + gameManager.levels[currentLevel].stageCount;

        // Set UI background
        Instantiate(gameManager.levels[currentLevel].bgPrefab, Vector3.zero, Quaternion.identity, mapPanel.transform.Find("Bg"));

        // Set button conditions
        mapPanel.transform.Find("PreBt").GetComponent<Button>().interactable = currentLevel > 0 ? true : false;
        mapPanel.transform.Find("NextBt").GetComponent<Button>().interactable = currentLevel < gameManager.levels.Length - 2 ? true : false;

        // Open panel and close other panels
        charPanel.SetActive(false);
        mapPanel.SetActive(true);
        weaponPanel.SetActive(false);
    }
    public void OpenWeaponPanel()
    {


        charPanel.SetActive(false);
        mapPanel.SetActive(false);
        weaponPanel.SetActive(true);
    }
}

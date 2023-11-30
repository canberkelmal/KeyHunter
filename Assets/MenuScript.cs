using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuScript : MonoBehaviour
{
    public GameObject mapPanel, charPanel, weaponPanel;
    public GameObject mapButton, charButton, weaponButton;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        OpenMapPanel();
    }

    public void OpenCharPanel()
    {

        charButton.transform.localScale = Vector3.one * 1.3f;
        mapButton.transform.localScale = Vector3.one;
        weaponButton.transform.localScale = Vector3.one;

        charButton.transform.Find("Selected").gameObject.SetActive(true);
        mapButton.transform.Find("Selected").gameObject.SetActive(false);
        weaponButton.transform.Find("Selected").gameObject.SetActive(false);
         
        charPanel.SetActive(true);
        mapPanel.SetActive(false);
        weaponPanel.SetActive(false);
    }
    public void OpenMapPanel()
    {
        LoadLevelUI();

        charButton.transform.localScale = Vector3.one;
        mapButton.transform.localScale = Vector3.one * 1.3f;
        weaponButton.transform.localScale = Vector3.one;

        charButton.transform.Find("Selected").gameObject.SetActive(false);
        mapButton.transform.Find("Selected").gameObject.SetActive(true);
        weaponButton.transform.Find("Selected").gameObject.SetActive(false);
        // Open panel and close other panels
        charPanel.SetActive(false);
        mapPanel.SetActive(true);
        weaponPanel.SetActive(false);
    }
    public void LoadLevelUI()
    {
        // Set level text
        int currentLevel = PlayerPrefs.GetInt("level", 0);
        mapPanel.transform.Find("LevelTx").GetComponent<Text>().text = (currentLevel + 1).ToString() + ". " + gameManager.levels[currentLevel].levelName;

        // Set stage text
        int currentStage = PlayerPrefs.GetInt("levelStage", 0);
        mapPanel.transform.Find("StageTx").GetComponent<Text>().text = "STAGE: " + (currentStage + 1).ToString() + "/" + gameManager.levels[currentLevel].stageCount;

        // Set UI background
        Instantiate(gameManager.levels[currentLevel].bgPrefab, mapPanel.transform.Find("Bg"));

        // Set button conditions
        //mapPanel.transform.Find("PreBt").GetComponent<Button>().interactable = currentLevel > 0 ? true : false;
        //mapPanel.transform.Find("NextBt").GetComponent<Button>().interactable = currentLevel < gameManager.levels.Length - 2 ? true : false;
    }
    public void PreLvlButton()
    {
        int targetLevel = PlayerPrefs.GetInt("level", 0) - 1;

        if(targetLevel >= 0 && targetLevel <= gameManager.levels.Length - 1)
        {
            PlayerPrefs.SetInt("level", targetLevel);
            LoadLevelUI();
        }

    }
    public void NextLvlButton()
    {
        int targetLevel = PlayerPrefs.GetInt("level", 0) + 1;

        if (targetLevel >= 0 && targetLevel <= gameManager.levels.Length - 1)
        {
            PlayerPrefs.SetInt("level", targetLevel);
            LoadLevelUI();
        }
    }
    public void StartButton()
    {
        gameManager.InitLevel();
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        gameObject.GetComponent<CanvasGroup>().DOFade(0, gameManager.UIFadeTime / 2).OnComplete(CloseMenuPanel);        
    }

    public void CloseMenuPanel()
    {
        gameObject.SetActive(false);
    }


    public void OpenWeaponPanel()
    {
        charButton.transform.localScale = Vector3.one;
        mapButton.transform.localScale = Vector3.one;
        weaponButton.transform.localScale = Vector3.one * 1.3f;

        charButton.transform.Find("Selected").gameObject.SetActive(false);
        mapButton.transform.Find("Selected").gameObject.SetActive(false);
        weaponButton.transform.Find("Selected").gameObject.SetActive(true);

        charPanel.SetActive(false);
        mapPanel.SetActive(false);
        weaponPanel.SetActive(true);
    }
}

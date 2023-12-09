using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class MenuScript : MonoBehaviour
{
    public GameObject mapPanel, charPanel, weaponPanel, weaponUpgradingPanel;
    public GameObject mapButton, charButton, weaponButton;
    public int upgCoinCost = 15;
    public int upgCrossCost = 10;

    GameManager gameManager;
    Weapon upgradingWeapon;
    float maxDamage = 0;
    float maxRange = 0;
    float minSpeed = 9999999;
    float maxSpeed = 0;

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
        weaponUpgradingPanel.SetActive(false);
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
        weaponUpgradingPanel.SetActive(false);
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

        int limit = weaponPanel.transform.Find("Weapons").childCount > gameManager.weapons.Count ? gameManager.weapons.Count : weaponPanel.transform.Find("Weapons").childCount;

        for(int i = 0; i < limit; i++)
        {
            weaponPanel.transform.Find("Weapons").GetChild(i).GetComponent<WeaponMenuUISc>().SetWeapon(gameManager.weapons[i]);
        }

        charPanel.SetActive(false);
        mapPanel.SetActive(false);
        weaponPanel.SetActive(true);
        weaponUpgradingPanel.SetActive(false);

        foreach(Weapon wp in gameManager.weapons)
        {
            if(wp.maxDamage > maxDamage)
            {
                maxDamage = wp.maxDamage;
            }
            if(wp.maxRange > maxRange)
            {
                maxRange = wp.maxRange;
            }
            if(wp.maxAttackSpeed > maxSpeed)
            {
                maxSpeed = wp.maxAttackSpeed;
            }
            if(wp.minAttackSpeed < minSpeed)
            {
                minSpeed = wp.minAttackSpeed;
            }
        }  
    }
     
    public void OpenWeaponUpgradePanel(Weapon UIWeapon)
    {
        upgradingWeapon = UIWeapon;
        SetWeaponUpgradePanelUI();

        weaponUpgradingPanel.transform.Find("DamageBar").Find("LoadBar").Find("RedFill").GetComponent<Image>().fillAmount = (maxDamage - upgradingWeapon.maxDamage) / maxDamage;
        weaponUpgradingPanel.transform.Find("RangeBar").Find("LoadBar").Find("RedFill").GetComponent<Image>().fillAmount =  (maxRange - upgradingWeapon.maxRange) / maxRange;
        weaponUpgradingPanel.transform.Find("AttackSpeedBar").Find("LoadBar").Find("RedFill").GetComponent<Image>().fillAmount = (upgradingWeapon.minAttackSpeed - minSpeed) / (maxSpeed-minSpeed);

        weaponUpgradingPanel.SetActive(true);
    }

    public void SetWeaponUpgradePanelUI()
    {
        Transform panelBg = weaponUpgradingPanel.transform.Find("PanelBg");

        panelBg.Find("Title").GetComponent<Text>().text = upgradingWeapon.weaponName;
        panelBg.Find("WeaponUI").GetComponent<Image>().sprite = upgradingWeapon.icon;

        SetWeaponUpgradePanelBars();
    }
    public void SetWeaponUpgradePanelBars()
    {
        Transform upgradingPanel = weaponUpgradingPanel.transform;
        upgradingPanel.Find("DamageBar").Find("LoadBar").GetComponent<LoadBarSc>().SetFillAmount(upgradingWeapon.Damage() / maxDamage, upgradingWeapon.NextDamage() / maxDamage, false);
        upgradingPanel.Find("RangeBar").Find("LoadBar").GetComponent<LoadBarSc>().SetFillAmount(upgradingWeapon.Range() / maxRange, upgradingWeapon.NextRange() / maxRange, false);
        
        float amoS = 1 - (upgradingWeapon.AttackSpeed() - minSpeed) / (maxSpeed - minSpeed);
        float amoSF = 1 - (upgradingWeapon.NextAttackSpeed() - minSpeed) / (maxSpeed - minSpeed);

        upgradingPanel.Find("AttackSpeedBar").Find("LoadBar").GetComponent<LoadBarSc>().SetFillAmount(amoS, amoSF, false);
        upgradingPanel.Find("LevelTx").GetComponent<Text>().text = "Lv. " + upgradingWeapon.GetLevel().ToString() + "/" + gameManager.weaponMaxLevel.ToString();
        SetButtonStatu();
    }
    public void SetButtonStatu()
    {
        if(upgradingWeapon.GetLevel() < gameManager.weaponMaxLevel && gameManager.coinAmount > upgCoinCost && gameManager.crossAmount > upgCrossCost)
        {
            weaponUpgradingPanel.transform.Find("UpgradeButton").GetComponent<Button>().interactable = true;
        }
        else
        {
            weaponUpgradingPanel.transform.Find("UpgradeButton").GetComponent<Button>().interactable = false;
        }
    }
    public void WeaponUpgradeButton()
    {
        upgradingWeapon.Upgrade();
        gameManager.SetCoinAmount(-upgCoinCost);
        gameManager.SetCrossAmount(-upgCrossCost);

        SetWeaponUpgradePanelBars();

        int limit = weaponPanel.transform.Find("Weapons").childCount > gameManager.weapons.Count ? gameManager.weapons.Count : weaponPanel.transform.Find("Weapons").childCount;

        for (int i = 0; i < limit; i++)
        {
            weaponPanel.transform.Find("Weapons").GetChild(i).GetComponent<WeaponMenuUISc>().SetWeapon(gameManager.weapons[i]);
        }
    }
    public void CloseWeaponUpgradePanel()
    {
        weaponUpgradingPanel.SetActive(false);
    }
}

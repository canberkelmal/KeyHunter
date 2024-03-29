using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Buff;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerController;
    public Transform levelsParent, collectablesParent;
    public LayerMask attackableLayerMask, defaultLayerMask, blockAttackLayerMask, deathLayerMask;
    public Material attackedMat;
    public List<Weapon> weapons;
    public List<Buff> buffs;
    public List<GameObject> collectables;
    public float baseAttackSpeedMultiplier = 1f;
    public float baseDamageMultiplier = 1f;
    public Text coinText, crossText;
    public int weaponMaxLevel = 10;
    public int coinAmount = 0;
    public int crossAmount = 0; 
    public float dropHeight = 0.5f;
    public float healBuffAmount = 20f;
    public Level[] levels;
    public GameObject[] levelPrefabs;
    public GameObject coinUIPrefab, crossUIPrefab, keyUIPrefab;
    public GameObject levelBuffUI, lockedTx, chooseWeaponUI, continueUI, settingsUI, menuWarningUI, menuPanel, stageFadePanel, failPanel, keyUI, takeParticle, getKeyParticle;
    

    public Text levelStageText;
    public float UIFadeTime = 0.7f;

    List<GateSc> gateScs = new List<GateSc>();
    CameraController camController;
    int currentLevel, currentStage;
    Weapon selectedWeapon;
    Buff buffUI1, buffUI2;
    GameObject finalGate;
    public int enemyCount = 0;
    bool hasKey = false;
    bool isKeyLevel = false;
    bool isBossLevel = false;
    public bool isContinue = false;

    private void Start()
    {
        isContinue = PlayerPrefs.GetInt("Continue", 0) > 0 ? true : false;
        continueUI.SetActive(isContinue); 
        camController = Camera.main.transform.GetComponent<CameraController>();
        deathLayerMask = LayerMask.NameToLayer("DeathEnemy");
        defaultLayerMask = LayerMask.NameToLayer("Default");

        // Set UI elements
        SetCoinAmount(0);
        SetCrossAmount(0);

        playerController.SetController(false);
        StageFadeOut();
        menuPanel.SetActive(true);
        //OpenMenu();
        foreach (Transform clv in levelsParent.transform)
        {
            Destroy(clv.gameObject);
        }
    }

    public void ContinueUI(bool ind)
    {
        continueUI.SetActive(false);
        if(ind)
        {
            menuPanel.GetComponent<MenuScript>().StartButton();
        }
        else
        {
            CloseMenuWarningUI();
            CloseSettings();
            PlayerPrefs.SetInt("Continue", 0);
            PlayerPrefs.SetInt("levelStage", 0);
            StageFadeOut();
            ClearLevelValues();
            Debug.Log("Level values cleared");
            menuPanel.SetActive(true);

            foreach (Transform clv in levelsParent.transform)
            {
                Destroy(clv.gameObject);
            }
            menuPanel.GetComponent<CanvasGroup>().alpha = 1f;
            menuPanel.GetComponent<MenuScript>().Start();
        }
    }

    public void InitLevel()
    {
        ResetGates();
        Time.timeScale = 1f;
        hasKey = false;
        PlayerPrefs.SetInt("Continue", 1);
        baseAttackSpeedMultiplier = PlayerPrefs.GetFloat("AttackSpeedBuff", 1);
        baseDamageMultiplier = PlayerPrefs.GetFloat("DamageBuff", 1);
        isBossLevel = false;
        hasKey = false;
        isKeyLevel = false;
        if(keyUI.transform.childCount > 0)
        {
            foreach(Transform a in keyUI.transform)
            {
                Destroy (a.gameObject);
            }
        }
        keyUI.GetComponent<Image>().color = Color.white;
        keyUI.GetComponent<Image>().enabled = false;
        keyUI.SetActive(false);
        stageFadePanel.GetComponent<CanvasGroup>().alpha = 1f;
        stageFadePanel.SetActive(true);
        stageFadePanel.GetComponent<CanvasGroup>().DOFade(0, UIFadeTime).OnComplete(StageLoaded);

        // Load level

        currentLevel = PlayerPrefs.GetInt("level", 0);
        currentStage = PlayerPrefs.GetInt("levelStage", 0);

        if (currentStage == 0)
        {
            OpenChooseWeaponPanel();
            playerController.Heal(playerController.maxHealth);
        }
        else
            playerController.SetController(true);

        levelStageText.transform.parent.Find("LevelTx").GetComponent<Text>().text = "Level " + (currentLevel+1).ToString();
        levelStageText.text = (currentStage + 1).ToString() + "/" + (levels[currentLevel].stageCount).ToString();
        levelStageText.transform.parent.gameObject.SetActive(true);

        foreach (Transform clv in levelsParent.transform)
        {
            Destroy(clv.gameObject);
        }

        GameObject levelPrefab = levels[currentLevel].levelPrefab.transform.GetChild(currentStage).gameObject; 
        GameObject spawnedLevel = Instantiate(levelPrefab, levelsParent);

        // Set player position and weapon
        player.transform.position = spawnedLevel.transform.Find("PlayerSpawnPoint").position;
        if(playerController.isDeath)
        {
            playerController.isDeath = false;
            playerController.Heal(playerController.maxHealth);
            player.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Start");
        }
        else
        {
            playerController.currentHealth = PlayerPrefs.GetFloat("PlayerHp", playerController.maxHealth);
            playerController.SetHp();
            player.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Start");
        }
        enemyCount = spawnedLevel.transform.Find("Enemies").childCount;
        SetWeapon();
    }
    public void StageFadeOut()
    {
        stageFadePanel.GetComponent<CanvasGroup>().alpha = 1f;
        stageFadePanel.SetActive(true);
        stageFadePanel.GetComponent<CanvasGroup>().DOFade(0, UIFadeTime).OnComplete(StageLoaded);
    }
    void StageLoaded()
    {
        stageFadePanel.SetActive(false);
    }
    public void SetBossLevel()
    {
        isBossLevel = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void ShakeCam()
    {
        camController.ShakeCam();
    }

    public void ShakeCamWithPower(float power)
    {
        camController.ShakeCamWithPower(power);
    }

    public void OpenMenu()
    {
        stageFadePanel.GetComponent<CanvasGroup>().alpha = 0f;
        stageFadePanel.SetActive(false);
        menuPanel.GetComponent<CanvasGroup>().alpha = 1f;
        menuPanel.SetActive(true);
        menuPanel.GetComponent<MenuScript>().OpenMapPanel();
        foreach (Transform clv in levelsParent.transform)
        {
            Destroy(clv.gameObject);
        }
        failPanel.SetActive(false);
        levelBuffUI.SetActive(false);
        chooseWeaponUI.SetActive(false);

        ClearLevelValues();
    }

    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddToGates(GateSc addedSc)
    {
        gateScs.Add(addedSc);
    }

    public void LockedTx()
    {
        //lockedTx.GetComponent<LockedTxSc>().ShowTx();
        lockedTx.SetActive(true);
    }

    public void ResetGates()
    {
        gateScs = new List<GateSc>();
    }

    public void SetFinalGate(GameObject gate)
    {
        finalGate = gate;
        CheckForFinalGate();
    }

    public void SetKeyLevel(bool isKeyed)
    {
        isKeyLevel = isKeyed;
        CheckForFinalGate();
    }

    public void GetKey()
    {
        hasKey = true;
        CheckForFinalGate();
        //Invoke("OpenGates", 1);
        OpenGates();
        keyUI.SetActive(true);
    }

    public void OpenGates()
    {
        if (gateScs.Count > 0)
        {
            foreach (GateSc g in gateScs)
            {
                g.SetGateAvailable(true);
            }
        }
    }

    public void LoadPlayerToPlatform(GameObject platform)
    {
        playerController.LoadToPlatform(platform);
    }
    public void UnloadPlayerFromPlatform()
    {
        playerController.UnloadFromPlatform();
    }



    public void EnemyDeath()
    {
        ShakeCam();
        enemyCount--;
        if (enemyCount <= 0)
        {
            CheckForFinalGate();
            AllEnemiesDeath();
        }
    }

    public void CheckForFinalGate()
    {
        bool keyStatu = true;
        if(isKeyLevel && !hasKey)
        {
            keyStatu = false;
        }

        bool enemyStatu = enemyCount <= 0 ? true : false;

        if(finalGate != null)
        {
            finalGate.GetComponent<FinalGateScript>().SetGateAvailable(keyStatu, enemyStatu);
        }
    }

    public void AllEnemiesDeath()
    {
        foreach (Transform collectable in collectablesParent)
        {
            collectable.GetComponent<CollectableSc>().MoveToPlayer(player.transform);
        }
    }

    public void SetWeaponPref(string weaponName)
    {
        PlayerPrefs.SetString("WeaponName", weaponName); 
        SetWeapon();
    }

    public void SetWeapon()
    {
        // Remove current weapon from player
        foreach (Transform currentWeapon in playerController.weaponPoint.transform)
        {
            Destroy(currentWeapon.gameObject);
        }

        // Find the desired weapon
        selectedWeapon = GetWeapon(PlayerPrefs.GetString("WeaponName", "Blade"));

        playerController.SetWeapon(selectedWeapon);
    }

    public void OpenChooseWeaponPanel()
    {
        playerController.SetController(false);

        // Panel fade in
        chooseWeaponUI.GetComponent<CanvasGroup>().alpha = 0f;
        chooseWeaponUI.GetComponent<RectTransform>().transform.localPosition = Vector3.up * 250;
        chooseWeaponUI.SetActive(true);
        chooseWeaponUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), UIFadeTime, false).SetEase(Ease.OutElastic);
        chooseWeaponUI.GetComponent<CanvasGroup>().DOFade(1, UIFadeTime / 2);
    }

    public void WeaponButton(string weaponName)
    {
        SetWeaponPref(weaponName);
        CloseChooseWeaponPanel();
    }
    public void CloseChooseWeaponPanel()
    {
        playerController.SetController(true);

        chooseWeaponUI.GetComponent<CanvasGroup>().alpha = 1f;
        chooseWeaponUI.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
        chooseWeaponUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 250f), UIFadeTime, false).SetEase(Ease.InOutQuint).OnComplete(ChooseWeaponPanelFadedOuted);
        chooseWeaponUI.GetComponent<CanvasGroup>().DOFade(0, UIFadeTime);
    }
    public void ChooseWeaponPanelFadedOuted()
    {
        chooseWeaponUI.SetActive(false);
        playerController.SetController(true);
    }

    public Weapon GetWeapon(string weaponName)
    {
        Weapon retWeapon = null;
        foreach (var weapon in weapons)
        {
            if (weapon.name == weaponName)
            {
                retWeapon = weapon;
            }
        }
        return retWeapon;
    }

    public void SetCoinAmount(int addAmount)
    {
        coinAmount = PlayerPrefs.GetInt("CoinAmount", 0);
        coinAmount += addAmount;
        PlayerPrefs.SetInt("CoinAmount", coinAmount);
        coinText.text = coinAmount.ToString();
    }

    public void SetCrossAmount(int addAmount)
    {
        crossAmount = PlayerPrefs.GetInt("CrossAmount", 0); 
        crossAmount += addAmount;
        PlayerPrefs.SetInt("CrossAmount", crossAmount);
        crossText.text = crossAmount.ToString();
    }
    public void SetBuff(Buff buff1, Buff buff2)
    {
        buffUI1 = buff1;
        buffUI2 = buff2;

        playerController.SetController(false);

        levelBuffUI.transform.Find("Buff1").Find("UISprite").GetComponent<Image>().sprite = buff1.uISprite;
        levelBuffUI.transform.Find("Buff1").Find("Title").GetComponent<Text>().text = buff1.buffName;

        levelBuffUI.transform.Find("Buff2").Find("UISprite").GetComponent<Image>().sprite = buff2.uISprite;
        levelBuffUI.transform.Find("Buff2").Find("Title").GetComponent<Text>().text = buff2.buffName;

        // Panel fade in
        levelBuffUI.GetComponent<CanvasGroup>().alpha = 0f;
        levelBuffUI.GetComponent<RectTransform>().transform.localPosition = Vector3.up * 250;
        levelBuffUI.SetActive(true);
        levelBuffUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), UIFadeTime, false).SetEase(Ease.OutElastic);
        levelBuffUI.GetComponent<CanvasGroup>().DOFade(1,UIFadeTime/2);
    }

    public void Buff1Button()
    {
        //buffUI1.BuffEffect();
        TakeBuff(buffUI1.type);
        CloseBuffPanel();
    }
    public void Buff2Button()
    {
        //buffUI2.BuffEffect();
        TakeBuff(buffUI2.type);
        CloseBuffPanel();
    }

    public void TakeBuff(Buff.BuffTypes a)
    {
        switch (a)
        {
            case BuffTypes.attackSpeed:
                baseAttackSpeedMultiplier = PlayerPrefs.GetFloat("AttackSpeedBuff", 1) / 1.15f;
                PlayerPrefs.SetFloat("AttackSpeedBuff", baseAttackSpeedMultiplier);
                playerController.SetAttackSpeed();
                break;
            case BuffTypes.doubleShot:

                break;

            case BuffTypes.damage:

                baseDamageMultiplier = PlayerPrefs.GetFloat("DamageBuff", 1) * 1.15f;
                PlayerPrefs.SetFloat("DamageBuff", baseDamageMultiplier);
                playerController.SetDamage();

                break;
            case BuffTypes.heal:
                playerController.Heal(healBuffAmount);
                break;
        }
    }

    public void CloseBuffPanel()
    {
        levelBuffUI.GetComponent<CanvasGroup>().alpha = 1f;
        levelBuffUI.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
        levelBuffUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 250f), UIFadeTime, false).SetEase(Ease.InOutQuint).OnComplete(BuffPanelFadedOuted);
        levelBuffUI.GetComponent<CanvasGroup>().DOFade(0, UIFadeTime);
    }

    public void BuffPanelFadedOuted()
    {
        levelBuffUI.SetActive(false);
        playerController.SetController(true);
    }

    public void ReachToFinalGate()
    {
        keyUI.SetActive(false);
        playerController.SetController(false);

        levelBuffUI.SetActive(false);
        chooseWeaponUI.SetActive(false);
         
        ClearCollectables();
        /*menuPanel.GetComponent<CanvasGroup>().alpha = 0f;
        menuPanel.SetActive(true);
        menuPanel.GetComponent<CanvasGroup>().DOFade(1, UIFadeTime / 2).OnComplete(ClearCollectables);*/

        levelStageText.transform.parent.gameObject.SetActive(false);
        NextStage();
    }

    public void ClearCollectables()
    {
        foreach (Transform t in collectablesParent)
        {
            Destroy(t.gameObject);
        }
    }     
     
    public void NextStage()
    {
        // If all level stages have not been finished.
        if (currentStage < levelPrefabs[currentLevel].transform.childCount - 2)
        {
            currentStage++;
            PlayerPrefs.SetInt("levelStage", currentStage);
            stageFadePanel.GetComponent<CanvasGroup>().alpha = 0f;
            stageFadePanel.SetActive(true);
            stageFadePanel.GetComponent<CanvasGroup>().DOFade(1, UIFadeTime).OnComplete(InitLevel);
        }
        // Go next level, first stage.
        else
        {
            //currentLevel++;
            currentStage = 0;
            PlayerPrefs.SetInt("level", currentLevel);
            PlayerPrefs.SetInt("levelStage", currentStage);
            stageFadePanel.GetComponent<CanvasGroup>().alpha = 0f;
            stageFadePanel.SetActive(true);
            stageFadePanel.GetComponent<CanvasGroup>().DOFade(1, UIFadeTime).OnComplete(OpenMenu);
        }
        PlayerPrefs.SetInt("MaxStage", currentStage);
    }

    public void PlayerIsDeath()
    {
        currentStage = 0;
        failPanel.SetActive(true);
        PlayerPrefs.SetInt("levelStage", currentStage);
        PlayerPrefs.SetInt("MaxStage", currentStage);
        ShakeCam();
    }

    public void ResetLevelAndStage()
    {
        currentLevel = 0;
        currentStage = 0;
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.SetInt("levelStage", currentStage);
        PlayerPrefs.GetInt("MaxStage", 0);
        InitLevel();
    }

    public void ResetWeaponUpgrades()
    {
        foreach(Weapon wp in weapons)
        {
            wp.ResetLevel();
        }
        stageFadePanel.GetComponent<CanvasGroup>().alpha = 1f;
        stageFadePanel.SetActive(true);
        stageFadePanel.GetComponent<CanvasGroup>().DOFade(0, UIFadeTime).OnComplete(StageLoaded);
    }
     
    public void ResetBuffs()
    {
        PlayerPrefs.SetFloat("AttackSpeedBuff", 1);
        baseAttackSpeedMultiplier = PlayerPrefs.GetFloat("AttackSpeedBuff", 1);
        playerController.SetAttackSpeed();

        PlayerPrefs.SetFloat("DamageBuff", 1);
        baseDamageMultiplier = PlayerPrefs.GetFloat("DamageBuff", 1);
        playerController.SetDamage();
    }

    public void ResetCollectables()
    {
        SetCoinAmount(-coinAmount);
        SetCrossAmount(-crossAmount);
    }

    public void ResetAllGame()
    {
        playerController.Heal(playerController.maxHealth);
        ResetCollectables();
        ResetBuffs();
        ResetWeaponUpgrades();
        ResetLevelAndStage();
    }

    public void OpenSettings()
    {
        Time.timeScale = 0f;
        settingsUI.SetActive(true);
    }

    public void GoMenu()
    {
        menuWarningUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void CloseMenuWarningUI()
    {
        menuWarningUI.SetActive(false);
    }

    public void ClearLevelValues()
    {
        ResetBuffs();
        playerController.Heal(playerController.maxHealth);
    }
}

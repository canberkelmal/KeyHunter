using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform levelsParent;
    public LayerMask attackableLayerMask, defaultLayerMask, blockAttackLayerMask, deathLayerMask;
    public Material attackedMat;
    public List<Weapon> weapons;
    public List<Buff> buffs;
    public List<GameObject> collectables;
    public float baseAttackSpeed = 1f;
    public Text coinText, crossText;
    public int coinAmount = 0;
    public int crossAmount = 0;
    public float dropHeight = 0.5f;
    public Level[] levels;
    public GameObject[] levelPrefabs;
    public GameObject levelBuffUI, chooseWeaponUI, menuPanel;
    public float UIFadeTime = 0.7f;


    int currentLevel, currentStage;
    Weapon selectedWeapon;
    Buff buffUI1, buffUI2;

    private void Start()
    {
        deathLayerMask = LayerMask.NameToLayer("DeathEnemy");
        defaultLayerMask = LayerMask.NameToLayer("Default");

        // Set UI elements
        SetCoinAmount(0);
        SetCrossAmount(0);

        menuPanel.SetActive(true);
    }

    public void InitLevel()
    {
        // Load level
        OpenChooseWeaponPanel();

        currentLevel = PlayerPrefs.GetInt("level", 0);
        currentStage = PlayerPrefs.GetInt("levelStage", 0);
        
        foreach (Transform clv in levelsParent.transform)
        {
            Destroy(clv.gameObject);
        }

        GameObject levelPrefab = levels[currentLevel].levelPrefab.transform.GetChild(currentStage).gameObject; 
        GameObject spawnedLevel = Instantiate(levelPrefab, levelsParent);

        // Set player position and weapon
        player.transform.position = spawnedLevel.transform.Find("PlayerSpawnPoint").position;
        SetWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetWeaponPref(string weaponName)
    {
        PlayerPrefs.SetString("WeaponName", weaponName); 
        SetWeapon();
    }

    public void SetWeapon()
    {
        PlayerController playerSc = player.GetComponent<PlayerController>();

        // Remove current weapon from player
        foreach (Transform currentWeapon in playerSc.weaponPoint.transform)
        {
            Destroy(currentWeapon.gameObject);
        }

        // Find the desired weapon
        selectedWeapon = GetWeapon(PlayerPrefs.GetString("WeaponName", "Blade"));

        // Set the desired weapon
        GameObject weaponObject = Instantiate(selectedWeapon.prefab, playerSc.weaponPoint.transform);
        playerSc.attackRange = selectedWeapon.range;
        playerSc.rangeCircleImage.transform.localScale = Vector3.one * playerSc.attackRange;
        playerSc.isRanged = selectedWeapon.ranged;
        playerSc.bullet = selectedWeapon.bullet != null ? selectedWeapon.bullet : null;
        playerSc.attackSpeed = baseAttackSpeed * selectedWeapon.weaponSpeed;
        player.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = selectedWeapon.animator;        
    }

    public void OpenChooseWeaponPanel()
    {
        player.GetComponent<PlayerController>().SetController(false);

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
        player.GetComponent<PlayerController>().SetController(true);

        chooseWeaponUI.GetComponent<CanvasGroup>().alpha = 1f;
        chooseWeaponUI.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
        chooseWeaponUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 250f), UIFadeTime, false).SetEase(Ease.InOutQuint).OnComplete(ChoosePanelFadedOuted);
        chooseWeaponUI.GetComponent<CanvasGroup>().DOFade(0, UIFadeTime);
    }
    public void ChoosePanelFadedOuted()
    {
        chooseWeaponUI.SetActive(false);
        PlayerController playerSc = player.GetComponent<PlayerController>();
        playerSc.SetController(true);
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

        PlayerController playerSc = player.GetComponent<PlayerController>();
        playerSc.SetController(false);

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
        buffUI1.BuffEffect();
        CloseBuffPanel();
    }
    public void Buff2Button()
    {
        buffUI2.BuffEffect();
        CloseBuffPanel();
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
        PlayerController playerSc = player.GetComponent<PlayerController>();
        playerSc.SetController(true);
    }

     
     
    public void NextStage()
    {
        if(levelPrefabs.Length > 0)
        {

        }
        // If all level stages have not been finished.
        if (currentStage < levels[currentLevel].stageCount - 2)
        {
            currentStage++;
            PlayerPrefs.SetInt("levelStage", currentStage);
        }
        // Go next level, first stage.
        else
        {
            ClearLevelBuffs();
            currentLevel++;
            currentStage = 0;
            PlayerPrefs.SetInt("level", currentLevel);
            PlayerPrefs.SetInt("levelStage", currentStage);
        }
    }

    public void ClearLevelBuffs()
    {

    }
}

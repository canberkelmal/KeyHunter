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
    public List<GameObject> collectables;
    public float baseAttackSpeed = 1f;
    public Text coinText, crossText;
    public int coinAmount = 0;
    public int crossAmount = 0;
    public float dropHeight = 0.5f;
    public GameObject[] levelPrefabs;
    int currentLevel, currentStage;

    private void Start()
    {
        InitLevel(currentLevel, currentStage);
        SetWeapon();
        deathLayerMask = LayerMask.NameToLayer("DeathEnemy");
        defaultLayerMask = LayerMask.NameToLayer("Default");
        SetCoinAmount(0); 
        SetCrossAmount(0);
    }

    void InitLevel(int level, int stage)
    {
        currentLevel = PlayerPrefs.GetInt("level", 0);
        currentStage = PlayerPrefs.GetInt("levelStage", 0);

        foreach (Transform clv in levelsParent.transform)
        {
            Destroy(clv.gameObject);
        }

        GameObject levelPrefab = levelPrefabs[level].transform.GetChild(stage).gameObject; 
        GameObject spawnedLevel = Instantiate(levelPrefab, levelsParent);
        player.transform.position = spawnedLevel.transform.Find("PlayerSpawnPoint").position;
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
        string weaponName = PlayerPrefs.GetString("WeaponName", "Blade");
        Weapon selectedWeapon = null;
        foreach (var weapon in weapons)
        {
            if(weapon.name == weaponName)
            {
                selectedWeapon = weapon;
            }
        }

        // Set the desired weapon
        GameObject weaponObject = Instantiate(selectedWeapon.prefab, playerSc.weaponPoint.transform);
        playerSc.attackRange = selectedWeapon.range;
        playerSc.rangeCircleImage.transform.localScale = Vector3.one * playerSc.attackRange;
        playerSc.isRanged = selectedWeapon.ranged;
        playerSc.bullet = selectedWeapon.bullet != null ? selectedWeapon.bullet : null;
        playerSc.attackSpeed = baseAttackSpeed * selectedWeapon.weaponSpeed;
        player.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = selectedWeapon.animator;        
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
    public void SetBuff()
    {

    }
}

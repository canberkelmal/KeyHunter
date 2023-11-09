using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public LayerMask attackableLayerMask, defaultLayerMask, blockAttackLayerMask, deathLayerMask;
    public Material attackedMat;
    public List<Weapon> weapons;

    private void Start()
    {
        SetWeapon();
        deathLayerMask = LayerMask.NameToLayer("DeathEnemy");
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
        string weaponName = PlayerPrefs.GetString("WeaponName", "Blade");
        PlayerController playerSc = player.GetComponent<PlayerController>();

        // Remove current weapon from player
        foreach (Transform currentWeapon in playerSc.weaponPoint.transform)
        {
            Destroy(currentWeapon.gameObject);
        }

        // Find the desired weapon
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
        playerSc.isRanged = selectedWeapon.ranged;
        playerSc.bullet = selectedWeapon.bullet != null ? selectedWeapon.bullet : null;
        player.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = selectedWeapon.animator;
    }
}

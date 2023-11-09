using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public LayerMask attackableLayerMask, defaultLayerMask, blockAttackLayerMask;
    public Material attackedMat;
    public List<Weapon> weapons;


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

    public void SetWeapon(string weaponName)
    {
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
    }
}

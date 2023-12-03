using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    public GameObject prefab;
    public GameObject bullet;

    public RuntimeAnimatorController animator;

    public Sprite icon;

    public bool ranged;

    public float range;
    public float weaponSpeed;

    public float maxDamage, minDamage;
    public float maxRange, minRange;
    public float maxAttackSpeed, minAttackSpeed;

    public float Damage()
    {
        float damage = minDamage + PlayerPrefs.GetInt("weaponName" + "Level", 0) * ((maxDamage-minDamage)/10);
        return damage;
    }
    public float AttackSpeed()
    {
        float damage = minAttackSpeed - PlayerPrefs.GetInt("weaponName" + "Level", 0) * ((maxAttackSpeed - minAttackSpeed) / 10);
        return damage;
    }
    public float Range()
    {
        float damage = minRange + PlayerPrefs.GetInt("weaponName" + "Level", 0) * ((maxRange - minRange) / 10);
        return damage;
    }

    public void Upgrade()
    {
        int currentLevel = PlayerPrefs.GetInt("weaponName" + "Level", 0);
        PlayerPrefs.SetInt("weaponName" + "Level", currentLevel + 1);
    }

    public void ResetLevel()
    {
        PlayerPrefs.SetInt("weaponName" + "Level", 0);
    }
}

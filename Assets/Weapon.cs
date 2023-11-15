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
}

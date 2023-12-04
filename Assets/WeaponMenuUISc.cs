using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenuUISc : MonoBehaviour
{
    Weapon buttonWeapon;

    public void SetWeapon(Weapon weapon)
    {
        buttonWeapon = weapon;
        SetUI();
    }
    public void SetUI()
    {
        transform.Find("WeaponUI").GetComponent<Image>().sprite = buttonWeapon.icon;
    }

    public void ButtonPressed()
    {
        if(buttonWeapon != null)
        {
            transform.parent.parent.parent.GetComponent<MenuScript>().OpenWeaponUpgradePanel(buttonWeapon);
        }
    }
}

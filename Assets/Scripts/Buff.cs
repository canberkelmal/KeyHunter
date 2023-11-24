using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CollectableSc;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff")]
public class Buff : ScriptableObject
{
    public string buffName;

    public Sprite uISprite;

    public enum BuffTypes
    {
        attackSpeed,
        doubleShot,
    }

    public BuffTypes type;

    public void BuffEffect()
    {
        switch (type)
        {
            case BuffTypes.attackSpeed:

                break;
            case BuffTypes.doubleShot:

                break;
        }
    }
}

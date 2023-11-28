using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public GameObject bgPrefab;
    public int stageCount;
    public GameObject levelPrefab;

}

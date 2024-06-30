using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class BuildManage : MonoBehaviour
{
    public static BuildManage main;


    [Header("Referemces")]
    [SerializeField] private GameObject[] towerPrefabs;

    private int selectedTower = 0 ;


    private void Awake()
    {
        main = this;
    }


    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTower];
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public static int MaxItemsNextToFlag = 6;
    
    // Item Prefabs
    public GameObject[] itemPrefabs;
    
    // Static Item Prefabs
    public static GameObject[] ItemPrefabs;
    
    private void Start()
    {
        ItemPrefabs = itemPrefabs;
    }
}
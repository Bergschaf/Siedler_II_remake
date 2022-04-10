using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    /// <summary>
    /// The maximum number of items that can lay next to a flag
    /// </summary>
    public static int MaxItemsNextToFlag = 6;

    public int itemCount = 1; // How many Item Types are in the game
    // Item Prefabs
    public GameObject[] itemPrefabs;
    
    // Static Item Prefabs
    public static GameObject[] ItemPrefabs;
    
    /// <summary>
    /// All flags with Item Suppliers attached to them
    /// At the position (item ID) of an item is a list of all flags that have suppliers (e.g. stone hut for stone) that item
    /// </summary>
    public static List<FlagScript>[] ItemSuppliers;

    /// <summary>
    /// All flags with Item Suppliers, that supply all kind of items (e.g. a warehouse or the main base) attached to them
    /// </summary>
    public static List<FlagScript> GenericSuppliers;

    private void Awake()
    {
        ItemPrefabs = itemPrefabs;
        
        ItemSuppliers = new List<FlagScript>[itemCount];
        GenericSuppliers = new List<FlagScript>();
    }

    /// <summary>
    /// Called by a flag, when it needs an item of the given type
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="itemID"></param>
    public static void DemandItem(FlagScript flag, int itemID)
    {
        
    }
}
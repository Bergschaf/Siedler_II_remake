using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    /// <summary>
    /// The maximum number of items that can lay next to a flag
    /// </summary>
    public static int MaxItemsNextToFlag = 6;

    public static int itemCount = 1; // How many Item Types are in the game
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
        for (int i = 0; i < itemCount; i++)
        {
            ItemSuppliers[i] = new List<FlagScript>();
        }
        GenericSuppliers = new List<FlagScript>();
    }

    /// <summary>
    /// Called by a flag, when it needs an item of the given type
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="itemID"></param>
    /// <returns>The number of items that weren't send, 0 if all were send</returns>
    public static int DemandItem(FlagScript flag, int itemID, int numberOfItems)
    {
        FlagScript[] suppliers = ItemSuppliers[itemID].ToArray().Concat(GenericSuppliers.ToArray()).ToArray();
        List<Tuple<Road[], FlagScript>> supplierpaths = new List<Tuple<Road[], FlagScript>>();
        
        List<Road[]> paths = GameHandler.GetMultipleRoadGridPaths(flag, suppliers);
        
        for(int i = 0; i < paths.Count; i++)
        {
            supplierpaths.Add(new Tuple<Road[], FlagScript>(paths[i].Reverse().ToArray(), suppliers[i]));
        } 
        
        // Sort the paths by the length of the path
        supplierpaths = supplierpaths.OrderBy(x => GetRoadPathLength(x.Item1)).ToList();
        
        
        // TODO Maybe some weighting for the paths? e.g. weigh direct suppliers (e.g. stone-cutter) more than the generic suppliers (e.g. warehouse)
        int itemsToSend = numberOfItems;
        List<ItemScript> toRemove;
        for (int i = 0; i < supplierpaths.Count; i++)
        {
            toRemove = new List<ItemScript>();
            int min = Math.Min(itemsToSend, supplierpaths[i].Item2.AvailableItems[itemID].Count);
            for (int j = 0; j < min; j++)
            {
                supplierpaths[i].Item2.AvailableItems[itemID][j].GetTransportedToFlag(flag,supplierpaths[i].Item1);
                toRemove.Add(supplierpaths[i].Item2.AvailableItems[itemID][j]);
                itemsToSend--;
            }

            foreach (var x in toRemove)
            {
                supplierpaths[i].Item2.AvailableItems[itemID].Remove(x);
            }
            
        }

        return itemsToSend;

    }

    

    /// <summary>
    /// determines the Length of a road path represented by an array of roads
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static float GetRoadPathLength(Road[] path)
    {
        float length = 0;
        foreach (var road in path)
        {
            length += road.Len;
        }

        return length;
    }


}
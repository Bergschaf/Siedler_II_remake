using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using Object = System.Object;

/// <summary>
/// The script assigned to the flag GameObject
/// </summary>
public class FlagScript : MonoBehaviour
{
    /// <summary>
    /// The GameObject of the crossing at the road
    /// </summary>
    private GameObject _dirtCrossing;

    /// <summary>
    /// All road attached to the Flag
    /// </summary>
    /// <code>List(Tuple(Road,FlagScript): Road,TargetFlag</code>
    public List<Tuple<Road, FlagScript>> AttachedRoads; // (Road,Target)

    /// <summary>
    /// The Items next to the Flag
    /// </summary>
    public ItemScript[] Items;

    /// <summary>
    /// The possible positions for Items next to the Flag
    /// </summary>
    private Vector3[] _itemPositions;

    /// <summary>
    /// The alignment to the terrain of the items at the positions next to the flag
    /// </summary>
    private Vector3[] _itemAlignments;

    /// <summary>
    /// A list of items for each type the flag has available (including items in the building at the flag)
    /// </summary>
    public List<ItemScript>[] AvailableItems;

    private void Awake()
    {
        Vector3 position = transform.position;
        position = new Vector3(position.x,
            GameHandler.ActiveTerrain.SampleHeight(position),
            position.z);
        transform.position = position;

        Node temp = Grid.NodeFromWorldPoint(position);

        if (temp.Type == "Road")
        {
            GameHandler.PlaceFlagInRoad(this);
        }

        temp.Type = "Flag";
        temp.Buildable = false;
        temp.Flag = this;
        temp.BuildableIcon.SetActive(false);

        Items = new ItemScript[ItemHandler.MaxItemsNextToFlag];
        GenerateItemPositionsAlignment();

        AvailableItems = new List<ItemScript>[ItemHandler.itemCount];
        for (int i = 0; i < ItemHandler.itemCount; i++)
        {
            AvailableItems[i] = new List<ItemScript>();
        }
        Grid.NodeFromWorldPoint(transform.position).CalculateBuildableTypeAround();
    }

    private void OnMouseDown()
    {
        if (GameHandler.CurrentlyBuildingRoad)
        {
            if (GameHandler.CurrentRoad.add_point(transform.position))
            {
                GameHandler.EndBuildingRoad(gameObject.GetComponent<FlagScript>());
            }
        }
        else if (!UIHandler.GUIActive)
        {
            UIHandler.StartFlagGUI(transform.position);
        }

        UIHandler.LastClickedFlag = gameObject;
        
    }

    private void GenerateDirtCrossing()
    {
        _dirtCrossing = Instantiate(GameHandler.DirtCrossingPrefab);
        _dirtCrossing.GetComponent<DirtCrossingMeshScript>().SetVertices(transform.position);
        if (!GameHandler.AllFlags.Contains(this))
        {
            GameHandler.AllFlags.Add(this);
        }
    }

    private void DestroyDirtCrossing()
    {
        if (_dirtCrossing != null) _dirtCrossing.GetComponent<DirtCrossingMeshScript>().destroy();
        GameHandler.AllFlags.Remove(this);
    }

    public void AddRoad(Road road, FlagScript targetFlagScript)
    {
        if (AttachedRoads == null)
        {
            AttachedRoads = new List<Tuple<Road, FlagScript>>();
        }

        if (AttachedRoads.Count < 1)
        {
            GenerateDirtCrossing();
        }

        AttachedRoads.Add(new Tuple<Road, FlagScript>(road, targetFlagScript));
    }

    public void RemoveRoad(Road road)
    {
        foreach (var t in AttachedRoads)
        {
            if (t.Item1 == road)
            {
                AttachedRoads.Remove(t);
                t.Item1.destroy();
                break;
            }
        }

        if (AttachedRoads.Count < 1)
        {
            DestroyDirtCrossing();
        }
    }

    /// <summary>
    /// Destroys the Flag and removes all the road connections
    /// </summary>
    public void Destroy()
    {
        foreach (var item in Items)
        {
            if (item != null) Destroy(item.gameObject);
        }

        // The Roads that should be removed from a flag, because they connect to this flag
        List<Road> toRemove;
        if (AttachedRoads != null)
            foreach (var t in AttachedRoads)
            {
                if (t.Item2.AttachedRoads != null)

                {
                    toRemove = new List<Road>();
                    foreach (var x in t.Item2.AttachedRoads)
                    {
                        if (x.Item2 == this)
                        {
                            toRemove.Add(x.Item1);
                        }
                    }

                    foreach (var r in toRemove)
                    {
                        t.Item2.RemoveRoad(r);
                    }
                }
            }

        GameHandler.AllFlags.Remove(this);
        DestroyDirtCrossing();
        Destroy(gameObject);
        UIHandler.EndGUI();
        var position = transform.position;
        Grid.NodeFromWorldPoint(position).Type = "Buildable0";
        Grid.NodeFromWorldPoint(position).CalculateBuildableTypeAround();
    }

    /// <summary>
    /// Generates the Positions for the Items next to the Flag in a circle
    /// </summary>
    private void GenerateItemPositionsAlignment()
    {
        _itemPositions = new Vector3[ItemHandler.MaxItemsNextToFlag];
        _itemAlignments = new Vector3[ItemHandler.MaxItemsNextToFlag];
        int c = 0;
        RaycastHit hit;
        float maxItems = ItemHandler.MaxItemsNextToFlag;

        for (float i = 0; i < 360; i += 360 / maxItems)
        {
            Vector3 pos = transform.position +
                          Quaternion.AngleAxis(i, Vector3.up) * Vector3.forward *
                          2f;

            _itemPositions[c] = new Vector3(pos.x, GameHandler.ActiveTerrain.SampleHeight(pos) + 0.2f, pos.z);

            Physics.Raycast(pos, Vector3.down, out hit, 2, GameHandler.TerrainLayer);

            _itemAlignments[c] = hit.normal;

            c++;
        }
    }

    /// <summary>
    /// Adds the item next to the flag
    /// </summary>
    /// <param name="item"></param>
    /// <returns>True if it succeeded, false if there are already to much items next to to the flag</returns>
    public bool AddItem(ItemScript item)
    {
        float maxItems = ItemHandler.MaxItemsNextToFlag;

        for (int i = 0; i < maxItems; i++)
        {
            if (Items[i] == null)
            {
                Transform transform1 = item.transform;

                transform1.position = _itemPositions[i];
                transform1.up = _itemAlignments[i];
                transform1.rotation = Quaternion.Euler(0, 360 * (i / maxItems), 0) *
                                      ItemHandler.ItemSpecificRotation[item.itemID];
                Items[i] = item;
                return true;
            }
        }

        return false;
    }

    public void AddAvailableItem(ItemScript item)
    {
        AvailableItems[item.itemID].Add(item);
    }

    /// <summary>
    /// Trys to remove the item from the flag
    /// </summary>
    /// <param name="itemScript"></param>
    /// <returns>true if the items was at the flag, false it not</returns>
    public bool RemoveItem(ItemScript itemScript)
    {
        for (int i = 0; i < ItemHandler.MaxItemsNextToFlag; i++)
        {
            if (Items[i] == itemScript)
            {
                Items[i] = null;
                AvailableItems[itemScript.itemID].Remove(itemScript);
                return true;
            }
        }

        foreach (var i in AvailableItems[itemScript.itemID])
        {
            if (i == itemScript)
            {
                // TODO Have a settler transport the item out of the building to the flag if it isN#t at the flag

                AvailableItems[itemScript.itemID].Remove(itemScript);
                return true;
            }
        }


        return false;
    }

    public void PlaceBuildingAtFlag(int buildingID)
    {
        
    }
}
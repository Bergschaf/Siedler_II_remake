using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public int itemID;
    public FlagScript currentFlag;
    public bool inTransport;

    private void Awake()
    {
        inTransport = false;
    }

    public void GetTransportedToFlag(FlagScript flag, Road[] path)
    {
        List<IEnumerator> corountines = new List<IEnumerator>();
        inTransport = true;
        int pathPos = 0;
        while (flag != currentFlag)
        {
            if (currentFlag == path[pathPos].Flag1)
            {
                corountines.Add(path[pathPos].Settler.TransportItemOnRoad(this, true));
                currentFlag = path[pathPos].Flag2;
            }
            else
            {
                corountines.Add(path[pathPos].Settler.TransportItemOnRoad(this, false));
                currentFlag = path[pathPos].Flag1;
            }

            pathPos++;
        }
        
        corountines.Add(EndTransport());
        StartCoroutine(GameHandler.ExecuteCoroutines(corountines));
    }

    private IEnumerator EndTransport()
    {
        inTransport = false;
        yield return null;
    }

    private void OnDestroy()
    {
        // TODO Tell the item target that it has to demand a new item
    }
}
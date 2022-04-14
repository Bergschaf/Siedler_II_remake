using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabHandlerScript : MonoBehaviour
{
    public GameObject[] tabContents;
    private int _currentTabID = 0;
    public GameObject parent;

    private void Start()
    {
        foreach (var t in tabContents)
        {
            t.SetActive(false);
        }
        tabContents[_currentTabID].SetActive(true);
    }

    public void ChangeTab(int tabNumber)
    {
        tabContents[_currentTabID].SetActive(false);
        _currentTabID = tabNumber;
        tabContents[_currentTabID].SetActive(true);

    }
}

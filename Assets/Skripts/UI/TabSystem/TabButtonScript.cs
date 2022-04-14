using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabButtonScript : MonoBehaviour
{
    public int tabIndex;
    public TabHandlerScript tabHandler;
    
    public void Click()
    {
        tabHandler.ChangeTab(tabIndex);
    }
}

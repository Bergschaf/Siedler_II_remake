
using UnityEngine;

public class DestroyFlagButtonScript : MonoBehaviour
{
    public void Click(GameObject parentGUI)
    {
        UIHandler.LastClickedFlag.GetComponent<FlagScript>().Destroy();
        Destroy(parentGUI);
    }
}

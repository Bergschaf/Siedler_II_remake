using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSkript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x,
            GameHandler.ActiveTerrain.SampleHeight(transform.position),
            transform.position.z); 
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBuildableSkript : MonoBehaviour
{
    
    void Start()
    {
        transform.position = new Vector3(transform.position.x,
            GameHandler.ActiveTerrain.SampleHeight(transform.position), transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReplaceWithFlag()
    {
        
    }
}
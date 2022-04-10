using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script for each settler, not fully developed yet
/// </summary>
public class SettlerScript : MonoBehaviour
{
    /// <summary>
    /// The settlers job
    /// </summary>
    public string job;

    /// <summary>
    /// The walking speed of the settler
    /// </summary>
    private float _speed = 10;

    /// <summary>
    /// The road the settler should stand at and carry the Items between the two flags of the road
    /// </summary>
    public Road AssignedRoad;

    /// <summary>
    /// The flag the settler is currently at
    /// </summary>
    public FlagScript currentFlag;

    /// <summary>
    /// The item the settler is currently carrying
    /// </summary>
    private ItemScript _item;

    /// <summary>
    /// True if the settler is currently carrying an item
    /// </summary>
    public bool transporting;

    // Start is called before the first frame update
    void Awake()
    {
        job = "Standard";
        currentFlag = GameHandler.HomeFlag;
    }

    /// <summary>
    /// Travels allong the path in a coroutine
    /// </summary>
    /// <param name="pathToTravel"></param>
    /// <returns></returns>
    private IEnumerator TravelAlongPath(Vector3[] pathToTravel)
    {

        float interpolationStepSize =
            _speed / Vector3.Distance(pathToTravel[0], pathToTravel[1]);

        float interpolation = 0;
        for (int i = 0; i < pathToTravel.Length - 1; i++)
        {
            while (interpolation < 1)
            {
                Vector3 lerpPos = Vector3.Lerp(pathToTravel[i], pathToTravel[i + 1], interpolation);
                transform.position = new Vector3(lerpPos.x, GameHandler.ActiveTerrain.SampleHeight(lerpPos), lerpPos.z);
                interpolation += interpolationStepSize * Time.deltaTime;
                yield return null;
            }

            interpolation = 0;
        }

    }

    /// <summary>
    /// Picks up the item from the flag, requires the settler to be at the flag
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    private IEnumerator PickUpItem(FlagScript flag, ItemScript item)
    {
        // TODO Fancy animation here
        yield return new WaitForSeconds(0.5f);

        flag.RemoveItem(item); // TODO Handle if false is returned  
        _item = item;
        var transform1 = _item.transform;
        transform1.parent = transform;
        transform1.localPosition = Vector3.up * 1.5f;
        transform1.localRotation = Quaternion.Euler(0, 0, 0) * ItemHandler.ItemSpecificRotation[_item.itemID];
    }

    /// <summary>
    /// Puts the item the settler is currently carrying down at the flag, requires the settler to be at the flag and to be carrying an item
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    private IEnumerator PutItemToFlag(FlagScript flag)
    {
        // TODO Fancy animation here
        yield return new WaitForSeconds(0.5f);
        _item.transform.parent = null;
        while (!flag.AddItem(_item))
        {
            yield return new WaitForSeconds(0.1f);
        }

        _item = null;
    }


    /// <summary>
    /// Sets the road a settler should stand at and carry the items between the two flags of the road
    /// </summary>
    /// <param name="roadToAssign">the road the settler should go to</param>
    /// <param name="roadPath">The path the Settler should take to the road</param>
    public void AssignRoad(Road roadToAssign, Road[] roadPath)
    {
        roadToAssign.Settler = this;
        AssignedRoad = roadToAssign;
        
        List<Vector3> Path = new List<Vector3>();

        Vector3 lastPos = currentFlag.transform.position;
        Path.Add(lastPos);
        foreach (var r in roadPath)
        {
            if (r == roadToAssign)
            {
                break;
            }

            if (Vector3.Distance(r.Pos1, lastPos) < Vector3.Distance(r.Pos2, lastPos))
            {
                for (int i = 1; i < r.RoadPoints.Length; i++)
                {
                    Path.Add(r.RoadPoints[i]);
                }

                lastPos = r.Pos2;
            }
            else
            {
                for (int i = r.RoadPoints.Length - 2; i >= 0; i--)
                {
                    Path.Add(r.RoadPoints[i]);
                }

                lastPos = r.Pos1;
            }
        }

        if (Vector3.Distance(roadToAssign.Pos1, lastPos) < Vector3.Distance(roadToAssign.Pos2, lastPos))
        {
            for (int i = 1; i < roadToAssign.RoadPoints.Length / 2; i++)
            {
                Path.Add(roadToAssign.RoadPoints[i]);
            }
        }
        else
        {
            for (int i = roadToAssign.RoadPoints.Length - 2; i >= roadToAssign.RoadPoints.Length / 2; i--)
            {
                Path.Add(roadToAssign.RoadPoints[i]);
            }
        }

        Path.Add(roadToAssign.MiddlePos);

        Vector3[] pathToTravel = GameHandler.MakeSmoothCurve(Path.ToArray());
        StartCoroutine(TravelAlongPath(pathToTravel));
        currentFlag = null;
    }

    public void GoBackToHomeFlag()
    {
        // TODO Check if settler could go to a nearby road and carry stuff there

        AssignedRoad = null;
        List<Vector3> Path = new List<Vector3>();
        FlagScript flag = Grid.ClosestFlagToWorldPoint(transform.position);
        Road[] roadPath = GameHandler.GetRoadGridPath(flag, GameHandler.HomeFlag);


        Vector3[] pathToTravel;
        if (flag != null && flag != GameHandler.HomeFlag && roadPath != null)
        {
            Path.Add(transform.position);
            Vector3 lastPos = flag.transform.position;
            Path.Add(lastPos);
            foreach (var r in roadPath)
            {
                if (Vector3.Distance(r.Pos1, lastPos) > Vector3.Distance(r.Pos2, lastPos))
                {
                    for (int i = 1; i < r.RoadPoints.Length; i++)
                    {
                        Path.Add(r.RoadPoints[i]);
                    }

                    lastPos = r.Pos2;
                }
                else
                {
                    for (int i = r.RoadPoints.Length - 2; i >= 0; i--)
                    {
                        Path.Add(r.RoadPoints[i]);
                    }

                    lastPos = r.Pos1;
                }
            }

            Path.Add(GameHandler.HomeFlag.transform.position);

            pathToTravel = GameHandler.MakeSmoothCurve(Path.ToArray());
        }
        else
        {
            Node[] nodePath = RoadPathfinding.FindPath(transform.position, GameHandler.HomeFlag.transform.position)
                .ToArray();

            pathToTravel = new Vector3[nodePath.Length];
            for (int i = 0; i < nodePath.Length; i++)
            {
                pathToTravel[i] = nodePath[i].WorldPosition;
            }


            pathToTravel = GameHandler.MakeSmoothCurve(pathToTravel);
        }

        StartCoroutine(TravelAlongPath(pathToTravel));
    }

    /// <summary>
    /// The Settler walks from one flag of the road, picks up the item and carries it to the other
    /// </summary>
    /// <param name="item">The type of item the Settler should carry</param>
    /// <param name="startOnFlag1">Is the flag, where the item is, the first flag of the road => true, if not => false</param>
    public IEnumerator TransportItemOnRoad(ItemScript item, bool startOnFlag1)
    {
        
        List<IEnumerator> coroutines = new List<IEnumerator>();
        
        if (transporting)
        {
            yield return new WaitUntil((() => !transporting));
        }
        transporting = true;


        Vector3[] pathToTravel = new Vector3[AssignedRoad.SmoothRoadPoints.Length / 2];

        if (startOnFlag1)
        {
            int c = 0;
            for (int i = pathToTravel.Length; i > 0; i--)
            {
                pathToTravel[c] = AssignedRoad.SmoothRoadPoints[i];
                c++;
            }
        }
        else
        {
            for (int i = pathToTravel.Length; i < pathToTravel.Length * 2; i++)
            {
                pathToTravel[i - pathToTravel.Length] = AssignedRoad.SmoothRoadPoints[i];
            }
        }

        coroutines.Add(TravelAlongPath(pathToTravel));


        if (startOnFlag1)
        {
            coroutines.Add(PickUpItem(AssignedRoad.Flag1, item));
            coroutines.Add(TravelAlongPath(AssignedRoad.SmoothRoadPoints));
            coroutines.Add(PutItemToFlag(AssignedRoad.Flag2));
        }
        else
        {
            coroutines.Add(PickUpItem(AssignedRoad.Flag2, item));
            coroutines.Add(TravelAlongPath(AssignedRoad.SmoothRoadPoints.Reverse().ToArray()));
            coroutines.Add(PutItemToFlag(AssignedRoad.Flag1));
        }

        Vector3[] pathToTravel3 = new Vector3[AssignedRoad.SmoothRoadPoints.Length / 2];

        if (!startOnFlag1)
        {
            int c = 0;
            for (int i = 0; i < pathToTravel3.Length; i++)
            {
                pathToTravel3[c] = AssignedRoad.SmoothRoadPoints[i];
                c++;
            }
        }
        else
        {
            int c = 0;

            for (int i = pathToTravel3.Length * 2 - 1; i > pathToTravel3.Length - 1; i--)
            {
                pathToTravel3[c] = AssignedRoad.SmoothRoadPoints[i];
                c++;
            }
        }

        coroutines.Add(TravelAlongPath(pathToTravel3));
        coroutines.Add(EndTransport());
        yield return GameHandler.ExecuteCoroutines(coroutines);
    }

    private IEnumerator EndTransport()
    {
        transporting = false;
        yield return null;
    }


    // Temp
    private void OnMouseDown()
    {
        ItemHandler.DemandItem(GameHandler.HomeFlag, 1, 2);
        ItemHandler.DemandItem(GameHandler.HomeFlag, 0, 2);

        ItemHandler.DemandItem(GameHandler.HomeFlag, 2, 2);

    }
}
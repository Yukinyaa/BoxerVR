using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackerFinder : MonoBehaviour
{
    #region static
    static bool spawned = false;
    static int finderCount;
    static List<Valve.VR.SteamVR_TrackedObject> obj;
    public static void Reset()
    {
        spawned = false;
    }
    public static void SpawnTrackedObject(GameObject trackedObjectPF)
    {
        if (spawned == true) return;
        spawned = true;
        obj = new List<Valve.VR.SteamVR_TrackedObject>();
        finderCount = GameObject.FindObjectsOfType<TrackerFinder>().Length;
        for (int i = 1; i < 10; i++)
        {
            var go = GameObject.Instantiate(trackedObjectPF).GetComponent<Valve.VR.SteamVR_TrackedObject>();
            Debug.Assert(go != null);
            go.index = (Valve.VR.SteamVR_TrackedObject.EIndex)i;
            obj.Add(go);
        }
    }
    #endregion

    public GameObject trackedObjectPF;
    public float reigeon;

    public bool Engage()
    {
        var cast = Physics.BoxCastAll(transform.position, transform.localScale, Vector3.up);

        int inColCount = 0;
        Valve.VR.SteamVR_TrackedObject foundObject = null;

        foreach (var other in cast)
        {
            Valve.VR.SteamVR_TrackedObject obj = other.collider.GetComponent<Valve.VR.SteamVR_TrackedObject>();
            if (obj != null)
            {
                foundObject = obj;
                inColCount++;
            }
        }

        if (inColCount == 1 && foundObject != null)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            obj.Remove(foundObject);
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(foundObject.transform.Find("indicator").gameObject);
            transform.SetParent(foundObject.transform);
            this.transform.localPosition = Vector3.zero;
            return true;
        }
        //else
        return false;
    }


    private void Awake()
    {
        Reset();
    }
    private IEnumerator Start()
    {
        SpawnTrackedObject(trackedObjectPF);


        this.transform.localScale = new Vector3(reigeon, reigeon, reigeon);
        GetComponent<MeshRenderer>().enabled = true;


        yield return new WaitForSeconds(3f);


        while (Engage() != true) yield return null;
        finderCount--;
        if (finderCount == 0)
        {
            foreach (var o in obj) Destroy(o.gameObject);
        }
        
        yield break;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionRouter : MonoBehaviour
{
    public List<GameObject> colRouteObjects;
    private void OnCollisionEnter(Collision collision)
    {
        foreach (var a in colRouteObjects)
        {
            foreach (var b in a.GetComponents<IColRouteObject>())
            {
                b.OnRoutedCollisionEvent(collision); 
            }
            
        }
    }
}

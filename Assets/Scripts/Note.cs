using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandSide {left,right,any}
public class Note : MonoBehaviour
{
    public float beat;
    public Vector2 pos;
    public MusicPlayer mp;
    
    public HandSide handSide;

    public float speed;

    public List<List<GameObject>> OnDestoroyObjects;
    public void Init(float beat, Vector2 pos, MusicPlayer parent, float speed, HandSide hs)
    {
        this.beat = beat;
        this.pos = pos;
        this.mp = parent;
        this.speed = speed;
        handSide = hs;
    }

    void Update()
    {
        float delta = mp.CurrentBeat - beat;
        transform.position = new Vector3(pos.x, pos.y, -delta * speed);
    }
   void OnCollisionEnter(Collision collision) {
       if(LayerMask.NameToLayer("Hands")!=collision.gameObject.layer) return;
       if(handSide == HandSide.any) DestroyMe();

       switch(collision.rigidbody.name)
       {
        case "LHS":
            if(handSide == HandSide.left)
            {
                DestroyMe();
            }
            break;
        case "RHS":
            if(handSide == HandSide.right)
            {
                DestroyMe();
            }
            break;
        default: 
            throw new ArgumentException();
       }
   }
    void DestroyMe()
    {
        //foreach(var obj in OnDestoroyObjects)
        {
            //Instantiate(obj[UnityEngine.Random.Range(0,obj.Count)],transform.position,Quaternion.identity);
        }
        Destroy(this);
    }
}
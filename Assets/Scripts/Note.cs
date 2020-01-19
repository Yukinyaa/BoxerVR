using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandSide {left,right,any}
public enum HookSide{left,right,up,down,none}
public class Note : MonoBehaviour
{
    public float beat;
    public Vector2 pos;
    public HookSide hookSide;
    public MusicPlayer mp;
    public float reqStrength;
    public HandSide handSide;

    public float speed;

    public List<List<GameObject>> OnDestoroyObjects;
    public void Init(float beat, Vector2 pos, MusicPlayer parent, float speed, HandSide hs = HandSide.any, HookSide hss = HookSide.none)
    {
        this.beat = beat;
        this.pos = pos;
        this.mp = parent;
        this.speed = speed;
        handSide = hs;
        hookSide = hss;
    }

    void Update()
    {
        float delta = mp.CurrentBeat - beat;
        transform.position = new Vector3(pos.x, pos.y, -delta * speed);
    }
    const float sideAccuracy = 0.5f;
   void OnCollisionEnter(Collision collision) {
        if(LayerMask.NameToLayer("Hands")!=collision.gameObject.layer) return;
        
        var handVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        var hvNorm = handVelocity.normalized;
        switch(hookSide)
        {
            case HookSide.down:
                if(hvNorm.x<sideAccuracy) return;
                break;
            case HookSide.up:
                if(hvNorm.x>-sideAccuracy) return;
                break;
            case HookSide.left:
                if(hvNorm.y>-sideAccuracy) return;
                break;
            case HookSide.right:
                if(hvNorm.x<sideAccuracy) return;
                break;
            case HookSide.none:
                break;
        }
        if(handVelocity.magnitude <= reqStrength)
            return;

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
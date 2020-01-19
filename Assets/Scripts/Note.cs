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
    public Transform sideIndicator;
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
        switch(hookSide)
        {
            case HookSide.none: sideIndicator.gameObject.SetActive(false); break;
            case HookSide.up: sideIndicator.Rotate(0,0,0); break;
            case HookSide.right: sideIndicator.Rotate(0,0,90); break;
            case HookSide.down: sideIndicator.Rotate(0,0,180); break;
            case HookSide.left: sideIndicator.Rotate(0,0,270); break;


        }
    }

    void Update()
    {
        float delta = mp.CurrentBeat - beat;
        transform.position = new Vector3(pos.x, pos.y, -delta * speed);
    }
    const float sideAccuracy = 0.5f;
   void OnCollisionEnter(Collision collision) {
        if(LayerMask.NameToLayer("Hands")!=collision.gameObject.layer) return;
        
        var handVelocity = collision.gameObject.GetComponent<SpeedMeter>().velocity;
        var hvNorm = handVelocity.normalized;
        Debug.Log(hvNorm.x);
        switch(hookSide)
        {
            case HookSide.up:
                if(hvNorm.y<sideAccuracy) return;
                break;
            case HookSide.down:
                if(hvNorm.y>-sideAccuracy) return;
                break;
            case HookSide.right:
                if(hvNorm.x>-sideAccuracy) return;
                break;
            case HookSide.left:
                if(hvNorm.x<sideAccuracy) return;
                break;
            case HookSide.none:
                break;
        }
        if(handVelocity.magnitude <= reqStrength)
            return  ;

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
        Destroy(this.gameObject);
    }
}
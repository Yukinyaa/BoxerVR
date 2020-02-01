using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandSide {left,right,any,both}

public class Note : MonoBehaviour
{
    public float beat;
    public Vector2 pos;
    public float? hookSide;
    public MusicPlayer mp;
    public float reqStrength;
    public HandSide handSide;
    public Transform sideIndicator;
    public float speed;

    public List<List<GameObject>> OnDestoroyObjects;

    static Texture red;
    static Texture blu;
    static Texture grey;
    static Texture white;

    static Texture ColoredTexture(Texture og)
    {
        Texture2D targetTexture = new Texture2D(og.width, og.height);

        for (int y = 0; y < og.height; y++)
        {
            for (int x = 0; x < og.width; x++)
            {

                targetTexture.SetPixel(x, y, Color.green);

            }
        }

        targetTexture.Apply();
        return targetTexture;
    }
    static void initTexture(Texture og)
    {
    }

    public void Init(float beat, Vector2 pos, MusicPlayer parent, float speed, HandSide hs = HandSide.any, float? hss = null)
    {
        this.beat = beat;
        this.pos = pos;
        this.mp = parent;
        this.speed = speed;
        handSide = hs;
        switch (hs)
        {
            case HandSide.any:
                gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                break;
            case HandSide.both:
                gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
                break;
            case HandSide.left:
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case HandSide.right:
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
        }
        hookSide = hss;
        if(hookSide == null)
            sideIndicator.gameObject.SetActive(false);
        else
            sideIndicator.Rotate(0,0,hss??0); 
    }

    void Update()
    {
        float delta = mp.CurrentBeat - beat;
        if(delta > 1) DestroyMe();
        transform.position = new Vector3(pos.x, pos.y, -delta * speed);
    }
    const float sideAccuracy = 0.5f;
   void OnCollisionEnter(Collision collision) {
        if(LayerMask.NameToLayer("Hands")!=collision.gameObject.layer) return;
        
        var handVelocity = collision.gameObject.GetComponent<SpeedMeter>().velocity;
        var hvNorm = handVelocity.normalized;
        Debug.Log(hvNorm.x);
        
        if(hookSide != null)
        {
            var aa = new Vector2(handVelocity.x, handVelocity.y).normalized;
            var bb = new Vector2(0,1).Rotate(hookSide??0);
            if(Vector2.Dot(aa, bb) < 0)
                return;
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
        Destroy(this.gameObject);
    }
}
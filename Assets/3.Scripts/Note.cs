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
    public Material[] mats;
    public int nIdx = 0;
    public AudioClip TickSound;

    public List<GameObject> OnDestoroyObjects;
    
    public void Init(float beat, Vector2 pos, MusicPlayer parent, float speed, HandSide hs = HandSide.any, float? hss = null)
    {
        this.beat = beat;
        this.pos = pos;
        this.mp = parent;
        this.speed = speed;
        handSide = hs;
        Color c;
        switch (handSide)
        {
            case HandSide.any:
                c = Color.white;
                nIdx = 0;
                break;
            case HandSide.both:
                c = new Color(.5f, 0, .5f);//purple
                nIdx = 0;
                break;
            case HandSide.left:
                c = Color.red;
                nIdx = 1;
                break;
            case HandSide.right:
                c = Color.blue;
                nIdx = 2;
                break;
            default: throw new Exception();
        }
#if UNITY_2018
        gameObject.GetComponent<Renderer>().material.color = c;
#elif UNITY_2019
        // You can re-use this block between calls rather than constructing a new one each time.
        //var block = new MaterialPropertyBlock();

        //// You can look up the property by ID instead of the string to be more efficient.
        //block.SetColor("_BaseColor", c);

        //// You can cache a reference to the renderer to avoid searching for it.
        //GetComponent<Renderer>().SetPropertyBlock(block);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = mats[nIdx];//Resources.Load("Mesh_MeshEffect_violet") as Material;//= mats[nIdx];
#endif
        hookSide = hss;
        if (hookSide == null)
            sideIndicator.gameObject.SetActive(false);
        else
        {
            sideIndicator.Rotate(0, 0, hss ?? 0);
          
        }
    }

    void Update()
    {
        float delta = mp.CurrentBeat - beat;
        if (delta > 1) Destroy(this);
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
            var bb = new Vector2(0, 1).Rotate(hookSide ?? 0);
            if(Vector2.Dot(aa, bb) < 0)
                return;
        }
        if(handVelocity.magnitude <= reqStrength)
            return;


        Debug.Log("LEL_2: " + this.transform.position);
        if (handSide == HandSide.any) DestroyMe();
        switch(collision.rigidbody.name)
        {
            case "LHS":
                if(handSide == HandSide.left || handSide == HandSide.both)
                {
                    DestroyMe();
                }
                break;
            case "RHS":
                if(handSide == HandSide.right || handSide == HandSide.both)
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
        BeatManager.Instance.GetComponent<AudioSource>().PlayOneShot(TickSound, 5);
        if (OnDestoroyObjects != null)
        {
            GameManager objGameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
            objGameMgr.SetnScore(objGameMgr.GetnScore() + 50);

            GameObject obj_Destroy;
            switch (nIdx)
            {
                case 0:
                    //violet
                    obj_Destroy = Instantiate(OnDestoroyObjects[0], this.transform.position, Quaternion.identity);
                    break;
                case 1:
                    obj_Destroy = Instantiate(OnDestoroyObjects[1], this.transform.position, Quaternion.identity);
                    //red
                    break;
                case 2:
                    obj_Destroy = Instantiate(OnDestoroyObjects[2], this.transform.position, Quaternion.identity);
                    //blue
                    break;
                default:
                    obj_Destroy = Instantiate(OnDestoroyObjects[0], this.transform.position, Quaternion.identity);
                    //violet
                    break;
            }
            Destroy(obj_Destroy, 2.0f);
            //foreach (var obj in OnDestoroyObjects)
            //{
            //}


        }
        Destroy(this.gameObject);
    }
}
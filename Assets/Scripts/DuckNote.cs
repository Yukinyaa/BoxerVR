using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColRouteObject
{
    void OnRoutedCollisionEvent(Collision col);
}
public class DuckNote : MonoBehaviour, IColRouteObject
{
    public float beat;
    public Vector2 pos;
    public MusicPlayer mp;
    public float length;
    public float speed;
    public float duration;
    public AudioClip FailedSound;
    public AudioClip SuccessSound;
    

    public List<List<GameObject>> OnDestoroyObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(float beat, float duration, MusicPlayer parent, float speed)
    {
        this.beat = beat;
        this.duration = duration;
        this.mp = parent;
        this.speed = speed;
        Color c = Color.gray;
        pos = new Vector2(0, 1.7f);
        transform.localScale = new Vector3(3, 1, duration * speed);
#if UNITY_2018
        gameObject.GetComponent<Renderer>().material.color = c;
#elif UNITY_2019
        // You can re-use this block between calls rather than constructing a new one each time.
        var block = new MaterialPropertyBlock();

        // You can look up the property by ID instead of the string to be more efficient.
        block.SetColor("_BaseColor", c);

        // You can cache a reference to the renderer to avoid searching for it.
        GetComponentInChildren<Renderer>().SetPropertyBlock(block);
#endif
    }

    void Update()
    {
        float delta = mp.CurrentBeat - beat;
        if ((delta + duration) * speed > 0)
        {
            DestroyMe(true);
        }
        transform.position = new Vector3(pos.x, pos.y, -delta * speed);
    }
    void DestroyMe(bool success)
    {
        BeatManager.Instance.GetComponent<AudioSource>().PlayOneShot(success ? SuccessSound : FailedSound, 5);
        //foreach(var obj in OnDestoroyObjects)
        {
            //Instantiate(obj[UnityEngine.Random.Range(0,obj.Count)],transform.position,Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    public void OnRoutedCollisionEvent(Collision col)
    {
        if (col.gameObject.name != "HeadCollider") return;

        DestroyMe(false);
    }
}

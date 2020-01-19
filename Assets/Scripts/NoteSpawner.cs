using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject note;
    MusicPlayer musicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<MusicPlayer>();
    }

    public float lastNoteSpawned = 0;

    // Update is called once per frame
    void Update()
    {   
        while (lastNoteSpawned-5 < musicPlayer.CurrentBeat)
        {
            var n = Instantiate(note, Vector3.zero, Quaternion.identity).GetComponent<Note>();
            Vector2 rnd = Random.insideUnitCircle * .6f + new Vector2(0, 1f);
            HandSide handSide; HookSide hookSide = HookSide.none;
            if(Random.Range(0,20)<1)
                handSide = HandSide.left;
            else if(Random.Range(0,19)<1)
                handSide = HandSide.right;
            else
            {
                handSide = HandSide.any;
                switch (Random.Range(0,20))
                {
                    case 0:  hookSide = HookSide.up;    break;
                    case 1:  hookSide = HookSide.down;  break;
                    case 2:  hookSide = HookSide.left;  break;
                    case 3:  hookSide = HookSide.right; break;
                    default: hookSide = HookSide.none;  break;
                }
            }

            n.Init(lastNoteSpawned, rnd,musicPlayer, 1, handSide , hookSide);
            lastNoteSpawned++;
        }
        

    }
}

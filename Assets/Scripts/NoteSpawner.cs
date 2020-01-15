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
            
            n.Init(lastNoteSpawned, rnd,musicPlayer, 1, HandSide.any );
            lastNoteSpawned++;
        }
        

    }
}

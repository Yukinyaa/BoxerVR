using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class NoteSpawner : MonoBehaviour
{
    public GameObject note;
    MusicPlayer musicPlayer;
    public Transform zero, two;
    public TextAsset inputCsv;

    bool AutoConstruct = false;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<MusicPlayer>();

        var text = File.ReadAllText("./map.csv").Split('\n');


        List<string[]> csv = new List<string[]>();

        foreach (var line in text)
        {
            csv.Add(line.Split(','));
        }
        //vsersion
        if(csv[0][3]=="1")
        {
            AutoConstruct = false;
            parseV1(csv);
        }
        else if (csv[0][3]=="0")
        {
            AutoConstruct = true;
        }
    }
    public void parseV1(List<string[]> csv)
    {
        //sanity check
        Debug.Assert(csv[0][0]=="BPM");
        Debug.Assert(csv[0][2]=="v");
        Debug.Assert(csv[1][0]=="오프셋");
        Debug.Assert(csv[1][0]=="오프셋");
        Debug.Assert(csv[5][0]=="타이밍");
        int version = int.Parse(csv[0][3]);
        //parse BPM


        Vector2 z = new Vector2(zero.position.x,zero.position.y);
        Vector2 t = new Vector2(two.position.x,two.position.y);
        Vector2 ztdelta = t-z;
        

        musicPlayer.BPM = int.Parse(csv[0][1]);
        musicPlayer.offset = int.Parse(csv[1][1]);
        for(int i = 6 ; i < csv.Count ; i++ )
        {
            try
            {
                var line = csv[i];
                if (line.Length < 3) break;
                var timing = float.Parse(line[0]);
                timing = (int)timing + (timing % 1 / 4 * 10);// magic
                var x = float.Parse(line[1]);
                var y = float.Parse(line[2]);
                var n = Instantiate(note, Vector3.zero, Quaternion.identity).GetComponent<Note>();
                HandSide handSide;
                switch (line[3])
                {
                    case "왼손": handSide = HandSide.left; break;
                    case "오른손": handSide = HandSide.right; break;
                    case "상관없음": handSide = HandSide.any; break;
                    case "양손": handSide = HandSide.both; break;
                    default: throw new System.Exception(string.Format("Error parsing {0}:{1}", i, 3));
                }
                float? hookDir;
                switch (line[4])
                {
                    case "왼쪽": hookDir = 270; break;
                    case "오른쪽": hookDir = 90; break;
                    case "위": hookDir = 0; break;
                    case "아래": hookDir = 180; break;
                    case "왼쪽위": hookDir = 315; break;
                    case "오른쪽위": hookDir = 45; break;
                    case "왼쪽아래": hookDir = 225; break;
                    case "오른쪽아래": hookDir = 135; break;
                    case "상관없음": hookDir = null; break;
                    default: throw new System.Exception(string.Format("Error parsing {0}:{1}", i, 4));
                }
                n.Init(timing, z + ztdelta * new Vector2(x / 3, y / 3), musicPlayer, 1, handSide, hookDir);
            }
            catch (System.FormatException) { throw new System.Exception(string.Format("Error parsing {0}:{1}", i, "x")); }
        }
    }

    public float lastNoteSpawned = 0;

    // Update is called once per frame
    void Update()
    {   
        if(AutoConstruct)
            while (lastNoteSpawned-5 < musicPlayer.CurrentBeat)
            {
                var n = Instantiate(note, Vector3.zero, Quaternion.identity).GetComponent<Note>();
                Vector2 rnd = Random.insideUnitCircle * .6f + new Vector2(0, 1f);
                HandSide handSide = HandSide.any;
                float? hookSide = null;
                if(Random.Range(0,20)<1)
                    handSide = HandSide.left;
                else if(Random.Range(0,19)<1)
                    handSide = HandSide.right;
                else
                {
                    switch (Random.Range(0,20))
                    {
                        case 0:  hookSide = 0;    break;
                        case 1:  hookSide = 90;  break;
                        case 2:  hookSide = 270;  break;
                        case 3:  hookSide = 180; break;
                        default: hookSide = null;  break;
                    }
                }

                n.Init(lastNoteSpawned, rnd,musicPlayer, 1, handSide , hookSide);
                lastNoteSpawned++;
            }
        

    }
}

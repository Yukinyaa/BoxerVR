using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MusicPlayer))]
public class BeatManager : MonoBehaviour
{
    MusicPlayer mp;
    // Use this for initialization

    public static BeatManager Instance;

    void Start()
    {
        Instance = this;
        mp = GetComponent<MusicPlayer>();
    }


    //todo: 트릭 생각남, 마지막 true인것만 기억하고 그 이후만 true, 이전은 모두 false인걸로 간주함. 2차원 행렬 입력 받는 class로 대체하면 이쁠거같음. 시발 구현하기 귀찮다
    private List<bool[]> markChecker = new List<bool[]>();


    int lastBeat = 0;
    // Update is called once per frame
    void Update()
    {
        UpdateBeats();
    }
    void UpdateBeats()
    {
        var currentBeat = mp.CurrentBeat;
        while (currentBeat > markChecker.Count - 2)
                markChecker.Add(new bool[48]);// 48? citation : https://github.com/Yukinyaa/HBMS_Editor/blob/master/Beat%20Snap%20Color.xlsx
    }
    /// <summary>
    /// </summary>
    /// <param name="tolerance">in seconds</param>
    /// <param name="subBeat">
    ///  48/2 for half beat, 
    ///  48/3, 48/3*2 for 3rd beat
    /// </param>
    /// <returns>is this subbeat played?</returns>
    public bool CheckBeat(float tolerance,int subBeat = 0)
    {
        int? q = FindNearestBeat(tolerance, subBeat);
        if (q == null) return false;
        else return markChecker[(int)q][subBeat];
    }


    /// <summary>
    /// Consume subeat
    /// </summary>
    /// <param name="tolerance"></param>
    /// <param name="subBeat">
    ///  48/2 for half beat, 
    ///  48/3, 48/3*2 for 3rd beat
    ///  </param>
    /// <returns> is this beat consumed? </returns>
    public bool ComsumeBeat(float tolerance, int subBeat = 0)
    {
        UpdateBeats();
        int? q = FindNearestBeat(tolerance, subBeat);
        if (q == null) return false;
        else
        {
            //todo: check last 48 subbeat
            if (markChecker[(int)q][subBeat] == true) return false;
            markChecker[(int)q][subBeat] = true;
            return true;
        }
    }
    /// <summary>
    /// Consumes subeat
    /// </summary>
    /// <param name="tolerance"></param>
    /// <param name="subBeat">
    ///  48/2 for half beat, 
    ///  48/3, 48/3*2 for 3rd beat
    ///  </param>
    /// <returns> is current this subbeat? </returns>
    public bool IsSubBeat(float tolerance, int subBeat = 0)
    {
        UpdateBeats();
        int? q = FindNearestBeat(tolerance, subBeat);
        if (q == null) return false;
        else return true;
    }
    int? FindNearestBeat(float tolerance, int subBeat = 0)
    {
        UpdateBeats();
        var currentBeat = mp.CurrentBeat;
        int currentBeati = (int)currentBeat;
        float currentSubBeat = currentBeat - currentBeati;
        var toleranceInBeat = tolerance * mp.BPM / 60;

        float target = subBeat / 48f;
        Debug.Log("FNB - Delta: " + (currentBeati + target - currentBeat));
        if (Compare(currentBeat, currentBeati + target, toleranceInBeat))
        {
            
            return currentBeati;
        }
        else if (Compare(currentBeat, currentBeati - 1 + target, toleranceInBeat))
        {
            return currentBeati - 1;
        }
        else if ((Compare(currentBeat, currentBeati + 1 + target, toleranceInBeat)))
        {
            return currentBeati + 1;
        }
        return null;
    }

    public bool Compare(float a, float b, float tolerence)
    {
        Debug.Assert(tolerence >= 0);
        if (a - tolerence < b && b < a + tolerence) return true;
        else return false;
    }

}

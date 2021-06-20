using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
    GameManager singleton
 */
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    private int m_nScore = 0;
    public Text m_scoreText;

    private double m_dDeltaTime = .0f;
    public AudioClip[] m_audioClip;
    public int m_nAudioCurIndex = 0;

    //public SongHandler m_InstSongHandler;
    //public AudioSource m_InstAudioSource;

    public GameObject m_panel;
    public bool bPause = false;

    void Awake()
    {

    }

    void Start()
    {
        m_nScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_scoreText.text = m_nScore.ToString();
    }


    public int GetnScore()
    {
        return m_nScore;
    }
    public void SetnScore(int _nScore)
    {
        m_nScore = _nScore;
    }
}

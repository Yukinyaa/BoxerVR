using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class LoadMusicFromFile : MonoBehaviour
{
    IEnumerator Start()
    {
#if UNITY_EDITOR
        var reqest = UnityWebRequest.Get("file://" + Application.dataPath + "/music.mp3");
        Debug.Log("Directory is : " + "file://" + Application.dataPath + "/music.mp3");
#else
        var reqest = UnityWebRequest.Get("file://" + Application.dataPath + "/../music.mp3");
        Debug.Log("Directory is : "+ "file://" + Application.dataPath + "/../music.mp3");
#endif

        yield return reqest.SendWebRequest();
        while (reqest.downloadProgress < 1.0)
        {
            yield return null;
        }
        
        
        if (reqest.isNetworkError)
        {
            Debug.Log(reqest.error);
        }
        else
        {
            GetComponent<AudioSource>().clip = NAudioPlayer.FromMp3Data(reqest.downloadHandler.data);
        }
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

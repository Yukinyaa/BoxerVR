using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Note;
    void Start()
    {
        
        switch(Note.GetComponent<Note>().nIdx)
        {
            case 0:
                if(this.gameObject.name=="Arrow02")
                    gameObject.SetActive(false);
                if (this.gameObject.name == "Arrow03")
                    gameObject.SetActive(false);
                break;
            case 1:
                if (this.gameObject.name == "Arrow01")
                    gameObject.SetActive(false);
                if (this.gameObject.name == "Arrow03")
                    gameObject.SetActive(false);
                break;
            case 2:
                if (this.gameObject.name == "Arrow01")
                    gameObject.SetActive(false);
                if (this.gameObject.name == "Arrow02")
                    gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

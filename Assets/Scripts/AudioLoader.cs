using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public AudioManager theAudio;

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager newAudio = Instantiate(theAudio);
            AudioManager.instance = newAudio;
            DontDestroyOnLoad(newAudio.gameObject);
        }
    }

}

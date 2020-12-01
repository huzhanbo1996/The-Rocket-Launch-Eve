using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource click;
    public AudioSource error;
    public AudioSource quiz;
    public enum SOUND_TYPE { PICK, ERROR, QUIZ };
    public bool enable;
    // Start is called before the first frame update
    void Start()
    {
        enable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive(bool active)
    {
        enable = active;
    }

    public void Play(SOUND_TYPE st)
    {
        if (!enable) return;
        switch (st)
        {
            case SOUND_TYPE.PICK:
                //if(!click.isPlaying)
                    click.Play();
                break;
            case SOUND_TYPE.ERROR:
                error.Play();
                break;
            case SOUND_TYPE.QUIZ:
                quiz.Play();
                break;
            default:
                Debug.LogError("Unkown sound type");
                break;
        }

    }
}

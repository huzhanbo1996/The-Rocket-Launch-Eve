using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class Pre : MonoBehaviour
{
    public VideoPlayer mVP;
    public AudioSource BGM;
    public GameObject contactUs;
    public GameObject mBtnNext;
    public GameObject mBtnContact;
    public float mTimeAwait;
    public bool mCanContinue;
    private enum VIDEO_STATE { BEFORE, IN, AFTER };
    // Start is called before the first frame update
    void Start()
    {
        mCanContinue = false;
        mVP.loopPointReached += test;
        BGM.Play();
        contactUs.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (mVP.isPlaying && mCanContinue)
        {
            if(Input.anyKeyDown)
            {
                StartCoroutine(LoadYourAsyncScene());
                BGM.Stop();
            }
        }
        if (contactUs.activeSelf)
        {
            if(Common.Utils.ClickedAnywhereOut(contactUs))
            {
                contactUs.SetActive(false);
            }
        }
        else
        {
            if (Common.Utils.ClickedOn(mBtnContact))
            {
                contactUs.SetActive(true);
            }
            if(Common.Utils.ClickedOn(mBtnNext))
            {
                //BGM.Stop();
                if (File.Exists(Application.persistentDataPath + "save.dat"))
                {
                    StartCoroutine(LoadYourAsyncScene());
                    mBtnNext.SetActive(false);
                    return;
                }
                mVP.Play();
                StartCoroutine(Wait());
            }
        }   
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(mTimeAwait);
        mCanContinue = true;
    }

    void test(VideoPlayer video)//video = video2
    {
        SceneManager.LoadScene("main");
        this.gameObject.SetActive(false);
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("main");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        mVP.Stop();
        this.gameObject.SetActive(false);
    }
}

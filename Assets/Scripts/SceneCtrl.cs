using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCtrl : MonoBehaviour
{
    public List<GameObject> Scenes = new List<GameObject>();
    public List<GameObject> UIobjs = new List<GameObject>();
    public GameObject InitialScene;
    public FadeOut fadeOutUtil;

    private int mCurrSceneI, mCurrSceneJ;
    // Start is called before the first frame update
    void Start()
    { 
        mCurrSceneI = mCurrSceneJ = -1;
        mCurrSceneI = int.Parse(InitialScene.name.Split('_')[1]);
        mCurrSceneJ = int.Parse(InitialScene.name.Split('_')[2]);
        foreach (GameObject scene in Scenes)
        {
            if (scene.name != "Scene_" + mCurrSceneJ.ToString() + "_" + mCurrSceneJ.ToString())
            {
                scene.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private GameObject FindSceneByIndex(int i, int j)
    {
        foreach(GameObject scene in Scenes)
        {
            if(scene.name == "Scene_" + i.ToString() + "_" + j.ToString())
            {
                Debug.Log(scene.name);
                return scene;
            }
        }
        Debug.LogError("FindSceneByIndex ERROR!!!");
        return null;
    }

    public GameObject GetCurrScene()
    {
        return FindSceneByIndex(mCurrSceneI, mCurrSceneJ);
    }
    public void ChangeDirection(ChangeSceneButtom.Direction dir)
    {
        GameObject currScene = FindSceneByIndex(mCurrSceneI, mCurrSceneJ);
        switch (dir)
        {
            case ChangeSceneButtom.Direction.UP:
                mCurrSceneJ += 1;
                break;
            case ChangeSceneButtom.Direction.DOWN:
                mCurrSceneJ -= 1;
                break;
            case ChangeSceneButtom.Direction.LEFT:
                mCurrSceneI -= 1;
                break;
            case ChangeSceneButtom.Direction.RIGHT:
                mCurrSceneI += 1;
                break;
            default:
                Debug.LogError("Unkown direction");
                break;
        }
        fadeOutUtil.DoFadeOut();
        GameObject newScene = FindSceneByIndex(mCurrSceneI, mCurrSceneJ);
        newScene.SetActive(true);
        currScene.SetActive(false);
        Vector3 move = newScene.transform.position - currScene.transform.position;
        this.transform.position += move;
        foreach(GameObject ui in UIobjs)
        {
            ui.transform.position += move;
        }
    }
}

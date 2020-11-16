using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButtom : MonoBehaviour
{
    public Direction direction;

    public enum Direction { UP, DOWN, LEFT, RIGHT};
    private SceneCtrl sceneCtrl;
    // Start is called before the first frame update
    void Start()
    {
        sceneCtrl = FindObjectOfType<SceneCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Common.Utils.ClickedOn(this.gameObject))
        {
            sceneCtrl.ChangeDirection(direction);
        }
    }

}

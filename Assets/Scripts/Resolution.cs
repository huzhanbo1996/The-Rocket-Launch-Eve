using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    public Camera mCamera;

    private void Start()
    {
        float designWidth = 1920;//开发时分辨率宽
        float designHeight = 1080;//开发时分辨率高
        float designOrthographicSize = 5.4f;//开发时正交摄像机Size
        float designScale = designWidth / designHeight;
        float scaleRate = (float)Screen.width / (float)Screen.height;
        //当前分辨率大于开发分辨率，会自动缩放，小于的时候需要手动处理如下
        if (scaleRate < designScale)
        {
            float scale = scaleRate / designScale;
            mCamera.orthographicSize = designOrthographicSize / scale;
        }
        else
        {
            mCamera.orthographicSize = designOrthographicSize;
        }
    }

    private void Update()
    {
        
    }
}

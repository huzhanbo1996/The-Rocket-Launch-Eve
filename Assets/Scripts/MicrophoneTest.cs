using UnityEngine;
using System.Collections.Generic;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class MicrophoneTest : MonoBehaviour {

#if PLATFORM_ANDROID

    private void OnEnable() {
        // App.MsgSystem.MsgSystem.MSG_CONFIRMED.AddListener( HandleAnswer );
    }

    private void OnDisable() {
        // App.MsgSystem.MsgSystem.MSG_CONFIRMED.RemoveListener( HandleAnswer );
    }

    private void Start() {

    }

    private void Update() {

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
        
    }


#endif
}
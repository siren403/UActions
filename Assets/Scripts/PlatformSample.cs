using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSample : MonoBehaviour
{
    private void OnGUI()
    {
#if UNITY_STANDALONE_WIN
        GUILayout.Label("win");
#elif UNITY_ANDROID
        GUILayout.Label("android");
#elif UNITY_IOS
        GUILayout.Label("ios");
#endif
    }
}
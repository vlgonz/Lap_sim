#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class ButtonExit : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        // If running in the Unity Editor, exit play mode
        EditorApplication.ExitPlaymode();
#else
        // If running as a standalone build, quit the application
        Application.Quit();
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string scene2; // Name of the second scene you want to load

    public void LoadNextScene()
    {
        SceneManager.LoadScene(scene2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string aScene;

    public void changeScene()
    {
        SceneManager.LoadScene(aScene);
    }

    public void quit()
    {
        Application.Quit();
    }
}

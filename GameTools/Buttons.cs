using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [Tooltip("What scene should this take the player to.")] public string aScene;
    [Tooltip("Transition type. 0 = Inlevel move, 1 = UnityScene change, 2 = quitgame")] public int whatScript = -1;

    private GameObject fadeCanvas;
    private string gameLevel = "Game";
    
    [Tooltip("Doors are within-level transitions.")]public bool isDoor = false;
    [Tooltip("If checked, door locks the camera.")] public bool cameraLocks = false;
    [Tooltip("If door, then where in scene does it take you.")] public Vector2 newPos;
    public bool changesMonologue;
    public string newMonologue;

    public bool timeSkip;

    void Awake()
    {
        fadeCanvas = GameObject.FindWithTag("Fade");
    }
   
    public void trigger()
    {
        StartCoroutine(change());
    }

    public IEnumerator change()
    {
        if(fadeCanvas != null)
            yield return StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(whatScript, gameLevel));

        if (isDoor)
        {
            //StopCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(whatScript, gameLevel));
            GameObject.FindWithTag("Player").transform.position = new Vector3(newPos.x, newPos.y, 0);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(whatScript, gameLevel));

            if (cameraLocks)
                GameObject.FindWithTag("MainCamera").transform.GetComponent<CameraFollow>().lockFollow();
            else
            {
                GameObject.FindWithTag("MainCamera").transform.GetComponent<CameraFollow>().unlockFollow();
                GameObject.FindWithTag("MainCamera").transform.GetComponent<CameraFollow>().resetCamera();
            }

            if(changesMonologue)
            {
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.currentMonologue = newMonologue;
            }

            if(timeSkip)
            {
                GameObject.FindWithTag("GameController").GetComponent<TimeSkip>().triggerChange();
            }
        }
        yield return new WaitForSeconds(0.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    [SerializeField, Tooltip("How fast the fade is")] private float alphaChange = -0.25f;
    private float maxWait = 0.05f;
    private IEnumerator fadeCoroutine;

    void Awake()
    {
        transform.GetComponent<CanvasGroup>().alpha = 1.0f;
        fadeCoroutine = screenFade(0, "");
        StartCoroutine(fadeCoroutine);
    }

    public void triggerFade(int whatScript, string aLevel)
    {
        fadeCoroutine = screenFade(whatScript, aLevel);
        StartCoroutine(fadeCoroutine);
    }

    public IEnumerator screenFade(int transition, string aLevel)
    {
        float finalAlpha = 0.5f + (2.0f * alphaChange);
        float wait = maxWait;

        if (finalAlpha < transform.GetComponent<CanvasGroup>().alpha)
        {
            while (transform.GetComponent<CanvasGroup>().alpha > finalAlpha)
            {
                if (wait > 0)
                    wait -= Time.deltaTime;
                else
                {
                    transform.GetComponent<CanvasGroup>().alpha += alphaChange;
                    wait = maxWait;
                }
                yield return null;
            }
        }

        else
        {
            while (transform.GetComponent<CanvasGroup>().alpha < finalAlpha)
            {
                if (wait > 0)
                    wait -= Time.deltaTime;
                else
                {
                    transform.GetComponent<CanvasGroup>().alpha += alphaChange;
                    wait = maxWait;
                }
                yield return null;
            }
        }

        alphaChange *= -1.0f;

        if (finalAlpha > 0.9f)
        {
            switch(transition)
            {
                case 0:                                     //DO nothing
                    break;

                case 1:
                    SceneManager.LoadScene(aLevel);         //Change to another unity scene
                    break;

                case 2:
                    Application.Quit();                     //Quit the game
                    break;

                case 3:
                    SceneManager.LoadScene(0);              //Reset to MainMenu
                    break;

                default:
                    break;

            }
        }
    }
}

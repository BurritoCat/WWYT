using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TextShow : MonoBehaviour
{
    public string[] textToShow;
    public TMP_Text text;
    private int wait, maxWait = 10;
    int arrayIndex, sentenceIndex;
    public bool hasStarted = false;
    public bool changesMonologue;
    public string newMonologue;

    public bool endGame = false;

    void Start()
    {
        wait = maxWait;
    }
    void Update()
    {
        if (transform.childCount == 0 && hasStarted)
            hasStarted = false;

        if (transform.childCount > 0 && !hasStarted)
        {
            if (endGame)
                GameObject.Find("GExit").GetComponent<Buttons>().trigger();
            arrayIndex = 0;
            sentenceIndex = 0;
            text = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = "";
            hasStarted = true;
        }

        else if (hasStarted)
        {
            if (wait > 0)
                wait--;

            else
            {
                wait = maxWait;
                if (sentenceIndex < textToShow[arrayIndex].Length)   //If string has more text, display it
                {
                    text.text += textToShow[arrayIndex][sentenceIndex++];
                }
            }
        }
    }

    public bool ContinueText()
    {
        if (arrayIndex < textToShow.Length - 1)
        {
            if (sentenceIndex >= textToShow[arrayIndex].Length)
            {
                text.text = "";
                arrayIndex++;
                sentenceIndex = 0;
            }
            else 
            {
                sentenceIndex = textToShow[arrayIndex].Length;
                text.text = textToShow[arrayIndex];
            }
                return false;
        }

        else
        {
            if (sentenceIndex < textToShow[arrayIndex].Length)
            {
                sentenceIndex = textToShow[arrayIndex].Length;
                text.text = textToShow[arrayIndex];

                return false;
            }
            else
            {
                if (this.gameObject.tag == "object")
                {
                    Destroy(this.gameObject.transform.GetChild(0).gameObject);
                    if(changesMonologue)
                        GameObject.FindWithTag("Player").GetComponent<Player>().currentMonologue = newMonologue;
                }
                else if (this.gameObject.tag == "monologue")
                    Destroy(this.gameObject);
                return true;
            }
        }
    }
}
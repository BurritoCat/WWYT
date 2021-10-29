using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeChangeDialogue : MonoBehaviour
{
    public string[] textToShow;
    public TMP_Text text;
    private int wait, maxWait = 10;
    int arrayIndex, sentenceIndex;
    bool hasStarted = false;
    public bool wasMinigameFailed = false;
    public DialogueTreeNode[] afternoonDialogues;

    void Start()
    {
        wait = maxWait;
    }
    void Update()
    {
        if (transform.childCount == 0 && hasStarted)
            hasStarted = false;

        if (GameObject.FindGameObjectsWithTag("MGText").Length > 0 && !hasStarted)
        {
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
                StartCoroutine(timeChange());

                Destroy(this.gameObject.transform.GetChild(0).gameObject);
                return true;
            }
        }
    }

    private IEnumerator timeChange()
    {
        GameObject fadeCanvas = GameObject.FindWithTag("Fade");
        yield return StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(0, ""));

        //Here, have to do the changing of in-scene objects.
        //Hardcode demo changes for now, later must relegate to another script.
        ///////////////////////////////////////////////
        this.gameObject.tag = "object";
        this.gameObject.AddComponent<TextShow>();
        this.gameObject.GetComponent<TextShow>().textToShow = new string[1] { "That's enough of that for now." };
        GameObject.FindWithTag("Player").GetComponent<Player>().changeTime();

        Dialogue charD = GameObject.FindWithTag("character").GetComponent<Dialogue>();
        SideCharacter otherChar = GameObject.FindWithTag("character").GetComponent<SideCharacter>();
        if (wasMinigameFailed)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().changeTime();
            GameObject.FindWithTag("Player").GetComponent<Player>().changeHealth(-5);
        }
        if (otherChar.characterFavor >= 100)
        {
            otherChar.setUpConversation(afternoonDialogues[0]);
        }

        else
        {
            otherChar.setUpConversation(afternoonDialogues[1]);
        }
        charD.textToShow = otherChar.Conversation;

        //Here, must wait for previous changes to be done. Right now, simply a wait for seconds.
        //Must change for a coroutine.
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(0, ""));

        ////////////////////////////////////////////
        
        GameObject.FindWithTag("Player").GetComponent<Player>().playerInput.SwitchCurrentActionMap("Player");
        GameObject.FindWithTag("Player").GetComponent<Player>().changeMove();
    }
}

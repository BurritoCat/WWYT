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
    public DialogueTreeNode[] newDialogues;

    void Start()
    {
        wait = maxWait;
    }
    void Update()
    {
        if (transform.childCount == 0 && hasStarted)
            hasStarted = false;

        if (GameObject.FindGameObjectsWithTag("MGText").Length > 0 && !hasStarted && this.gameObject.tag == "minigame")
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
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();

        //Here, must do the changing of in-scene objects.
        ///////////////////////////////////////////////

        if (this.gameObject.tag == "minigame") 
        {

            this.gameObject.tag = "object";
            this.gameObject.AddComponent<TextShow>();
            this.gameObject.GetComponent<TextShow>().textToShow = new string[1] { "That's enough of that for now." };
            GameObject.FindWithTag("Player").GetComponent<Player>().changeTime();

            Dialogue charD = GameObject.FindWithTag("character").GetComponent<Dialogue>();
            SideCharacter otherChar = GameObject.FindWithTag("character").GetComponent<SideCharacter>();
        
            if (wasMinigameFailed)
            {
                GameObject.FindWithTag("Player").GetComponent<Player>().changeTime();
                GameObject.FindWithTag("character").GetComponent<SideCharacter>().setFavor(-5);
                DialogueTreeNode chosenConvo = otherChar.ConversationNode.nextConversations[1];
                otherChar.setUpConversation(chosenConvo);
            }

            else
            {
                GameObject.FindWithTag("character").GetComponent<SideCharacter>().setFavor(5);
                DialogueTreeNode chosenConvo = otherChar.ConversationNode.nextConversations[0];
                otherChar.setUpConversation(chosenConvo);
            }

            charD.textToShow = otherChar.Conversation;

            GameObject.Find("HPhone").GetComponent<TextShow>().textToShow = new string[] { "A phone.", "My phone." };   
            
        }
        //Display transitions
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(0, ""));

        ////////////////////////////////////////////

        player.playerInput.SwitchCurrentActionMap("Player");
        player.changeMove();
        player.currentMonologue = "Better go see the boss";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public string[] textToShow;
    public TMP_Text text;
    private int wait, maxWait = 10;
    [SerializeField] bool needsResponse;
    int arrayIndex, sentenceIndex;
    bool hasStarted = false;

    void Start()
    {
        wait = maxWait;
    }
    void Update()
    {
        //Error check: Make sure text variables properly reset.
        if (transform.childCount < 2 && hasStarted)
            hasStarted = false;

        //If a textbox is found, and dialogue hasn't started yet,
        //start dialogue.
        if (transform.childCount > 1 && !hasStarted)
        {
            arrayIndex = 0;
            sentenceIndex = 0;
            text = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = "";
            hasStarted = true;
        }

        //Otherwise, continue dialogue.
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

    public int ContinueConversation(bool hasResponse, int[] responseNumbers)  // Return: 0 = continue, 1 = end, 2 = response
    {
        //Error check: index cannot be 1 less that textlength.
        
        //Text is not done: progress it and return a 0.
        if (arrayIndex < textToShow.Length - 1)
        {
            //If current text is done, then
            //progress to next text to show.
            if (sentenceIndex >= textToShow[arrayIndex].Length)
            {
                text.text = "";
                arrayIndex++;
                sentenceIndex = 0;
            }

            //If current text is not done,
            //fast forward it.
            else
            {
                sentenceIndex = textToShow[arrayIndex].Length;
                text.text = textToShow[arrayIndex];
            }
            return 0;
        }

        //Final text, either fast forward it or finish dialogue
        //and destroy text box.
        else
        {
            //Fast-forward text if not done.
            if (sentenceIndex < textToShow[arrayIndex].Length)
            {
                sentenceIndex = textToShow[arrayIndex].Length;
                text.text = textToShow[arrayIndex];

                return 0;
            }

            else
            {
                //If final text for this dialogue needs a response
                //ask for it and return.
                if (needsResponse && !hasResponse)
                    return 2;

                else
                {
                    //If have user response, then change dialogue and continue.
                    if(needsResponse && hasResponse)
                    {
                        changeConversation(responseNumbers);
                        return 0;
                    }

                    int[] temp = { 0 };
                    textToShow = this.gameObject.GetComponent<SideCharacter>().setUpConversation(temp);
                    text.text = "";
                    arrayIndex = 0;
                    sentenceIndex = 0;

                    //Otherwise, destroy dialogue box.
                    Destroy(this.gameObject.transform.GetChild(0).gameObject);
                    return 1;
                }
            }
        }
    }

    //Function to get a new conversation set from Database.
    private void changeConversation(int[] indexes)
    {
        textToShow = this.gameObject.GetComponent<SideCharacter>().setUpConversation(indexes);
        text.text = "";
        arrayIndex = 0;
        sentenceIndex = 0;
    }

    public void changeResponse(bool aBool)
    {
        needsResponse = aBool;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDatabase : MonoBehaviour
{
    public GameObject player;
    private int DBIndex = 0;
    string[][] Conversations = new string[][] 
    {
        new string[9] {
            //First 3 - Negative criticism, question, and comment.
            "It really shouldn't be this hard.","Couldn't handle it yourself?", "I don't appreciate coming in on my day off.",
        
            //Next 3 - Neutral criticism, question, and comment.
            "That's what you said last time.", "What's the problem?", "Not how I planned to spend my afternoon.",

            //Next 3 - Positive criticism, question, and comment.
            "This job will kill you if you keep this up.", "You good to go?","Ready when you are."
        }
        ,
        new string[9]
        {
            "I think you overdid it.", "You just fired him like that?", "I think that was too much.",
            "I don't think this solves anything.", "Was that necessary?", "More work for us then.",
            "Guess he wasn't cut out for this.", "Was there no way to keep him on?", "We just have to work twice as hard."
        }
    };

    public void displayText(int anIndex)
    {
        GameObject.FindWithTag("PlayerDialogue").GetComponent<TMP_Text>().text = Conversations[DBIndex][anIndex];
    }

    public void clearText()
    {
        GameObject.FindWithTag("PlayerDialogue").GetComponent<TMP_Text>().text = "";
    }

    public void directText(string text)
    {
        GameObject.FindWithTag("nameChoice").GetComponent<TMP_Text>().text = text;
    }

    public void increaseDBIndex()
    {
        DBIndex++;
    }
}

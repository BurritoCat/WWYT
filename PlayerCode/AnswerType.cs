using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerType : MonoBehaviour
{
    string[] answers = {
        "Negative", "Neutral", "Positive",
        "Criticize", "Question", "Comment"        
        };

    string[] names = { "Josh", "Lauren", "Keith", "Me"};

    public void SetText(int index)
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = answers[index];
    }

    public void SetNames(int index)
    {
        if (index == -1)
        {
            transform.GetChild(0).GetComponent<TMP_Text>().text = "Me";
            return;
        }

        transform.GetChild(0).GetComponent<TMP_Text>().text = names[index];
    }

    public void SetGlow(bool value)
    {
        if (!value)
        {
            GetComponent<Image>().color = new Color32(152, 187, 190, 255);
        }

        else
        {
            GetComponent<Image>().color = new Color32(124, 164, 168, 255);
        }

    }
}

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

    public void SetText(int index)
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = answers[index];
    }

    public void SetGlow(bool value)
    {
        if (!value)
        {
            GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        else
        {
            GetComponent<Image>().color = new Color32(0, 222, 5, 150);
        }

    }
}

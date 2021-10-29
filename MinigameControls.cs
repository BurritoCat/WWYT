using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class MinigameControls : MonoBehaviour
{
    //Press the correct Key Minigame
    [SerializeField] TMP_Text text;
    public int[] whatKeys = new int[6];
    private int currentIndex = 0;
    private bool started = false;
    [SerializeField] private float time = 10.0f;
    private bool failed = false, won = false;
    [SerializeField] private GameObject minigameCanvas;
    
    public string[][] possibleText = new string [][]
    {
        new string[2] {"Alright, nothing too serious.", "Should be done real quick." },
        new string[2] {"Crap. I can't make any sense of this.", "It'll take forever." }
    };
    
    void Update()
    {
        if(started && time > 0f && !won)
        {
            time -= Time.deltaTime;
        }

        if(time <= 0f && !failed && !won)
        {
            failed = true;
            GameObject player = GameObject.FindWithTag("Player");
            gameObject.GetComponent<TimeChangeDialogue>().textToShow = possibleText[1];
            gameObject.GetComponent<TimeChangeDialogue>().wasMinigameFailed = true;
            Destroy(this.gameObject.transform.GetChild(0).gameObject); 
            player.GetComponent<Player>().minigameEnd();
        }
    }
    
    public void startMinigame()
    {
        var PopUp = Instantiate(minigameCanvas, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
        PopUp.transform.SetParent(transform);
        PopUp.transform.SetSiblingIndex(0);

        for(int i = 0; i < 6; i++)
            whatKeys[i] = Random.Range(0, 4);

        text = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
        displayCurrentKey();
        started = true;
        failed = false;
    }

    public void displayCurrentKey()
    {
        string keyDisplay = "";
        switch (whatKeys[currentIndex])
        {
            case 0:
                keyDisplay = "A";
                break;

            case 1:
                keyDisplay = "D";
                break;

            case 2:
                keyDisplay = "S";
                break;

            case 3:
                keyDisplay = "W";
                break;
        }

        text.text = keyDisplay;
    }
    public bool minigameMove(Vector2 movementKeys)
    {

        int playerChoice = -1;

        //If x-axis, then y-axis disabled. Necessary?

        if (movementKeys.x != 0)
        {
            movementKeys.y = 0;
            if (movementKeys.x < 0)
                playerChoice = 0;
            else
                playerChoice = 1;
        }

        else if (movementKeys.y != 0)
        {
            movementKeys.x = 0;
            if (movementKeys.y < 0)
                playerChoice = 2;
            else
                playerChoice = 3;
        }

        if(playerChoice == whatKeys[currentIndex])
        {
            if(currentIndex == 5) 
            {
                won = true;
                gameObject.GetComponent<TimeChangeDialogue>().textToShow = possibleText[0];

                Destroy(this.gameObject.transform.GetChild(0).gameObject);
                Destroy(this);

                return true;
            }
            else 
            {
                currentIndex++;
                displayCurrentKey();
                return false;
            }
        }
        else 
        {
            return false;
        }
    }
}

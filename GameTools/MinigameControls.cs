using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class MinigameControls : MonoBehaviour
{
    //Press the correct Key Minigame
    [SerializeField] TMP_Text text;
    public int[] whatKeys;
    public int numberOfKeys = 6;
    private int currentIndex = 0;
    private bool started = false;
    [SerializeField] private float time = 10.0f, maxTime = 10f;
    public int repeats = 1, currentIt = 1;
    private bool failed = false, won = false;
    [SerializeField] private GameObject minigameCanvas, progressBar;
    public GameObject countdownBar;


    public string[][] possibleText = new string [][]
    {
        new string[2] {"Alright, nothing too serious.", "Should be done real quick." },
        new string[2] {"Crap. I can't make any sense of this.", "This isn't good." }
    };
    
    void Update()
    {
        if(started && time > 0f && !won)
        {
            time -= Time.deltaTime;

            float fullPercent = (float)time / (float)maxTime;
            if (fullPercent > 1)
                fullPercent = 1;
            else if (fullPercent < 0)
                fullPercent = 0;

            countdownBar.GetComponent<ProgressBar>().SetFill(fullPercent);
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
        //Reset time, and create the object to display keys
        time = maxTime;
        failed = false;
        won = false;
        var PopUp = Instantiate(minigameCanvas, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
        PopUp.transform.SetParent(transform);
        PopUp.transform.SetSiblingIndex(0);
        
        //Instantiate the ProgessBar
        var Bar = Instantiate(progressBar, new Vector3(transform.position.x, transform.position.y + 2.4f, transform.position.z), Quaternion.identity);
        Vector3 temp = Bar.transform.localScale;
        temp.Set(0.5f, 0.5f, 1);
        Bar.transform.localScale = temp;

        Bar.transform.SetParent(transform);
        Bar.transform.SetSiblingIndex(1);

        Canvas BarCanvas = Bar.GetComponent<Canvas>();
        BarCanvas.sortingLayerName = "UI";
        BarCanvas.sortingOrder = 1;

        countdownBar = Bar;

        text = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();

        populateArray();

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

    private void populateArray()
    {
        //Depending on the current number of keys, make an array.
        whatKeys = new int[numberOfKeys];
        for (int i = 0; i < numberOfKeys; i++)
            whatKeys[i] = Random.Range(0, 4);
        displayCurrentKey();
    }

    public bool minigameMove(Vector2 movementKeys)
    {

        int playerChoice = -1;

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
            if(currentIndex == numberOfKeys - 1)
            {
                if (currentIt < repeats)
                {
                    currentIndex = 0;
                    currentIt++;
                    populateArray();

                    time = maxTime;

                    return false;
                }

                won = true;
                gameObject.GetComponent<TimeChangeDialogue>().textToShow = possibleText[0];
                currentIt = 0;
                Destroy(this.gameObject.transform.GetChild(0).gameObject);
                this.enabled = false;
                Destroy(GameObject.FindWithTag("ProgressBar"));

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

    public void IncreaseDifficulty()
    {
        numberOfKeys += 2;
        repeats += 2;
        maxTime += 1;
    }
}

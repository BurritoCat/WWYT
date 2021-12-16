using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Resource variables
    public int money = 5000,                 // Divide by 100 to find display value.
                            timeOfDay = 0;   // 0 - morning, 1 - afternoon, 2 - evening
    [SerializeField] private GameObject clockUI, moneyUI;

    //Variables for movement
    Vector2 movementInput;
    private bool isMoving = false;
    private float speed = 0.01f;
    private bool canMove = true, minigameEnded = false;
    [HideInInspector] public PlayerInput playerInput;
    SpriteRenderer sprite;
    [SerializeField] bool allowedThroughDoors = false;

    //Variables for dialogue options
    private int selection = 0;
    [HideInInspector] public int[] currentAnswer = new int[2] { 0, 0 };
    private int nextAnswer = 0;
    private string currentMonologue = "Phone's ringing...";
    bool dialogueOptions = false;
    [SerializeField]int nameIndex = 0;

    //Prefab spots
    [Tooltip("Prefab of the textbox object")] public GameObject dialogueBox;
    [Tooltip("Prefab of the response choice object")] public GameObject optionBox;

    //Variables for collision
    float adjustX = 0.0f, moveAmount = 1.5f;
    private bool inRange = false;
    private Collider2D target;

    //Variables for minigames
    public GameObject currentMinigame;

    // Start is called before the first frame update
    void Awake()
    {
        /*
        healthUI = GameObject.FindWithTag("heart");
        changeHealth(0);
        */
        clockUI = GameObject.FindWithTag("clock");
        moneyUI = GameObject.FindWithTag("money");
        changeMoney(0);
    }

    // Update is called once per frame
    void Update()
    {
        //If in motion, then move according to keyboard inputs
        if (isMoving && canMove)
        {
            if (movementInput.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);

            else if (movementInput.x < 0)
                transform.localScale = new Vector3(1, 1, 1);

            transform.position = new Vector3(transform.position.x + movementInput.x, transform.position.y, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D aCol)
    {
        //If colliding with interactibles, then set target for interaction.
        if (validCollision(aCol))
        {
            inRange = true;
            target = aCol;
        }
    }

    void OnTriggerExit2D(Collider2D aCol)
    {
        //When exiting collision with interactibles, can no longer interact.
        if (validCollision(aCol))
        {
            inRange = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        //If key is pressed, action depends if players can move.
        if (context.performed)
        {
            //Normal movement if players can move.
            if (canMove && !minigameEnded)
            {
                movementInput = (context.ReadValue<Vector2>()) * speed;

                isMoving = true;
            }

            //Option movement if players cannot move
            else if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    if (child.tag == "monologue")
                    {
                        return;
                    }
                }
                Transform option;

                if (dialogueOptions)
                    option = transform.GetChild(0).transform.GetChild(selection + 1).transform.GetChild(0);
                
                else 
                    option = transform.GetChild(0).transform.GetChild(selection).transform.GetChild(0);

                option.GetComponent<AnswerType>().SetGlow(false);
                movementInput = (context.ReadValue<Vector2>());

                selection += ((int)movementInput.y);

                //Math to calculate current option
                if (dialogueOptions)
                {
                    if (selection > 2 + (nextAnswer * 3)) selection = 2 + (nextAnswer * 3);
                    selection %= (3 + (nextAnswer * 3));
                    if (selection < 0 + (nextAnswer * 3)) selection = (nextAnswer * 3);

                    option = transform.GetChild(0).transform.GetChild(selection + 1).transform.GetChild(0);
                    //Highlight selected answer.
                    option.GetComponent<AnswerType>().SetGlow(true);

                    //If two options highlighted, display what the text is.
                    if (nextAnswer > 0)
                    {
                        int currentText = currentAnswer[0] * 3;
                        currentText += selection - 3;
                        GameObject.FindWithTag("GameController").GetComponent<PlayerDatabase>().displayText(currentText);
                    }
                }

                else
                {
                    if (selection > 1)
                        selection = 1;
                    if (selection < 0)
                        selection = 0;

                    option = transform.GetChild(0).transform.GetChild(selection).transform.GetChild(0);
                    //Highlight selected answer.
                    option.GetComponent<AnswerType>().SetGlow(true);

                    string blame = "It was " + option.GetChild(0).GetComponent<TMP_Text>().text;
                    GameObject.FindWithTag("GameController").GetComponent<PlayerDatabase>().directText(blame);
                }
            }
        }

        //If key is released, stop moving
        if (context.canceled)
        {
            isMoving = false;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        //If in range for interacting, interact base on target.
        if (context.performed && inRange)
        {
            if (canMove)
            {
                //Cannot move
                canMove = !canMove;
                isMoving = false;

                if (GameObject.Find("DialogueBox(Clone)"))
                    Destroy(GameObject.Find("DialogueBox(Clone)"));

                if (target.gameObject.tag == "minigame")
                {
                    if (target.gameObject.GetComponent<MinigameControls>().enabled == false)
                        target.gameObject.GetComponent<MinigameControls>().enabled = true;
                    playerInput.SwitchCurrentActionMap("UI");
                    currentMinigame = target.gameObject;
                    currentMinigame.GetComponent<MinigameControls>().startMinigame();
                    
                    return;
                }

                //If it's a door, trigger the jump.
                if (target.gameObject.tag == "door")
                {
                    if (!allowedThroughDoors)
                    {
                        canMove = true;
                        createMonologue();
                        return;
                    }
                    //target.gameObject.GetComponent<Buttons>().trigger();
                    StartCoroutine("doorTravel");
                    return;
                }

                //If interaction was just started, set position of text box.
                //Objects are above player, dialogue is above other character.
                adjustX = 0;
                Transform pos;
                if (target.gameObject.tag == "object")
                    pos = transform;
                else
                {
                    pos = target.gameObject.transform.parent.transform;
                    if (transform.position.x > pos.position.x)
                        adjustX = -1.0f;
                    else
                        adjustX = 1.0f;
                }

                //Create text box at position.
                var PopUp = Instantiate(dialogueBox, new Vector3(pos.position.x + (moveAmount * adjustX), pos.position.y + moveAmount, pos.position.z), Quaternion.identity);
                if (target.gameObject.tag == "character")
                    PopUp.transform.SetParent(target.gameObject.transform.parent.transform);
                else
                    PopUp.transform.SetParent(target.gameObject.transform);
                PopUp.transform.SetSiblingIndex(0);
            }

            else
            {
                if(target.gameObject.tag == "minigame")
                {
                    if (!minigameEnded)
                    {
                        return;
                    }

                    bool endText = target.gameObject.GetComponent<TimeChangeDialogue>().ContinueText();

                    if (endText)
                    {
                        minigameEnded = false;
                        playerInput.SwitchCurrentActionMap("Player");
                        canMove = true;
                    }
                    return;
                }

                //If interaction was already started, continue with text
                //until done.
                if (target.gameObject.tag == "object")
                {
                    //Call relevant object function to check
                    //if text is over.
                    bool endObject = target.gameObject.GetComponent<TextShow>().ContinueText();
                    if (endObject)
                        canMove = true;
                }

                else
                {
                    //Check if player has a dialogue box attached,
                    //which means that player has to give an answer.
                    bool response;
                    if (transform.childCount < 1)
                        response = false;
                    else if(dialogueOptions)
                    {
                        currentAnswer[nextAnswer] = selection;
                        if (nextAnswer == 0)
                        {
                            //Move selection to next point
                            //and skip responding.
                            nextAnswer = 1;
                            selection = 3;

                            Transform option = transform.GetChild(0).transform.GetChild(selection + 1).transform.GetChild(0);

                            //Highlight selected answer.
                            option.GetComponent<AnswerType>().SetGlow(true);

                            int currentText = currentAnswer[0] * 3;
                            currentText += selection - 3;
                            GameObject.FindWithTag("GameController").GetComponent<PlayerDatabase>().displayText(currentText);
                            
                            return;
                        }

                        //If currentAnswer value is 3 or more, then
                        //player has given a full answer. Reset variables
                        //and pass this value.
                        nextAnswer = 0;
                        response = true;

                        if (transform.childCount >= 1)
                            Destroy(this.gameObject.transform.GetChild(0).gameObject);
                    }
                    
                    //If not dialogueOptions, then it's picking a name.
                    //Only need the first index in currentAnswer;
                    else
                    {
                        if(selection == 0)
                            nameIndex++;

                        currentAnswer[0] = selection;
                        response = true;
                        Destroy(this.gameObject.transform.GetChild(0).gameObject);
                    }
                    //Check if conversation ends,
                    //passing the player's answer.
                    int endConversation = target.gameObject.transform.parent.GetComponent<Dialogue>().ContinueConversation(response, currentAnswer);
                    currentAnswer = new int[] {0,0};

                    //if dialogue passed a 1,
                    //conversation is over.
                    if (endConversation == 1)
                    {
                        canMove = true;
                        if (transform.childCount >= 1)
                            Destroy(this.gameObject.transform.GetChild(0).gameObject);
                        selection = 0;
                    }

                    //If dialogue passed a 2,
                    //A response is needed, create text box.
                    else if (endConversation == 2)
                    {
                        createResponseField();
                    }

                    else if(endConversation == 3)
                    {
                        namesField();
                    }
                    //Otherwise, passed a 0 and nothing else happens.
                }
            }
        }
    }

    public void Return(InputAction.CallbackContext context)
    {
        //Code for displaying a hint.
        if(context.performed && transform.childCount == 0 && canMove)
        {
            createMonologue();
        }

        else if(context.performed  && !canMove)
        {
            if (!(transform.gameObject.transform.childCount > 0 && transform.gameObject.transform.GetChild(0).transform.childCount > 1))
                return;
            if (nextAnswer > 0)
            {
                Transform option = transform.GetChild(0).transform.GetChild(selection + 1).transform.GetChild(0);
                option.GetComponent<AnswerType>().SetGlow(false);
                currentAnswer[nextAnswer] = 0;
                nextAnswer = 0;
                selection = currentAnswer[nextAnswer];
                option = transform.GetChild(0).transform.GetChild(selection + 1).transform.GetChild(0);
                option.GetComponent<AnswerType>().SetGlow(true);
                GameObject.FindWithTag("GameController").GetComponent<PlayerDatabase>().clearText();
                return;
            }
        }
    }

    public void createMonologue()
    {
        if (GameObject.Find("DialogueBox(Clone)"))
            return;

        var PopUp = Instantiate(dialogueBox, new Vector3(transform.position.x + (moveAmount * adjustX), transform.position.y + moveAmount, transform.position.z), Quaternion.identity);
        PopUp.transform.SetParent(transform.parent.transform);
        PopUp.transform.SetSiblingIndex(0);

        PopUp.transform.gameObject.AddComponent<SelfDelete>();
        TMP_Text text = PopUp.transform.GetChild(0).transform.GetChild(0).transform.GetComponent<TMP_Text>();
        text.text = currentMonologue;

        return;
    }

    private IEnumerator doorTravel()
    {
        yield return target.gameObject.GetComponent<Buttons>().change();
        canMove = !canMove;
    }

    private void createResponseField()
    {
        dialogueOptions = true;
        Vector3 spot = new Vector3(transform.position.x + (-moveAmount* adjustX), transform.position.y + moveAmount * 0.8f, transform.position.z);
       
        //Create big dialogue box
        var PopUp = Instantiate(dialogueBox, new Vector3(spot.x, spot.y + 0.7f, spot.z), Quaternion.identity);
        PopUp.transform.SetParent(transform);
        PopUp.transform.GetChild(0).transform.GetChild(0).transform.gameObject.tag = "PlayerDialogue";

        float xOffset, xPattern, yOffset;
        //Create smaller option boxes
        for (int boxNumber = 0; boxNumber < 6; boxNumber++)
        {
            //Math for identifying each option's position.
            xOffset = Mathf.Floor((boxNumber)/3);
            xPattern = boxNumber % 2;
            yOffset = boxNumber % 3;

            var create = Instantiate(optionBox, new Vector3(spot.x - 1.9f + (4.05f * xOffset) - xPattern * 0.25f, spot.y + (0.75f * yOffset), spot.z), Quaternion.identity);
            create.transform.SetParent(transform.GetChild(0));
            create.transform.SetSiblingIndex(boxNumber + 1);
            create.transform.GetChild(0).GetComponent<AnswerType>().SetText(boxNumber);

            if (boxNumber == 0)
                create.transform.GetChild(0).transform.GetComponent<AnswerType>().SetGlow(true);
        }

        return;
    }

    public void namesField()
    {
        dialogueOptions = false;
        Vector3 spot = new Vector3(transform.position.x + (-moveAmount * adjustX), transform.position.y + moveAmount * 0.8f, transform.position.z);

        //Create big dialogue box
        var PopUp = Instantiate(dialogueBox, new Vector3(spot.x, spot.y + 0.7f, spot.z), Quaternion.identity);
        PopUp.transform.SetParent(transform);
        PopUp.transform.GetChild(0).transform.GetChild(0).transform.gameObject.tag = "nameChoice";

        //Create smaller option boxes
        var create = Instantiate(optionBox, new Vector3(spot.x - 1.9f, spot.y + 0.25f, spot.z), Quaternion.identity);
        create.transform.SetParent(transform.GetChild(0));
        create.transform.SetSiblingIndex(0);
        create.transform.GetChild(0).GetComponent<AnswerType>().SetNames(nameIndex);
        create.transform.GetChild(0).GetComponent<AnswerType>().SetGlow(true);

        string blame = "It was " + create.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text;
        GameObject.FindWithTag("GameController").GetComponent<PlayerDatabase>().directText(blame);

        create = Instantiate(optionBox, new Vector3(spot.x - 1.9f, spot.y + 1.0f, spot.z), Quaternion.identity);
        create.transform.SetParent(transform.GetChild(0));
        create.transform.SetSiblingIndex(1);
        create.transform.GetChild(0).GetComponent<AnswerType>().SetNames(-1);

        return;
    }

    public void setMove(bool aBool) { canMove = aBool; }

    bool validCollision(Collider2D aCol)
    {
        if (aCol.gameObject.tag == "object" || aCol.gameObject.tag == "character" || aCol.gameObject.tag == "door" || aCol.gameObject.tag == "minigame")
            return true;
        else return false;
    }

    public void quitGame()
    {
        SceneManager.LoadScene("MainMenu");         //Change to MM scene
        return;
    }

    ///////////////////////////////////////////////////////////////
    ///                     UI SCRIPTS                          ///
    ///////////////////////////////////////////////////////////////

    public void minigameMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentMinigame != null)
            {
                movementInput = (context.ReadValue<Vector2>());
                bool canEnd = currentMinigame.GetComponent<MinigameControls>().minigameMove(movementInput);
                movementInput = new Vector2(0, 0);

                if (canEnd)
                    minigameEnd();

                return;
            }
        }
    }
    public void minigameEnd()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Input.ResetInputAxes();
        minigameEnded = true;
        
        var PopUp = Instantiate(dialogueBox, new Vector3(transform.position.x + (moveAmount * adjustX), transform.position.y + moveAmount, transform.position.z), Quaternion.identity);
        PopUp.transform.SetParent(target.gameObject.transform);
        PopUp.transform.SetSiblingIndex(0);
        PopUp.transform.gameObject.tag = "MGText";

        return;
    }

    public void removeRange()
    {
        inRange = false;
    }
    public void changeTime()
    {
        timeOfDay++;
        if(timeOfDay > 2)
        {
            //End day function

            timeOfDay = 0;
        }

        clockUI.transform.GetComponent<Clock>().getTime(timeOfDay);
    }

    public void changeMoney(int aNum)
    {
        money += aNum;
        moneyUI.transform.GetComponent<Money>().getMoney(money);
    }

    public void changeMove()
    {
        canMove = true;
    }

    public int getIndex()
    {
        return nameIndex;
    }

    public void changeMonologue(string aString)
    {
        currentMonologue = aString;
    }

    public void allowDoors(bool aBool)
    {
        allowedThroughDoors = aBool;
    }

    public bool getDoors()
    {
        return allowedThroughDoors;
    }
}

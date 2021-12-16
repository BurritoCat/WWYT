using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSkip : MonoBehaviour
{
    public DialogueTreeNode[] newDialogues;
    public int dialoguesIndex;
    public void triggerChange()
    {
        StartCoroutine(timeChange());
    }

    private IEnumerator timeChange()
    {
        GameObject fadeCanvas = GameObject.FindWithTag("Fade");
        yield return StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(0, ""));

        //Here, must do the changing of in-scene objects.
        ///////////////////////////////////////////////
        Player ply = GameObject.FindWithTag("Player").GetComponent<Player>();
        //ply.enabled = false;
        while (ply.timeOfDay != 0)
            GameObject.FindWithTag("Player").GetComponent<Player>().changeTime();

        Dialogue charD = GameObject.FindWithTag("character").GetComponent<Dialogue>();
        SideCharacter otherChar = GameObject.FindWithTag("character").GetComponent<SideCharacter>();

        if (otherChar.characterFavor >= 100)
        {
            if(dialoguesIndex == 0)
                otherChar.setUpConversation(newDialogues[dialoguesIndex]);
        }

        else
        {
            if(ply.getIndex() == 0)
            {
                if (dialoguesIndex == 0)
                    otherChar.setUpConversation(newDialogues[dialoguesIndex + 2]);
            }

            else
            {
                if (dialoguesIndex == 0)
                    otherChar.setUpConversation(newDialogues[dialoguesIndex + 1]);
                TextShow door = GameObject.Find("FJoshDoor").GetComponent<TextShow>();
                door.textToShow = new string[] { "An empty office.", "No reason to go in." };
            }
        }

        if(dialoguesIndex > 1)
        {
            GameObject.Find("Player1").GetComponent<Player>().allowDoors(false);
            GameObject.Find("HPhone").GetComponent<TextShow>().endGame = true;
        }

        //Change computer back to being a minigame
        GameObject MGObject = GameObject.Find("MinigameObject");
        //MGObject.GetComponent<MinigameControls>().enabled = true;
        Destroy(MGObject.GetComponent<TextShow>());
        MGObject.tag = "minigame";

        MGObject.GetComponent<MinigameControls>().IncreaseDifficulty();

        //Reset office exit door
        Buttons someDoor = GameObject.Find("FExit").GetComponent<Buttons>();
        someDoor.newPos = new Vector2(78.3f, 1.5f);
        
        Buttons busStop = GameObject.Find("SOfficeDoor").GetComponent<Buttons>();
        someDoor.changeMonologue("Better go see the boss");
        
        charD.textToShow = otherChar.Conversation;
        dialoguesIndex += 2;

        MGObject.GetComponent<MinigameControls>().Reset();

        //Display transitions
        yield return new WaitForSeconds(0.5f);
        ply.removeRange();
        StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(0, ""));
    }
}

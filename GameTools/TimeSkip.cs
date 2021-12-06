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
        while (ply.timeOfDay != 0)
            GameObject.FindWithTag("Player").GetComponent<Player>().changeTime();

        Dialogue charD = GameObject.FindWithTag("character").GetComponent<Dialogue>();
        SideCharacter otherChar = GameObject.FindWithTag("character").GetComponent<SideCharacter>();

        if (otherChar.characterFavor >= 100)
        {
            otherChar.setUpConversation(newDialogues[dialoguesIndex]);
        }

        else
        {
            otherChar.setUpConversation(newDialogues[dialoguesIndex + 1]);
            if(dialoguesIndex==0)
            {
                TextShow door = GameObject.Find("FJoshDoor").GetComponent<TextShow>();
                door.textToShow = new string[] { "An empty office.", "No reason to go in." };
            }
        }

        if(dialoguesIndex > 1)
        {
            GameObject.Find("HPhone").GetComponent<TextShow>().endGame = true;
        }

        GameObject MGObject = GameObject.Find("MinigameObject");
        Destroy(MGObject.GetComponent<TextShow>());
        MGObject.tag = "minigame";
        MGObject.GetComponent<MinigameControls>().enabled = true;
        MGObject.GetComponent<MinigameControls>().IncreaseDifficulty();

        Buttons someDoor = GameObject.Find("FExit").GetComponent<Buttons>();
        someDoor.newPos = new Vector2(78.3f, 1.5f);
        
            someDoor.changesMonologue = false;
        
        charD.textToShow = otherChar.Conversation;
        dialoguesIndex += 2;

        //Display transitions
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fadeCanvas.GetComponent<ScreenFade>().screenFade(0, ""));

        ////////////////////////////////////////////
        /*
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.playerInput.SwitchCurrentActionMap("Player");
        player.changeMove();
        player.currentMonologue = "Better go see the boss";
        */
    }
}

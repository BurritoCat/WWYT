using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCharacter : MonoBehaviour
{
    public DialogueTreeNode ConversationNode;
    public string[] Conversation;
    public int characterFavor = 100; //0 - hate, 250 - love
    public int[] characterPreferences = {
        0,0,0,
        0,0,0,
        0,0,0 };

    public string[] setUpConversation(int[] indexes)
    {
        if (ConversationNode.changesDoor)
        {
            Debug.Log("Must change door " + GameObject.Find(ConversationNode.whichDoor));
            Buttons someDoor = GameObject.Find(ConversationNode.whichDoor).GetComponent<Buttons>();
            someDoor.newPos = new Vector2(ConversationNode.newX, ConversationNode.newY);
            if (ConversationNode.whatMonologue != "")
            {
                someDoor.changesMonologue = true;
                someDoor.newMonologue = ConversationNode.whatMonologue;
                someDoor.timeSkip = true;
            }
        }
        if (ConversationNode.isConversationEnd)
            return ConversationNode.stringsInConv;
        else
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (ConversationNode.affectsMoney)
                player.changeMoney(ConversationNode.moneyAmount);
            if (ConversationNode.affectsTime)
                player.changeTime();

            if (ConversationNode.nextConversations.Length == 1)
            {
                ConversationNode = ConversationNode.nextConversations[0];
                Conversation = ConversationNode.stringsInConv;
                this.gameObject.GetComponent<Dialogue>().changeResponse(ConversationNode.needsResponse);
                this.gameObject.GetComponent<Dialogue>().changeCharacter(ConversationNode.pickSomeone);
                if (ConversationNode.changesMonologue)
                    player.currentMonologue = ConversationNode.newMonologue;
                if (ConversationNode.changesDoor)
                {
                    Debug.Log("Must change door " + GameObject.Find(ConversationNode.whichDoor));
                    GameObject.Find(ConversationNode.whichDoor).GetComponent<Buttons>().newPos = new Vector2(ConversationNode.newX, ConversationNode.newY);
                }
            }

            else
            {
                int trueIndex = 0;
                //Needs math eq to calculate number
                trueIndex += indexes[0] * 3;
                trueIndex += indexes[1] - 3;
                characterFavor += characterPreferences[trueIndex];

                ConversationNode = ConversationNode.nextConversations[trueIndex];
                Conversation = ConversationNode.stringsInConv;
                this.gameObject.GetComponent<Dialogue>().changeResponse(ConversationNode.needsResponse);
                this.gameObject.GetComponent<Dialogue>().changeCharacter(ConversationNode.pickSomeone);
                if (ConversationNode.changesMonologue)
                    player.currentMonologue = ConversationNode.newMonologue;
            }

        }
        return Conversation;
    }

    public string[] setUpConversation(int index)
    {
        if (index == 1)
            characterFavor -= 5;

        if (ConversationNode.isConversationEnd)
            return ConversationNode.stringsInConv;

        else
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (ConversationNode.affectsMoney)
                player.GetComponent<Player>().changeMoney(ConversationNode.moneyAmount);
            if (ConversationNode.affectsTime)
                player.GetComponent<Player>().changeTime();

            if (ConversationNode.nextConversations.Length == 1)
            {
                ConversationNode = ConversationNode.nextConversations[0];
                Conversation = ConversationNode.stringsInConv;
                this.gameObject.GetComponent<Dialogue>().changeResponse(ConversationNode.needsResponse);
            }

            else
            {
                ConversationNode = ConversationNode.nextConversations[index];
                Conversation = ConversationNode.stringsInConv;
                this.gameObject.GetComponent<Dialogue>().changeResponse(ConversationNode.needsResponse);
                this.gameObject.GetComponent<Dialogue>().changeCharacter(ConversationNode.pickSomeone);
            }

        }

        return Conversation;
    }

    public void setUpConversation(DialogueTreeNode aNode)
    {
        ConversationNode = aNode;
        Conversation = ConversationNode.stringsInConv;
        this.gameObject.GetComponent<Dialogue>().changeResponse(ConversationNode.needsResponse);
        this.gameObject.GetComponent<Dialogue>().changeCharacter(ConversationNode.pickSomeone);
    }

    public void setFavor(int aNum)
    {
        characterFavor += aNum;
    }
}
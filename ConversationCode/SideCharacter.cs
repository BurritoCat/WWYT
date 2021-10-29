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
        if (ConversationNode.isConversationEnd)
            return ConversationNode.stringsInConv;

        else
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (ConversationNode.affectsHealth)
                player.GetComponent<Player>().changeHealth(ConversationNode.healthAmount);
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
                int trueIndex = 0;
                //Needs math eq to calculate number
                trueIndex += indexes[0] * 3;
                trueIndex += indexes[1] - 3;
                characterFavor += characterPreferences[trueIndex];

                ConversationNode = ConversationNode.nextConversations[trueIndex];
                Conversation = ConversationNode.stringsInConv;
                this.gameObject.GetComponent<Dialogue>().changeResponse(ConversationNode.needsResponse);
            }

        }
       
        return Conversation;
    }

    public void setUpConversation(DialogueTreeNode aNode)
    {
        ConversationNode = aNode;
        Conversation = ConversationNode.stringsInConv;
        this.gameObject.GetComponent<Dialogue>().changeResponse(ConversationNode.needsResponse);
    }
}

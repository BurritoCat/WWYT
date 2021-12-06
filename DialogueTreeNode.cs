using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "DialogueNode", order = 1)]
public class DialogueTreeNode : ScriptableObject
{
    public string[] stringsInConv;
    public DialogueTreeNode[] nextConversations;
    public bool needsResponse;
    public bool pickSomeone;
    public bool isConversationEnd;

    public bool affectsMoney;
    public int moneyAmount;

    public bool affectsTime;

    public bool changesMonologue;
    public string newMonologue;

    public bool changesDoor;
    public string whichDoor, whatMonologue;
    public float newX, newY;
}
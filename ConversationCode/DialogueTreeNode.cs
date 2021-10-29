using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "DialogueNode", order = 1)]
public class DialogueTreeNode : ScriptableObject
{
    public string[] stringsInConv;
    public DialogueTreeNode[] nextConversations;
    public bool needsResponse;
    public bool isConversationEnd;

    public bool affectsHealth;
    public int healthAmount;

    public bool affectsMoney;
    public int moneyAmount;

    public bool affectsTime;
}

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
}

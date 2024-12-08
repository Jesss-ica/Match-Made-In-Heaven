using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public NPC npc;
    public string playerName;
    bool inConversation;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Interact();
        } else if (Input.GetMouseButtonUp(0))
        {
            Invoke("Interact",0.1f);

        }
    }
    public void Interact()
    {
        if (LoggerMulti.instance.lineRead == true)
        {
            if (inConversation)
            {
                LoggerMulti.instance.SkipLine();
            }
            else
            {
                LoggerMulti.instance.StartDialogue(npc.dialogueAsset, npc.StartPosition, npc.npcName,playerName ,npc.spriteArray, npc.spriteRenderer);
            }
        }
    }

    void JoinConversation()
    {
        inConversation = true;
    }

    void LeaveConversation()
    {
        inConversation = false;
    }

    private void OnEnable()
    {
        LoggerMulti.OnDialogueStarted += JoinConversation;
        LoggerMulti.OnDialogueEnded += LeaveConversation;
    }

    private void OnDisable()
    {
        LoggerMulti.OnDialogueStarted -= JoinConversation;
        LoggerMulti.OnDialogueEnded -= LeaveConversation;
    }
}

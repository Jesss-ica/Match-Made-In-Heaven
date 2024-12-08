using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueTree : ScriptableObject
{
    [TextArea]
    public string notes;

    public DialogueSection[] sections;

    [System.Serializable]
    public struct DialogueSection
    {
        public Log[] dialogue;
        public bool endAfterDialogue;
        public BranchPoint branchPoint;
    }

    [System.Serializable]
    public struct Log
    {
        [TextArea]
        public string dialogue;
        [Range(0, 5)]
        public int emote;

    }

    [System.Serializable]
    public struct BranchPoint
    {
        [TextArea]
        public string question;
        [Range(0, 5)]
        public int emote;
        public Answer[] answers;
    }

    [System.Serializable]
    public struct Answer
    {
        public string answerLabel;
        public int nextElement;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] bool firstInteraction = true;
    [SerializeField] int repeatStartPosition;

    public string npcName;
    public DialogueTree dialogueAsset;

    [HideInInspector] public SpriteRenderer spriteRenderer; 
    public Sprite[] spriteArray;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [HideInInspector]
    public int StartPosition
    {
        get
        {
            if (firstInteraction)
            {
                firstInteraction = false;
                return 0;
            }
            else
            {
                return repeatStartPosition;
            }
        }
    }
}
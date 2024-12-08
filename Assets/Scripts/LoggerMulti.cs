using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using static DialogueTree;
//using UnityEditor.PackageManager.Requests;
using System.Net;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class LoggerMulti : MonoBehaviour
{
    public static LoggerMulti instance;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject buttonPrompt;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject answerBox;
    [SerializeField] UnityEngine.UI.Button[] answerObjects;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;

    public AudioManager audioManager;
    public AudioSource Blop;

    bool skipLineTriggered;
    bool answerTriggered;
    int answerIndex;
    string username;

    SpriteRenderer npcSpriteRenderer;
    [HideInInspector] public Sprite[] npcSprites;

    float charactersPerSecond = 50;
    [HideInInspector] public bool lineRead = true;

  private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    IEnumerator TypeTextUncapped(string line)
    {
        lineRead = false;
        audioManager.PlayAudio(Blop);
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;
        audioManager.PlayAudio(dialogueBox.GetComponent<AudioSource>());

        while (i < chars.Length)
        {
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                dialogueText.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        audioManager.StopAudio(dialogueBox.GetComponent<AudioSource>());
        lineRead = true;
    }

    public void StartDialogue(DialogueTree dialogueTree, int startSection, string name,string playerName,Sprite[] spriteArray, SpriteRenderer spriteRenderer)
    {
        ResetBox();

        npcSprites = spriteArray;
        npcSpriteRenderer = spriteRenderer;

        username = playerName;

        nameText.text = name + ":";
        buttonPrompt.SetActive(false);
        dialogueBox.SetActive(true);
        OnDialogueStarted?.Invoke();
        StartCoroutine(RunDialogue(dialogueTree, startSection));
    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {
            while (lineRead == false)
            {
                yield return null;
            }
            //dialogueText.text = dialogueTree.sections[section].dialogue[i];
            string logReplaced = dialogueTree.sections[section].dialogue[i].dialogue.Replace("@",username);
            StartCoroutine(TypeTextUncapped(logReplaced));
            CharacterEmote(dialogueTree.sections[section].dialogue[i].emote);
            while (skipLineTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
        }


        if (dialogueTree.sections[section].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke();
            dialogueBox.SetActive(false);
            yield break;
        }

        EventSystem.current.SetSelectedGameObject(null);
        //dialogueText.text = dialogueTree.sections[section].branchPoint.question;

        while (lineRead == false)
        {
            yield return null;
        }

        string questionLogReplaced = dialogueTree.sections[section].branchPoint.question.Replace("@", username);
        StartCoroutine(TypeTextUncapped(questionLogReplaced));
        CharacterEmote(dialogueTree.sections[section].branchPoint.emote);

        while (lineRead == false)
        {
            yield return null;
        }

        ShowAnswers(dialogueTree.sections[section].branchPoint);

        while (answerTriggered == false)
        {
            yield return null;
        }

        answerBox.SetActive(false);
        answerTriggered = false;

        StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement));
    }

    void ResetBox()
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        answerBox.SetActive(false);
        skipLineTriggered = false;
        answerTriggered = false;
    }

    void CharacterEmote(int emote)
    {
        for (int i = 0; i < npcSprites.Length; i++)
        {
            if (i == emote)
            {
                npcSpriteRenderer.sprite = npcSprites[i];
            }
        }
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        // Reveals the aselectable answers and sets their text values
        float bpAnsLength = branchPoint.answers.Length;
        float btnHeight = 1 / bpAnsLength;
        int loopInt = 0;

        answerBox.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (i < branchPoint.answers.Length)
            {
                RectTransform rect = answerObjects[i].GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, btnHeight * loopInt);
                loopInt++;
                rect.anchorMax = new Vector2(1, btnHeight * loopInt);
                answerObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = branchPoint.answers[i].answerLabel;
                answerObjects[i].gameObject.SetActive(true);
            }
            else
            {
                answerObjects[i].gameObject.SetActive(false);
            }
        }
        answerObjects[branchPoint.answers.Length - 1].Select();
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        answerIndex = answer;
        answerTriggered = true;
    }
}
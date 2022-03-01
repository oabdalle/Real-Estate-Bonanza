using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public GameObject continueButton;
    public LevelLoader levelLoader;

    private Queue<string> sentences;
    private Story currentStory;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    // Start is called before the first frame update
    void Start()
    {   
        //get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices){
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
         
    }

    public void StartDialogue(Dialogue dialogue, TextAsset inkJSON)
    {
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;
        currentStory = new Story(inkJSON.text);
        ContinueConvo();
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

    public void ContinueConvo()
    {
        if(currentStory.canContinue){
            string sentence = currentStory.Continue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
            DisplayChoices(); //Display choice if any
        }else{
            EndDialogue();
             Scene scene = SceneManager.GetActiveScene();
            if(scene.name == "End")
                SceneManager.LoadScene("menu");
            else
                levelLoader.LoadNextLevel();
        }
    }
    private void DisplayChoices(){
        List<Choice> currentChoices = currentStory.currentChoices;
        if(currentChoices.Count > choices.Length){
            Debug.LogError("More choices then allowed " + currentChoices.Count);
        }
        int index = 0;
        //Enable and initialize # of choices for this line
        foreach(Choice choice in currentChoices){
            choices[index].gameObject.SetActive(true);
            continueButton.SetActive(false);
            choicesText[index].text = choice.text;
            index++;
        }
        for(int i = index; i < choices.Length; i++){
            choices[i].gameObject.SetActive(false);
        }
        if(index == 0) //No choices were given -> show continue
            continueButton.SetActive(true);
    }
    
    public void MakeChoice(int choiceIndex){
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueConvo();
    }
}

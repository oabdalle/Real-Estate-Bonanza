using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;
	public GameObject startButton;
	[Header("Ink JSON")]
	[SerializeField] private TextAsset inkJson;

	public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue, inkJson);
		startButton.SetActive(false);
	}
}

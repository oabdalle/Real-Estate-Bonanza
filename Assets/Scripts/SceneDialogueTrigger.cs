using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;
	[Header("Ink JSON")]
	[SerializeField] private TextAsset inkJson;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, inkJson);
    }
}

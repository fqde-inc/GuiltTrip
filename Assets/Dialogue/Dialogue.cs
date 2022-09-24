using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public float waitTime;

    private int index;
    public AudioSource audioSource;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray()){
            textComponent.text += c;
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(textSpeed);
        }
        
        yield return new WaitForSeconds(waitTime);
        NextLine();
    }

    void NextLine()
    {
        if(index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            gameObject.SetActive(false);
        }
    }
}

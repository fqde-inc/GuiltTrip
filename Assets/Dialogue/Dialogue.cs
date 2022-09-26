using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using URPGlitch.Runtime.AnalogGlitch;
using URPGlitch.Runtime.DigitalGlitch;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string AddColor(this char c, Color col) => $"<color={ColorHexFromUnityColor(col)}>{c}</color>";
    public static string AddColor(this string c, Color col) => $"<color={ColorHexFromUnityColor(col)}>{c}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGB(unityColor)}";
    
}
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI authorTextComponent;
    public TextMeshProUGUI textComponent;
    public DialogueLines lines;
    public float textSpeed;

    public Volume volume;

    private int index;
    public AudioSource audioSource;
    public AudioClip clip;
    [Serializable]
    public struct Tags {
        public char _char;
        public Color color;
    }
    public Tags[] tags;
    public Dictionary<char, Color> tagDictionary;

    public Color textColor;
    public Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        tagDictionary = new Dictionary<char, Color>();
        foreach(var tag in tags) {
           tagDictionary.Add(tag._char, tag.color); 
        }
        gameObject.SetActive(false);
    }

    void Awake() {
        //textColor = textColor == null ? textComponent.color : textColor;

    }
    // Update is called once per frame
    void Update()
    {
        //textComponent.ForceMeshUpdate();
        //var textInfo = textComponent;

        //var textToWobble = GetStrBetweenTags(textInfo.text, "<wobble>","</wobble>");

        //var verts = textInfo.mesh[charInfo.materialReferenceIndex].vertices;
    }

    public void StartDialogue(DialogueLines _lines){
        StopAllCoroutines();
        textComponent.text = string.Empty;
        authorTextComponent.text = string.Empty;
        index = 0;
        lines = _lines;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        DialogueLines.Line line = lines.lines[index];

        if(line.author == "GLITCH"){
            StartCoroutine(Glitch());
            yield return new WaitForSeconds(line.waitingTime);
            NextLine();
            yield break;
        } else if(lines.lines[index].author == "WAIT") {
            Image img = this.GetComponent<Image>();
            img.enabled = false;
            yield return new WaitForSeconds(line.waitingTime);
            img.enabled = true;
            NextLine();
            yield break;
        }

        Dictionary<string, Color> table = new Dictionary<string, Color>(){
            { "Wolf" , Color.grey },
            { "Jack" , Color.yellow },
            { "???" , Color.white },
            { "Caith" , Color.magenta }
        };
        
        authorTextComponent.text = $"{line.author.AddColor(table[line.author])}";

        var textarray = line.text.ToCharArray();
        bool skipNext = false;
        for(int i = 0; i < textarray.Length; i++){
            if(skipNext){
                skipNext = false;
                continue;
            }

            char c = textarray[i];

            if (c == '$'){
                var next = textarray[i + 1];

                if( next == ' ' || next == '\0') 
                    continue;

                else if( next == 'g' ) 
                    StartCoroutine(Glitch());

                else if( Char.IsDigit(next) )
                    yield return new WaitForSeconds( (float) Char.GetNumericValue(next) / 5 );

                skipNext = true;
                continue;
            }

            if (tagDictionary.ContainsKey(c)) {
                currentColor = currentColor == textColor ? tagDictionary[c] : textColor;
                continue;
            }
            
            textComponent.text += $"{c.AddColor(currentColor)}";
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(textSpeed);
        }
        
        yield return new WaitForSeconds(line.waitingTime);
        NextLine();
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void NextLine()
    {
        if(index < lines.lines.Count - 1){  
            index++;
            textComponent.text = string.Empty;
            authorTextComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            gameObject.SetActive(false);
        }
    }

    IEnumerator ColorRainbow()
    {
        while (true) {
            for (int i = 0; i < textComponent.textInfo.characterCount; ++i) {
                string hexcolor = Rainbow(textComponent.textInfo.characterCount * 5, i + (int)Time.deltaTime);
                Color32 myColor32 = hexToColor(hexcolor);

                int meshIndex = textComponent.textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textComponent.textInfo.characterInfo[i].vertexIndex;
                Color32[] vertexColors = textComponent.textInfo.meshInfo[meshIndex].colors32;

                vertexColors[vertexIndex + 0] = myColor32;
                vertexColors[vertexIndex + 1] = myColor32;
                vertexColors[vertexIndex + 2] = myColor32;
                vertexColors[vertexIndex + 3] = myColor32;
            }
            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            yield return new WaitForSeconds(0.001f);
        }
    }
 
    public static Color32 hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
 
    public static string Rainbow(int numOfSteps, int step)
    {
        var r = 0.0;
        var g = 0.0;
        var b = 0.0;
        var h = (double)step / numOfSteps;
        var i = (int)(h * 6);
        var f = h * 6.0 - i;
        var q = 1 - f;
        switch (i % 6)
        {
            case 0:
                r = 1;
                g = f;
                b = 0;
                break;
            case 1:
                r = q;
                g = 1;
                b = 0;
                break;
            case 2:
                r = 0;
                g = 1;
                b = f;
                break;
            case 3:
                r = 0;
                g = q;
                b = 1;
                break;
            case 4:
                r = f;
                g = 0;
                b = 1;
                break;
            case 5:
                r = 1;
                g = 0;
                b = q;
                break;
        }
        return "#" + ((int)(r * 255)).ToString("X2") + ((int)(g * 255)).ToString("X2") + ((int)(b * 255)).ToString("X2");
    }
 

    IEnumerator Glitch()
    {
        AnalogGlitchVolume agl;
        DigitalGlitchVolume dgl;

        if (volume.profile.TryGet<AnalogGlitchVolume>(out agl)){
            agl.active = true;
        }
        if (volume.profile.TryGet<DigitalGlitchVolume>(out dgl)){
            dgl.active = true;
        }

        yield return new WaitForSeconds(0.2f);
        
        if (volume.profile.TryGet<AnalogGlitchVolume>(out agl)){
            agl.active = false;
        }
        if (volume.profile.TryGet<DigitalGlitchVolume>(out dgl)){
            dgl.active = false;
        }

        yield return new WaitForSeconds(0.05f);

        if (volume.profile.TryGet<AnalogGlitchVolume>(out agl)){
            agl.active = true;
        }
        if (volume.profile.TryGet<DigitalGlitchVolume>(out dgl)){
            dgl.active = true;
        }

        yield return new WaitForSeconds(0.2f);
        
        if (volume.profile.TryGet<AnalogGlitchVolume>(out agl)){
            agl.active = false;
        }
        if (volume.profile.TryGet<DigitalGlitchVolume>(out dgl)){
            dgl.active = false;
        }

    }
}

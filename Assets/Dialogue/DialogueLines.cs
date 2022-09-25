using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public class DialogueLines
{
    [Serializable]
    public struct Line {
        public string author;
        public string text;
        public float waitingTime;
        public Dictionary<Line,DialogueLines> choices;
    }
    public float waitingTime;

    public List<Line> lines;
    public int index;

}
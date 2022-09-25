using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using URPGlitch.Runtime.AnalogGlitch;
using URPGlitch.Runtime.DigitalGlitch;

public class Checkpoint : MonoBehaviour{

    public Volume volume;
    public Dialogue system;

    public DialogueLines lines;
    
    private bool passed;
    public bool mustGlitch;
    public GameObject NPC;

    private void OnTriggerEnter(Collider other)
    {
        if (passed) 
            return;
        passed = true;

        if (mustGlitch) 
            StartCoroutine( Glitch() );
        
        if (NPC != null)
            NPC.active = true;

        StartCoroutine( WriteLines() );

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

        yield return new WaitForSeconds(0.01f);

        if (volume.profile.TryGet<AnalogGlitchVolume>(out agl)){
            agl.active = true;
        }
        if (volume.profile.TryGet<DigitalGlitchVolume>(out dgl)){
            dgl.active = true;
        }

        yield return new WaitForSeconds(0.1f);
        
        if (volume.profile.TryGet<AnalogGlitchVolume>(out agl)){
            agl.active = false;
        }
        if (volume.profile.TryGet<DigitalGlitchVolume>(out dgl)){
            dgl.active = false;
        }

        yield return new WaitForSeconds(0f);
    }

    
    IEnumerator WriteLines()
    {
        yield return new WaitForSeconds(0.31f);

        system.StartDialogue(lines);
    }
}

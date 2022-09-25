using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using URPGlitch.Runtime.AnalogGlitch;
using URPGlitch.Runtime.DigitalGlitch;

public class Checkpoint : MonoBehaviour{

    public Volume volume;
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine( Glitch() );
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

        yield return new WaitForSeconds(0.5f);
        
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

        yield return new WaitForSeconds(0.2f);
        
        if (volume.profile.TryGet<AnalogGlitchVolume>(out agl)){
            agl.active = false;
        }
        if (volume.profile.TryGet<DigitalGlitchVolume>(out dgl)){
            dgl.active = false;
        }

        yield return new WaitForSeconds(0f);
    }
}

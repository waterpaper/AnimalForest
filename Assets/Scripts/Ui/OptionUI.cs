using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider EffectSlider;
    
    void Awake()
    {
        BGMSlider = transform.GetChild(0).GetChild(1).GetComponent<Slider>();
        EffectSlider = transform.GetChild(0).GetChild(2).GetComponent<Slider>();
    }

    private void OnEnable()
    {
        BGMSlider.value = SoundManager.instance.BGMSoundVolume;
        EffectSlider.value = SoundManager.instance.EffectSoundVolume;
    }

    private void LateUpdate()
    {
        if(BGMSlider.value != SoundManager.instance.BGMSoundVolume)
        {
            SoundManager.instance.BGMSoundVolumeLeveling(BGMSlider.value);
        }

        if(EffectSlider.value != SoundManager.instance.EffectSoundVolume)
        {
            SoundManager.instance.EffectSoundVolumeLeveling(EffectSlider.value);
        }
    }

    public void ExitButton()
    {
        UIManager.instance.UISetting(UiKind.UiKind_OptionUi);
    }
}

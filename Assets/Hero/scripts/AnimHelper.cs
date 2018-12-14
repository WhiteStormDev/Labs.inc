using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHelper : MonoBehaviour {
    [SerializeField] public bool isTurned = true;
    [SerializeField] private AudioSource legAudioSource;
    [SerializeField] private List<AudioClip> standartFootSteps;
    [SerializeField] private List<AudioClip> ladderFootSteps;
    

    //events_______________________________________
    void StepStandartSoundPlayOneShot()
    {
        if (standartFootSteps.Count == 0) return;
        legAudioSource.PlayOneShot(standartFootSteps[Random.Range(0, standartFootSteps.Count)]);
    }
    void StepLadderSoundPlayOneShot()
    {
        if (ladderFootSteps.Count == 0) return;
        legAudioSource.PlayOneShot(ladderFootSteps[Random.Range(0, ladderFootSteps.Count)]);
    }
    
}

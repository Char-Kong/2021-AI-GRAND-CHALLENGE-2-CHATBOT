using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class SetTextToSpeech
//{
//    public SetInput input;
//    public SetVoice voice;
//    public SetAudioConfig audioConfig;
//
//}

public class LipSyncManager : MonoBehaviour
{
    internal string Input;
    private Image imageComponent;
    private float WaitTimeBetweenCharacters;
    private float WaitTimeBetweenWords;
    private float WaitTimeBetweenSentences;
    internal bool PutInputIntoQueue;

    // Start is called before the first frame update
    private void Start()
    {
        Input = "안녕하세요";
        imageComponent = transform.Find("Character1_Reference").GetComponent<Image>();
        WaitTimeBetweenCharacters = 0.052f;
        WaitTimeBetweenWords = 0.08f;
        WaitTimeBetweenSentences = 0.2f;

        CheckForAllLetters();
        CheckForDuplicates();
        AddComversionElements();


    }

    private void AddComversionElements()
    {
        throw new NotImplementedException();
    }

    private void CheckForDuplicates()
    {
        throw new NotImplementedException();
    }

    private void CheckForAllLetters()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

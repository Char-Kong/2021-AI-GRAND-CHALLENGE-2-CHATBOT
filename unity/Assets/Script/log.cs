using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class log : MonoBehaviour
{
    // Start is called before the first frame update
    private Text logText = null;
    private ScrollRect Scroll_rect = null;
    void Start()
    {
        logText = GameObject.Find("Text").GetComponent<Text>();
        Scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Scroll_rect.verticalNormalizedPosition = 0.0f;
        }
    }
}
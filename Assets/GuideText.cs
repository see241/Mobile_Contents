using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideText : MonoBehaviour
{
    public static GuideText instance;

    private Text text;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Guide(string guide, float deleteDelay)
    {
        text.text = guide;
        Invoke("DeleteText", deleteDelay);
    }

    public void Guide(string guide)
    {
        text.text = guide;
    }

    public void DeleteText()
    {
        text.text = "";
    }
}
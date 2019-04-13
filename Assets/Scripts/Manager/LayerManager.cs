using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public static LayerManager instance;

    public Canvas[] canvas = new Canvas[5];
    public GameObject[] gameObjects = new GameObject[5];

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Layer(int v)
    {
        for (int i = 0; i < 5; i++)
        {
            canvas[i].enabled = false;
            gameObjects[i].SetActive(false);
        }
        canvas[v].enabled = true;
        gameObjects[v].SetActive(true);
    }
}
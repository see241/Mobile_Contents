using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItem : MonoBehaviour
{
    public GameObject item;
    public int cost;
    public GameObject go_Cost;

    // Use this for initialization
    private void Start()
    {
        ShopInit();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void ShopInit()
    {
    }

    public void ResetBase()
    {
        GetComponent<CardBase>().enabled = true;
        GetComponent<CardControl>().enabled = true;
        GetComponent<CardItem>().enabled = false;
        if (go_Cost != null)
            Destroy(go_Cost);
    }
}
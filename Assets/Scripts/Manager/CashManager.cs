using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    private Dictionary<string, TextAsset> dataTable = new Dictionary<string, TextAsset>();
    public static CashManager instance;

    private void Awake()
    {
        instance = this;
        GetAllActionTable();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void SetData(string k, TextAsset t)
    {
        dataTable.Add(k, t);
    }

    public TextAsset GetData(string k)
    {
        return dataTable[k];
    }

    private void GetAllActionTable()
    {
        object[] obj = Resources.LoadAll("ActionTable/");
        for (int i = 0; i < obj.Length; i++)
        {
            TextAsset textAsset = obj[i] as TextAsset;
            char[] ch = { '#', ',' };
            string[] k = textAsset.text.Split('\n')[0].Split(ch);
            Debug.Log(k[2]);
            SetData(k[2], textAsset);
        }
    }
}
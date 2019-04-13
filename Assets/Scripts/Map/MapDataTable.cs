using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

internal struct MAP
{
    public int value;
    public string key;
}

public class MapDataTable : MonoBehaviour
{
    public TextAsset table;
    private Queue<MAP> mapTable = new Queue<MAP>();

    private int[] index = new int[4];

    // Use this for initialization
    private void Start()
    {
        SetData();
        InitTable();

        SettingAllMap();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void InitTable()
    {
        MAP tmp;
        tmp.key = "Shop"; tmp.value = index[0];
        mapTable.Enqueue(tmp);
        tmp.key = "Dungeon"; tmp.value = index[1];
        mapTable.Enqueue(tmp);
        tmp.key = "Rest"; tmp.value = index[2];
        mapTable.Enqueue(tmp);
        tmp.key = "Mystery"; tmp.value = index[3];
        mapTable.Enqueue(tmp);
    }

    private void SetData()
    {
        List<string> data = new List<string>(table.text.Split('\n'));
        for (int i = 0; i < data.Count; i++)
        {
            ProcesseData(data[i]);
        }
    }

    private void ProcesseData(string k)
    {
        string[] datas = k.Split(',');
        switch (datas[0])
        {
            case "Shop": index[0] = int.Parse(datas[1]); break;
            case "Dungeon": index[1] = int.Parse(datas[1]); break;
            case "Rest": index[2] = int.Parse(datas[1]); break;
            case "Mystery": index[3] = int.Parse(datas[1]); break;
        }
    }

    [ContextMenu("Test")]
    private void SettingAllMap()
    {
        StartCoroutine(_SettingAllMap());
    }

    private void Division(ref Map map, ref int idx, ref Queue<MAP> _m)
    {
        if (idx < _m.Peek().value - 1)
        {
            map.mapKind = _m.Peek().key;
            idx += 1;
        }
        else if (_m.Peek().value != 0)
        {
            map.mapKind = _m.Dequeue().key;
            idx = 0;
        }
    }

    private IEnumerator SetRandomMap()
    {
        for (int i = 0; i < 5; i++)
        {
            Map[] maps = GameObject.Find("Index" + i).GetComponentsInChildren<Map>();
            Debug.Log(maps.Length);
            int ranLength = maps.Length;

            int[] ranArr = Enumerable.Range(0, ranLength).ToArray();

            Queue<MAP> tmpMap = new Queue<MAP>(mapTable);
            int index = 0;
            for (int j = 0; j < maps.Length; j++)
            {
                int ranIdx = Random.Range(j, ranLength);

                int tmp = ranArr[ranIdx];

                ranArr[ranIdx] = ranArr[j];

                ranArr[j] = tmp;

                Division(ref maps[ranArr[j]], ref index, ref tmpMap);
            }
        }

        yield return null;
    }

    private IEnumerator _SettingAllMap()
    {
        float t = Time.time;
        yield return StartCoroutine(MapManager.instance._CreateMap());
        Debug.Log("CreateEnd");
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(SetRandomMap());
    }
}
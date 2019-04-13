using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public GameObject map;

    private List<string> mapKinds;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    [ContextMenu("CreateMap")]
    private void CreateMap()
    {
        StartCoroutine(_CreateMap());
    }

    public void LinkRandomMap()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int rand = Random.Range(1, GameObject.Find("Index" + i).transform.GetChildCount() - 1);
                GameObject target = GameObject.Find("Index" + i).transform.GetChild(rand).gameObject;
                Map test = target.GetComponent<Map>();
                string targetName = "(" + (test.index + 1) + "," + (test.level + 1) + ")";
                target.GetComponent<Map>().LinkMap(GameObject.Find(targetName));
            }
        }
        for (int i = 1; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int rand = Random.Range(1, GameObject.Find("Index" + i).transform.GetChildCount() - 1);
                GameObject target = GameObject.Find("Index" + i).transform.GetChild(rand).gameObject;
                Map test = target.GetComponent<Map>();
                string targetName = "(" + (test.index - 1) + "," + (test.level + 1) + ")";
                target.GetComponent<Map>().LinkMap(GameObject.Find(targetName));
            }
        }
    }

    public IEnumerator _CreateMap()
    {
        for (int i = 0; i < 5; i++)
        {
            map.GetComponent<Map>().index = i;
            map.transform.position = new Vector2(Random.Range(-0.5f, 0.5f) + (map.GetComponent<Map>().index - 2) * 2f, Random.Range(-0.5f, 1.2f) + 60);
            Instantiate(map, GameObject.Find("Index" + i).transform);

            yield return new WaitForFixedUpdate();
        }
    }
}
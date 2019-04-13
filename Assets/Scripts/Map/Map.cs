using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Sprite[] images = new Sprite[4];

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject line;

    public List<GameObject> linkedMaps = new List<GameObject>();

    public int index;

    public int level;

    [SerializeField]
    private string mk;

    public string mapKind
    {
        get { return mk; }
        set
        {
            mk = value;
            switch (mk)
            {
                case "Shop":
                    mapState = GameState.Shop;
                    break;

                case "Dungeon":
                    mapState = GameState.Dungeon;
                    break;

                case "Rest":
                    mapState = GameState.Rest;
                    break;

                case "Mystery":
                    mapState = GameState.Mystery;
                    break;
            }
            IconChange();
        }
    }

    private GameState mapState;

    private void Awake()
    {
    }

    // Use this for initialization
    private void Start()
    {
        gameObject.name = "(" + index + "," + level + ")";

        if (level < 10)
        {
            CreateNextMap();
        }
        if (level == 10 && index == 4)
        {
            MapManager.instance.LinkRandomMap();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (CheckAble())
            GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
        if (!CheckAble())
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    public void IconChange()
    {
        GetComponent<SpriteRenderer>().sprite = images[(int)mapState];
    }

    private bool CheckAble()
    {
        if (level <= Character.instance.curMapLevel)
            return true;
        else
            return false;
    }

    public void StartCreate(int v)
    {
        index = v;
        //Instantiate(gameObject, GameObject.Find("Map").transform);
        //Debug.Log(index);
        //gameObject.transform.position = new Vector2(Random.Range(-0.5f, 1.5f) + (index - 2) * 1.5f, 0);
        //this.gameObject.name = "(" + index + "," + level + ")";
    }

    [ContextMenu("CreateMap")]
    private void CreateNextMap()
    {
        GameObject nextMap = Instantiate(map, GameObject.Find("Index" + index).transform);
        SettingNextMap(ref nextMap);
        LinkMap(nextMap);
    }

    private void SettingNextMap(ref GameObject go)
    {
        go.GetComponent<Map>().index = index;
        go.GetComponent<Map>().level = level + 1;
        go.gameObject.name = "(" + go.GetComponent<Map>().index + "," + go.GetComponent<Map>().level + ")";
        go.transform.position = new Vector2(Random.Range(-0.5f, 0.5f) + (go.GetComponent<Map>().index - 2) * 2f, Random.Range(0.5f, 1.2f) + (go.GetComponent<Map>().level * 2) + 60);
    }

    public void LinkMap(GameObject go)
    {
        GameObject tmp = Instantiate(line, GameObject.Find("Line").transform);
        LineRenderer _line = tmp.GetComponent<LineRenderer>();
        _line.SetPosition(0, this.transform.position);
        _line.SetPosition(1, go.transform.position);
        linkedMaps.Add(go);
    }

    public IEnumerator SelectMap()
    {
        yield return Character.instance.StartCoroutine(Character.instance.MoveTo(gameObject));
        yield return new WaitForSeconds(0.05f);
        Debug.Log("Move End");
        yield return CameraControll.instance.StartCoroutine(CameraControll.instance.CameraFadeIn());
        yield return new WaitForSeconds(0.05f);
        Debug.Log("FadeIn End");
        GameManager.instance.eState = mapState;
        yield return CameraControll.instance.StartCoroutine(CameraControll.instance.CameraFadeOut());
        yield return new WaitForSeconds(0.05f);
        Debug.Log("FadeOut End");
    }
}
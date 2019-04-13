using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Shop = 0, Dungeon, Rest, Mystery, Map
}

public class GameManager : MonoBehaviour
{
    public Text tCurGold;
    public Text shopCurGold;

    private GameState state;

    public GameState eState
    {
        get { return state; }
        set
        {
            state = value;
            //StopAllCoroutines();

            StartCoroutine(ChangeScene());
        }
    }

    public GameObject textMs;
    public int gGold;
    public static GameManager instance;

    private int cg;
    public int curGold { get { return cg; } set { cg = value; tCurGold.text = "You Have " + cg + " Gold"; shopCurGold.text = "You Have " + cg + " Gold"; } }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        eState = GameState.Map;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Escape()
    {
    }

    [ContextMenu("Gamestate")]
    private void test()
    { Debug.Log(eState); }

    private void DungeonStart()
    {
        Debug.Log("Dungeon Start");
        MsgQueue.instance.StartCoroutine(MsgQueue.instance.MessageQueu());
        DeckManager.instance.InitDeck();
        Player.instance.InitPlayer();
        DeckManager.instance.DrawCard(5);
        Player.instance.StartPlayerTurn();
        EnemyController.instance.InitEnemy();
    }

    public void DungeonClear()
    {
        eState = GameState.Map;
        curGold += gGold;
    }

    public void BackToMain()
    {
        StartCoroutine(CameraControll.instance.CameraFadeIn());
        eState = GameState.Map;
        StartCoroutine(CameraControll.instance.CameraFadeOut());
    }

    public void Heal()
    {
        Player.instance.curHp += 10;
        if (Player.instance.curHp > Player.instance.maxHp)
            Player.instance.curHp = Player.instance.maxHp;
        eState = GameState.Map;
    }

    public void RandHeal()
    {
        int r = Random.Range(-5, 11);
        Player.instance.curHp += r;

        if (Player.instance.curHp > Player.instance.maxHp)
            Player.instance.curHp = Player.instance.maxHp;

        if (Player.instance.curHp <= 0)
            Player.instance.curHp = 1;

        GameObject _text = Instantiate(textMs, Camera.main.transform);
        _text.transform.position = (Vector2)Camera.main.transform.position - new Vector2(0, 3);
        string result = null;
        if (r <= 0) result = " Hp Less"; else result = " Hp Heal";
        _text.GetComponent<TextMesh>().text = r + result;
        eState = GameState.Map;
        Destroy(_text, 3);
    }

    public void PopUpMsg(string s)
    {
        GameObject _text = Instantiate(textMs, Camera.main.transform);
        _text.transform.position = (Vector2)Camera.main.transform.position - new Vector2(0, 3);
        _text.GetComponent<TextMesh>().text = s;

        Destroy(_text, 0.75f);
    }

    private IEnumerator ChangeScene()
    {
        yield return StartCoroutine(CameraControll.instance.CameraFadeIn());
        LayerManager.instance.Layer((int)eState);
        CameraControll.instance.SetLayer((int)eState);
        if (state == GameState.Dungeon)
        {
            DungeonStart();
        }
        if (state == GameState.Map)
        {
            if (Character.instance.curMapLevel >= 0)
                Character.instance.SecletMap();
        }
        else
        {
            Character.instance.SecletDisable();
        }
        yield return StartCoroutine(CameraControll.instance.CameraFadeOut());
    }
}
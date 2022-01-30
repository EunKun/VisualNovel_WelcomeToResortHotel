using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class TextManager : MonoBehaviour
{
    #region TextControlPrint
    [SerializeField] string[] totalScripts;
    //텍스트 명 설정
    [System.NonSerialized] public string scriptFileName = "1";
    public static string playerName;
    public static readonly string playerColorName = "<color=yellow>";
    #endregion

    #region OutPutText
    public static int scriptNum = 1;
    public static bool isScriptPeriod;
    public static bool isNovelScriptMode;
    public static string novelScript;
    public static bool isCenterScriptMode;
    public static string centerScript;
    public static int novelLineNum = 1;
    public readonly static int novelLineMax = 13;
    public static int stopCount;

    [SerializeField]
    public static string[] outputScripts;
    [Header("스크립트 관련")]
    public GameObject[] charaNameObjs;
    public Text[] charaNameText;
    public Text normalMainScriptText;
    public Text novelMainScriptText;
    public static GameObject centerTextObj;
    public Text centerMainScriptText;
    public static GameObject fadeOut;
    public static GameObject selectBtnMainObj;
    public static Button[] selectBtns = new Button[3];
    public static GameObject centerImageObj; // 중앙 이미지 출력 패널
    public static Image centerImage; // 중앙 이미지
    
    [Header("로그 관련")]
    public static GameObject mainLogObj;
    public static Transform normalLogParentPos;
    public static GameObject logPrefab;
    public static string[] novelLog = new string[5];
    #endregion

    #region CharPos
    [Header("캐릭터 관련")]
    public SpriteAtlas atlas;
    public static Transform[] charaPos;
    public GameObject[] charaObjs;
    private string[] charaName = new string[3];
    #endregion

    #region test
    public Text testViewScript;
    #endregion

    void Start()
    {
        mainLogObj = GameObject.Find("LogPanel");

        normalLogParentPos = GameObject.Find("NormalLogContent").GetComponent<Transform>();
        logPrefab = Resources.Load("Prefabs/LogScriptPanel") as GameObject;

        charaPos = GameObject.Find("CharaPosition").GetComponentsInChildren<Transform>();
        fadeOut = GameObject.Find("FadeOut");
        selectBtnMainObj = GameObject.Find("SelectBtn");

        centerImageObj = GameObject.Find("CenterImagePanel");
        centerImage = centerImageObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        centerImageObj.SetActive(false);

        selectBtns = new Button[3];
        for (int i = 0; i < selectBtnMainObj.transform.childCount; i++)
        {
            selectBtns[i] = selectBtnMainObj.transform.GetChild(i).GetChild(0).GetComponent<Button>();
            selectBtns[i].name = "SelectBtn_" + i;
        }

        centerTextObj = centerMainScriptText.transform.parent.gameObject;

        FirstStart();
    }

    public void FirstStart()
    {
        scriptNum = 1;
        centerMainScriptText.text = "";

        mainLogObj.SetActive(false);
        centerTextObj.GetComponent<CanvasGroup>().alpha = 0;

        LoadNewText(scriptFileName);

        if (isNovelScriptMode)
            LoadScript.NovelScriptMode("v_mode");
        else
            LoadScript.NovelScriptMode("n_mode");
    }

    void Update()
    {
        if (GameManager.ins.state == GameManager.State.Play)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
                NextScript();
        }

        if (isNovelScriptMode)
            LoadScript.PeriodIcon(novelMainScriptText.transform.GetChild(0).gameObject);
        else
            LoadScript.PeriodIcon(normalMainScriptText.transform.GetChild(0).gameObject);

        if (Input.GetKeyDown(KeyCode.L))
            LogView.OpenLog();

        if (Input.GetKeyDown(KeyCode.C))
            LogView.LogClear();
    }

    public void NextScript()
    {
        if (scriptNum < totalScripts.Length)
        {
            outputScripts = LoadScript.MakeScript(totalScripts[scriptNum]);

            StartCoroutine(PlayScript(outputScripts[0], outputScripts[1], outputScripts[3], 
                outputScripts[4], outputScripts[5], outputScripts[6], outputScripts[7], outputScripts[8]));
            scriptNum++;
        }
        else
        {
            scriptNum = 1;
            testViewScript.text = "";
            novelMainScriptText.text = "";

            Debug.Log("최대길이 or 다음 페이지로 넘김 기능 추가");
        }
    }

    void ModCheck(string _mod)
    {
        if (_mod != "")
        {
            if (_mod.Contains("_mode"))
                LoadScript.NovelScriptMode(_mod);

            if (_mod.Contains("centerImage"))
                CenterImagePanel(_mod);

            if (_mod.Contains("fadein") || _mod.Contains("fadeout"))
                StartCoroutine(Fade(_mod));

            if (_mod.Contains("jump"))
            {
                string[] _temp = _mod.Split('_');
                LoadNewText(_temp[1]);
            }
        }
    }

    IEnumerator PlayScript(string _mode, string _charaName, string _charaMoving, string _script, 
        string _bgm, string _se, string _selectBtn, string _keypoint)
    {
        //테스트용 스크립트 출력용
        testViewScript.text = totalScripts[scriptNum];

        GameManager.ins.SaveData();

        ModCheck(_mode);

        if (_script == "")
        {
            if (_mode.Contains("select"))
            {
                SelectButton.Open(_mode, _selectBtn, _keypoint);
            }
            else
            {
                scriptNum++;
                NextScript();
            }
        }
        else
        {
            PlayerNameCheck(ref _charaName, ref _script);

            yield return new WaitUntil(() => GameManager.ins.state == GameManager.State.Play);

            if (_mode.Contains("select"))
                SelectButton.Open(_mode, _selectBtn, _keypoint);

            TextInit();
            StopAllCoroutines();

            if (isCenterScriptMode)
                StartCoroutine(LoadScript.OutputText(centerMainScriptText, _charaName, _script));
            else
            {
                if (isNovelScriptMode)
                    StartCoroutine(LoadScript.OutputText(novelMainScriptText, _charaName, _script));
                else
                    StartCoroutine(LoadScript.OutputText(normalMainScriptText, _charaName, _script));
            }

            LoadScript.CharaMove(_charaMoving, charaObjs);

            //나중에 풀어야 함
            PlayMusic.Play(_bgm, _se); 
        }
    }

    void JumpNextScript()
    {

    }

    IEnumerator Fade(string _mode)
    {
        string[] _modeCheck = new string[2];

        if(_mode.Contains("_"))
        {
            _modeCheck = _mode.Split('_');
            if (_modeCheck[0].Trim() == "fadein" || _modeCheck[0].Trim() == "fadeout")
            {
                GameManager.ins.state = GameManager.State.Fading;
                Debug.Log("페이드 시간 : " + _modeCheck[1]);

                int _fadeTimeLength = int.Parse(_modeCheck[1]);
                float _time = _fadeTimeLength * 0.1f;

                LeanTween.alphaCanvas(fadeOut.GetComponent<CanvasGroup>(), 1, _time);
                yield return new WaitForSeconds(_time);
                LeanTween.alphaCanvas(fadeOut.GetComponent<CanvasGroup>(), 0, _time);
                yield return new WaitForSeconds(_time);

                GameManager.ins.state = GameManager.State.Play;
            }
            else
                Debug.Log("스크립트 명령어 틀린듯?");
        }
    }

    void PlayerNameCheck(ref string _playerName, ref string _script)
    {
        if (_playerName.Contains("주인공"))
            _playerName = _playerName.Replace("주인공", playerName);

        if (_script.Contains("(주인공)"))
            _script = _script.Replace("(주인공)", playerName);
    }

    void CenterImagePanel(string _mode)
    {
        if (_mode.Contains("centerImage_close"))
            centerImageObj.SetActive(false);
        else if (_mode.Contains("centerImage"))
        {
            string[] imageName = _mode.Split('_');
            centerImageObj.SetActive(true);
            
            //centerImage.sprite = atlas.GetSprite(imageName[1]);
        }
    }

    public void LoadNewText(string _scriptFileName)
    {
        totalScripts = LoadScript.Load(_scriptFileName);
        outputScripts = totalScripts[scriptNum].Split(',');
        novelMainScriptText.text = "";
    }

    public void TextInit()
    {
        if(isNovelScriptMode)
        {
            normalMainScriptText.transform.parent.gameObject.SetActive(false);
            novelMainScriptText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            normalMainScriptText.transform.parent.gameObject.SetActive(true);
            novelMainScriptText.transform.parent.gameObject.SetActive(false);

            foreach (Text _temp in charaNameText)
                _temp.text = "";

            for (int i = 0; i < charaNameObjs.Length; i++)
            {
                charaNameObjs[i].SetActive(false);
                charaNameText[i].text = "";
            }

            normalMainScriptText.text = "";
        }
    }
}

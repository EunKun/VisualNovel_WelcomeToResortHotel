using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScript{

    public static string[] Load(string _fileName)
    {
        TextAsset asset = Resources.Load("MainText/" + _fileName) as TextAsset;
        string[] _temp = asset.ToString().Split('\n');

        PlayerPrefs.SetString(GameManager.ins.saveString[0], _fileName);

        return _temp;
    }

    public static string[] MakeScript(string _script)
    {
        string[] output = _script.Split(',');
        return output;
    }

    public static void CharaMove(string _charaMoving, GameObject[] _moveCharaObjs)
    {
        if (_charaMoving != "")
        {
            string[] charaName = _charaMoving.Split('|');

            for (int i = 0; i < charaName.Length; i++)
            {
                if (charaName[i] == "")
                    _moveCharaObjs[i].SetActive(false);
                else
                    _moveCharaObjs[1].SetActive(true);
            }
            MoveObj.Move(_moveCharaObjs, _charaMoving);
        }
    }

    public static void NovelScriptMode(string _mode)
    {
        switch (_mode)
        {
            case "v_mode":
                LogPanelControl(true);
                break;
            case "n_mode":
                LogPanelControl(false);
                break;
            case "c_mode":
                CenterText(true);
                break;
            case "c_mode_close":
                CenterText(false);
                break;
        }
    }

    static void CenterText(bool _mode)
    {
        TextManager.centerTextObj.transform.GetChild(0).GetComponent<Text>().text = "";

        if (_mode)
            LeanTween.alphaCanvas(TextManager.centerTextObj.GetComponent<CanvasGroup>(), 1, 0.1f);
        else
        {
            LeanTween.alphaCanvas(TextManager.centerTextObj.GetComponent<CanvasGroup>(), 0, 0.1f);
            TextManager.centerScript = "";
        }

        TextManager.isCenterScriptMode = _mode;
    }

    static void LogPanelControl(bool vMode)
    {
        if(vMode)
            TextManager.isNovelScriptMode = true;
        else
            TextManager.isNovelScriptMode = false;
    }

    public static IEnumerator OutputText(Text _mainText, string _charaName, string _script)
    {
        TextManager.isScriptPeriod = false;

        if(!TextManager.isNovelScriptMode) 
            _mainText.text = "";

        if(_charaName != "")
        {
            _mainText.text = TextManager.playerColorName + _charaName + "</color>「";
        }

        string[] temp = _script.Split('|'); //스토퍼 기능
        Debug.Log("temp 길이 : " + temp.Length);

        if (TextManager.isCenterScriptMode)
            TextManager.centerScript += _script + "\n";
        else
            TextManager.novelScript += _script + "\n";

        for (int i = 0; i < temp.Length; i++)
        {
            char[] letter = temp[i].ToCharArray();
            bool isFuntion = false;

            switch(temp[i].Trim())
            {
                case "s":
                    isFuntion = true;
                    GameManager.ins.state = GameManager.State.Stop;
                    TextManager.isScriptPeriod = true;
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                    TextManager.isScriptPeriod = false;
                    GameManager.ins.state = GameManager.State.Play;
                    break;
                default:

                    break;
            }

            if (!isFuntion)
            {
                for (int j = 0; j < letter.Length; j++)
                {
                    if (letter[j] == '@')
                        _mainText.text += ',';
                    else
                    {
                        if (_charaName != "")
                        {
                            if (temp.Length == 1 && j == letter.Length)
                                _mainText.text += letter[j] + "」";
                            else if (temp.Length - 1 == i && j == letter.Length - 1)
                                _mainText.text += letter[j] + "」";
                            else
                                _mainText.text += letter[j];
                        }
                        else
                            _mainText.text += letter[j];
                    }

                    yield return null;
                }
            }
            else
                isFuntion = true;
        }

        ReplaceText(_mainText.text);
    }

    static void ReplaceText(string _script)
    {
        if (TextManager.isNovelScriptMode)
        {
            if (TextManager.novelScript != null)
                TextManager.novelLineNum++;
        }

        LogView.MakeNormalModeLog(TextManager.normalLogParentPos, _script);

        TextManager.isScriptPeriod = true;
        TextManager.stopCount = 0;
    }

    static string AddCharaName(string _charName, string script)
    {
        string _temp = "";

        if(_charName == "")
            _temp = _charName + script;
        else
            _temp = _charName + "「" + script + "」";


        return _temp;
    }

    public static void PeriodIcon(GameObject _periodObj)
    {
        if (TextManager.isScriptPeriod)
        {
            _periodObj.SetActive(true);
            LeanTween.alphaCanvas(_periodObj.GetComponent<CanvasGroup>(), Mathf.PingPong(Time.time, 1.5f), 0.8f);
        }
        else
            _periodObj.gameObject.SetActive(false);
    }
}
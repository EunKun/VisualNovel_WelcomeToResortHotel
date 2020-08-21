using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    public static void Open(string _mode, string _script, string _keyPoint)
    {
        if (_mode.Contains("select"))
        {
            GameManager.ins.state = GameManager.State.Select;

            string[] _tempSelect = new string[3];
            _tempSelect = _mode.Split('_');

            string[] _selectScript = new string[3];
            float _tempCount = int.Parse(_tempSelect[1]);

            if (_script.Contains("|"))
                _selectScript = _script.Split('|');

            int[] _pointValue = new int[3];
            if(_keyPoint.Contains("|"))
            {
                string[] _temp = _keyPoint.Split('|');
                for (int i = 0; i < _temp.Length; i++)
                {
                    try
                    {
                        _pointValue[i] = int.Parse(_temp[i]);
                    }
                    catch
                    {
                        _pointValue[i] = 0;
                    }
                }
            }

            if(_tempCount > 1)
            {
                foreach (Button btn in TextManager.selectBtns)
                    btn.onClick.RemoveAllListeners();

                for (int i = 0; i < _tempCount; i++)
                {
                    int _temp = i;
                    TextManager.selectBtns[i].transform.parent.gameObject.SetActive(true);
                    TextManager.selectBtns[i].transform.GetChild(0).GetComponent<Text>().text = _selectScript[i];
                    //명령어 넣어야 함. 예시로 입력 값 넣음
                    if (_tempSelect[2].Trim() == "v")
                        TextManager.selectBtns[i].onClick.AddListener(() => Btn_AddKeyPointer(_pointValue[_temp]));
                    else if(_tempSelect[2].Trim() == "s")
                        TextManager.selectBtns[i].onClick.AddListener(() => Btn_ScriptLineChange(_pointValue[_temp].ToString()));
                }
            }
        }
        else
        {
            for (int i = 0; i < TextManager.selectBtns.Length; i++)
                TextManager.selectBtns[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public static void Btn_AddKeyPointer(int _keyPoint)
    {
        GameManager.ins.keyPoint += _keyPoint;
        Debug.Log("투르엔딩 포인트 : " + GameManager.ins.keyPoint);
        PlayerPrefs.SetInt(GameManager.ins.saveString[2], GameManager.ins.keyPoint);

        CloseBtn(true, _keyPoint, null);
    }

    public static void Btn_ScriptLineChange(string _line)
    {
        CloseBtn(false, 0, _line);
    }

    static void CloseBtn(bool isPoint, int _point, string _scriptLine)
    {
        for (int i = 0; i < TextManager.selectBtns.Length; i++)
            TextManager.selectBtns[i].transform.parent.gameObject.SetActive(false);

        GameManager.ins.state = GameManager.State.Play;

        int _lineNum = 0;

        if(isPoint)
            GameManager.ins.tm.NextScript();
        else
        {
            if (int.TryParse(_scriptLine, out _lineNum))
            {
                TextManager.scriptNum = _lineNum;
                GameManager.ins.tm.NextScript();
            }
            else
                Debug.Log("라인점프 뻑남");
        }
    }
}

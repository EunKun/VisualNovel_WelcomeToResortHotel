﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State { Title, Play, Select, Log, Fading, Stop}
    public State state;

    public static GameManager ins;
    public TextManager tm;
    public UIManager ui;
    public bool isNewGame;
    public bool isAllClear = false;

    public int keyPoint = 1;
    [System.NonSerialized] public readonly int maxKeyPoint = 100;
    [System.NonSerialized] public string[] saveString = { "ScriptfileName", "ScriptlineNum", "EndingPoint", "Ending"};

    private void Awake()
    {
        if (ins == null)
            ins = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Title;
        ui.startBtn.onClick.AddListener(() => OpenPlayerNameInput());
        ui.decisionPlayerNameBtn.onClick.AddListener(() => Btn_NewStart());
        ui.loadBtn.onClick.AddListener(() => Btn_LoadData());

        FirstStart();
    }

    public void FirstStart()
    {
        if (PlayerPrefs.GetInt(saveString[1], 0) != 0 && PlayerPrefs.GetInt(saveString[1], 0) != 1)
            ui.loadBtn.interactable = true;
        else
            ui.loadBtn.interactable = false;

        if (PlayerPrefs.GetInt(saveString[3], 0) != 0)
            ui.clearBonusBtn.transform.parent.gameObject.SetActive(true);
        else
            ui.clearBonusBtn.transform.parent.gameObject.SetActive(false);
    }

    public void OpenPlayerNameInput()
    {
        AlertPanelManager.playerNameInputFieldObj.gameObject.SetActive(true);
        state = State.Select;
    }


    public void Btn_NewStart()
    {
        if (state == State.Select)
        {
            PlayerPrefs.DeleteAll();

            if (AlertPanelManager.playerNameField.text != "")
                TextManager.playerName = AlertPanelManager.playerNameField.text;
            else
                TextManager.playerName = "서진";

            PlayerPrefs.SetString("playerName", TextManager.playerName);
        }
        else
            Debug.Log("로드이거나 시작부분이 뭔가 꼬였음 확인 필요");

        state = State.Play;

        AlertPanelManager.playerNameInputFieldObj.gameObject.SetActive(false);
        ui.mainTitleObj.SetActive(false);
        tm.NextScript();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(saveString[1], TextManager.scriptNum);
        PlayerPrefs.SetInt(saveString[2], keyPoint);
    }

    public void Btn_LoadData()
    {
        Script.Load(PlayerPrefs.GetString(saveString[0]));
        TextManager.scriptNum = PlayerPrefs.GetInt(saveString[1], 1);
        keyPoint = PlayerPrefs.GetInt(saveString[2], 1);

        Btn_NewStart();
    }
}
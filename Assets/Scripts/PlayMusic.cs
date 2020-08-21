using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayMusic
{
    public static void Play(string _bgmNum, string _seNum)
    {
        if(_bgmNum != "")
        {
            SoundManager.ins.soundController.bgm.Stop();
            if(_bgmNum != "stop")
            {
                SoundManager.ins.soundController.bgm.clip = SoundManager.ins.soundController.bgmSource[int.Parse(_bgmNum.Trim())];
                SoundManager.ins.soundController.bgm.Play();
            }
        }

        if(_seNum != "")
        {
            SoundManager.ins.soundController.se.Stop();
            if(_seNum != "stop")
            {
                SoundManager.ins.soundController.se.clip = SoundManager.ins.soundController.seSource[int.Parse(_seNum.Trim())];
                SoundManager.ins.soundController.se.Play();
            }
        }
    }
}

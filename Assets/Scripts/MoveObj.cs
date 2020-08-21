using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveObj
{
    public static void Move(GameObject[] objs, string _movePos)
    {
        string[] _pos = _movePos.Split('|');

        if(_pos.Length > 1)
        {
            int num = -1;

            for (int i = 0; i < objs.Length; i++)
            {
                if (_pos[i].Contains("&"))
                {
                    string[] tempPos = _pos[i].Split('&');
                    
                    objs[i].transform.position = TextManager.charaPos[int.Parse(tempPos[0])].position;
                    LeanTween.move(objs[i], TextManager.charaPos[int.Parse(tempPos[1])], 1f);
                }
                else
                {
                    for (int j = 0; j < objs.Length; j++)
                    {
                        if (int.TryParse(_pos[j], out num))
                            objs[j].transform.position = TextManager.charaPos[int.Parse(_pos[j])].position;
                    }
                }
            }
        }
    }
}

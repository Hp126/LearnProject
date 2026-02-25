using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingletonBase<InputManager>
{

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            //Debug.Log("Input--- 触发成就达成");
            EventCenter.Broadcast("DoAchievement");

            EventCenter.Broadcast("OnCancelLogin");
        }

    }

}

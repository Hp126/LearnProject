using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Learn1_1 : MonoBehaviour
{
    private void OnEnable()
    {
        EventCenter.AddListener("DoAchievement", DoAchievement,0);

    }

    private void OnDisable()
    {
        EventCenter.RemoveListener("DoAchievement", DoAchievement);

    }


    private void DoAchievement()
    {
        Debug.Log("Learn_1_1--- 执行成就系统逻辑");
    }
}

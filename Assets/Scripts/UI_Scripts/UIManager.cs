using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject go_SkillCheckBase; // 스킬체크 UI ON/OFF



    //시간을 멈추지 않기위해(채광 UI가 띄워져 있는동안 계속 난이도가 올라가도록)
    //상태 변수를 이용해 시간을 제외한 나머지만 멈춰줌
    public static bool isMinning = false;




    private void Update()
    {
        if (isMinning)
            OnMinningUI();
        else
            OffMinningUI();
    }



    //채광 스킬체크
    public void OnMinningUI()
    {
        go_SkillCheckBase.SetActive(true); // 스킬체크 UI 활성화
    }

    public void OffMinningUI()
    {
        isMinning = false;

        go_SkillCheckBase.SetActive(false); // 스킬체크 UI 비활성화
    }

}

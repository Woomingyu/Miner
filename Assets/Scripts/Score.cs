using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Score : MonoBehaviour
{
    public bool isHighScore; // 스코어 구분용

    float highScore;
    TextMeshProUGUI uiText;


    //최고기록이 아닌 일반 스코어와 스코어 저장은 GameManager에서 관리함
    void Start()
    {
        //PlayerPrefs.DeleteAll(); 저장 데이터 삭제
        uiText = GetComponent<TextMeshProUGUI>(); //스크립트의 tmp 획득

        //최고기록 == 기록 꺼내오기 & 기록 표기
        if (isHighScore)
        {
            highScore = PlayerPrefs.GetFloat("Score");
            uiText.text = highScore.ToString("F0"); //포멧 F0 => 소수점 생략 F1 소수점 한자리....
        }

    }


    void LateUpdate()
    {
        if (!GameManager.isLive) //isLive가 true인 경우에만 아래 실행
            return;

        if (isHighScore && GameManager.score < highScore) //현재 스코어가 하이스코어에 못미치는경우
            return;

        //현재 스코어가 하이스코어를 넘음
        uiText.text = GameManager.score.ToString("F0") + "G";
    }
}

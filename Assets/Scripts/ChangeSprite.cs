using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    //생성 스프라이트 랜덤 변경 (배경용)
    public Sprite[] sprites;
    SpriteRenderer spriter;
    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        Change();
    }

   
    public void Change()
    {
        //배열 길이에 따라 랜덤값 변수 생성 
        int ran = Random.Range(0, sprites.Length);
        //스프라이트를 배열에서 랜덤으로 뽑아 변경
        spriter.sprite = sprites[ran];
    }
}

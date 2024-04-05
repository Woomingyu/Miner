using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObject : MonoBehaviour
{
    public GameObject[] objs;


    public void Change()
    {
        int ran = Random.Range(0, objs.Length);

        for (int index = 0; index < objs.Length; index++)
        {
            transform.GetChild(index).gameObject.SetActive(ran == index); // 랜덤값과 index가 동일한경우 true
        }
    }
}

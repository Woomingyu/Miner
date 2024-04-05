using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DugeonObjectController : MonoBehaviour
{
    [SerializeField]
    private GameObject Well;



    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void UsedWell()
    {
        anim.SetBool("UseWell", true);

        Invoke("ResetWell", 2f);
    }

    private void ResetWell()
    {
        anim.SetBool("UseWell", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAxeAnimation : MonoBehaviour
{
    Animator axe;
    void Start()
    {
        axe = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void move()
    {
        axe.SetBool("move", true);
    }

    public void notmove()
    {
        axe.SetBool("move", false);
    }
    public void attack()
    {
        axe.SetBool("attack", true);
    }
    public void notattack()
    {
        axe.SetBool("attack", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootanimation : MonoBehaviour
{
    Animator arrowanimator;
    // Start is called before the first frame update
    void Start()
    {
         arrowanimator = GetComponent<Animator>();
    
    }

    // Update is called once per frame
    public void drawback()
    {
        arrowanimator.SetBool("drawback", true);
        arrowanimator.SetBool("shoot", false);

    }

    public void shoot()
    {
        arrowanimator.SetBool("shoot", true);
        arrowanimator.SetBool("failed", false);
        arrowanimator.SetBool("drawback", false);
    }

    public void failed()
    {
        arrowanimator.SetBool("failed", true);
        arrowanimator.SetBool("shoot", false);
        arrowanimator.SetBool("drawback", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinganimation : MonoBehaviour
{
    Animator swordanimator;
    // Start is called before the first frame update
    void Start()
    {
        swordanimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void swinganimation()
    {
        swordanimator.SetTrigger("Swing");
    }
    
}

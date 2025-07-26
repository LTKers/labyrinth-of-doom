using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeyou : MonoBehaviour
{
    [SerializeField] GameObject seeyou;
    float timer = 0;
    float timers = 0;
    bool on = false;
    // Start is called before the first frame update
    void Start()
    {
        timers = 0;
        timer = 0;
        seeyou.SetActive(false);
    
    }

    public void Show()
    {
        seeyou.SetActive(true);
        Debug.Log("show");
    }
    public void Hide()
    {
        seeyou.SetActive(false);
    }
   
}


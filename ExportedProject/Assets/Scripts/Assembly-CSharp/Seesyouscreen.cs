using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesyouscreen : MonoBehaviour
{
    [SerializeField] GameObject seeyou;
    float timer=0;
    float timers=0;
    bool on = false;
    void Start()
    {
        seeyou.SetActive(false);
    }
    void Update()
    {
        Debug.Log(timers);
         if (on == false)
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                seeyou.SetActive(true);
                on = true;
                Debug.Log("true");
                timer=0;
               

            }
        }
        else
        {
            timers += Time.deltaTime;
            if (timers >= 20)
            {
                seeyou.SetActive(false);
                on = false;
                Debug.Log("false");
               timers=0;

            }
        }
        
        
    }
   
}

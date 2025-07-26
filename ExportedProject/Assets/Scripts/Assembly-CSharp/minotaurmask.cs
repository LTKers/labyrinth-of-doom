using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minotaurmask : MonoBehaviour
{
    [SerializeField] Camera cam;
    float timer;
    float timers;
    bool on = false;

    public GameObject player;
  

    // Start is called before the first frame update

    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
       
        Seeyou sees = player.GetComponent<Seeyou>();
        if (on == false)
        {
            timer += Time.deltaTime;
            if (timer >= 20)
            {

                
                cam.cullingMask = ~(1 << LayerMask.NameToLayer("agent"));
                timer = 0;
                on = true;
                sees.Show();
                Debug.Log("bing");
                

            }
        }
        if (on==true)
        {
            timers += Time.deltaTime;
            if (timers >= 5)
            {
                cam.cullingMask = ~(1 << LayerMask.NameToLayer("minimap")) & ~(1 << LayerMask.NameToLayer("agent"));
                timers = 0;
                on = false;
                sees.Hide();

            }
        }
        
    }
}

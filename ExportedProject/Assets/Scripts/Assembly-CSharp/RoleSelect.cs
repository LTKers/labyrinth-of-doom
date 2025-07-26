using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelect : MonoBehaviour
{
    [SerializeField] GameObject role;
    float timer=0;
    // Start is called before the first frame update
    void Start()
    {
        role.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>=3)
        {
            role.SetActive(false);   
        }
    }
}

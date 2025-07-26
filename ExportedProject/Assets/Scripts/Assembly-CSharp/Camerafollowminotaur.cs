using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollowminotaur : MonoBehaviour
{
    [SerializeField] private Minotaurmapsetting settings;
    [SerializeField] private float cameraHeight;


    private void Awake()
    {
        settings.GetComponentInParent<Minotaurmapsetting>();
        cameraHeight = transform.position.y;
    }

    void Update()
    {

        Vector3 targetPosition=settings.targetToFollow.transform.position;
      
       
        if(settings.rotateWithTheTarget){
            Quaternion targetRotation=settings.targetToFollow.transform.rotation;
            transform.rotation=Quaternion.Euler(90,targetRotation.eulerAngles.y,0);
        }
    }
}

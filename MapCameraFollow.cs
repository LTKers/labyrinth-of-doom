using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class MapCameraFollow : MonoBehaviour
{
    [SerializeField] private MiniMapSetting settings;
    [SerializeField] private float cameraHeight;
    PhotonView PV;
    
    private void Awake()
    {

        if (PV.IsMine)
        {
            settings.GetComponentInParent<MiniMapSetting>();
            cameraHeight = transform.position.y;
        }
    }

    void Update()
    {
        if (PV.IsMine)
        {
            Vector3 targetPosition = settings.targetToFollow.transform.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y + cameraHeight, targetPosition.z);

            if (settings.rotateWithTheTarget)
            {
                Quaternion targetRotation = settings.targetToFollow.transform.rotation;
                transform.rotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, 0);
            }
        }
    }
}

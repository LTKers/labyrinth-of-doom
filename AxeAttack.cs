using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AxeAttack : MonoBehaviour
{
    [SerializeField] Camera cam;

    PhotonView PV;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    
    public void slash()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        FindObjectOfType<MinotaurAudioManager>().Play("Swordslash");
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            Debug.Log("damage");
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(50);
          

        }

    }

}

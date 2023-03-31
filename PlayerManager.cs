using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;
    Vector3 masterposition;
    Vector3 masterrotation;
    bool dead = false;
    bool added = false;
    bool onetime = true;
    bool bing = false;
    bool bong = true;

    PlayerController control;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
        control = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    void CreateController()
    {
        masterposition = new Vector3(0.0f, 1.05f, 126.12f);
        masterrotation = new Vector3(0.0f, 1.0f, 2.0f);
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoints();
        if (PhotonNetwork.IsMasterClient)
        {
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Minotaur"), masterposition, Quaternion.identity, 0, new object[] { PV.ViewID });
        }
        else
        {
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (added==true && dead == true && onetime==true && PhotonNetwork.IsMasterClient==false)
        {
            CreateController();
            onetime = false;
            Debug.Log("revive");
        }
        if (dead == false)
        {
            added = false;
        }
     
    }

    void Update()
    {
        if (bing == true)
        {
       
            bing = false;
            bong = false;
        }

    }
    public void Adder()
    {
        added = true;
        Debug.Log(added);
    }

    public void Die()
    {
        
        PhotonNetwork.Destroy(controller);
        if (PhotonNetwork.IsMasterClient)
        {
            
            Debug.Log("Minotaurdead");
           
        }
        else
        {
            dead = true;
        }
    }

}

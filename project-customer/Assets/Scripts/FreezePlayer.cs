using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    private GameObject player;
    private Camera camera;
    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camera= Camera.main;

        player.GetComponent<MouseLook>().CanLookAround = false;
        camera.GetComponent<MouseLook>().CanLookAround = false;
        Console.WriteLine("frozen camera");

    }
    public void OnDestroy()
    {
        player.GetComponent<MouseLook>().CanLookAround = true;
        camera.GetComponent<MouseLook>().CanLookAround = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect : MonoBehaviour
{
    void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            MouseLook[] mouseLooks = GetComponentsInParent<MouseLook>();
            foreach(MouseLook mouseLook in mouseLooks)
            {
                mouseLook.CanLookAround = true;
            }
            Destroy(GetComponentInChildren<InspectRotate>().gameObject);
            GameManager.Instance.crosshair.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

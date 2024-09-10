using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectRotate : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    private bool buttonIsPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            buttonIsPressed = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            buttonIsPressed = false;
        }


        if (buttonIsPressed)
        {
            Vector3 r = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed;

            transform.Rotate(r);
        }
    }
}

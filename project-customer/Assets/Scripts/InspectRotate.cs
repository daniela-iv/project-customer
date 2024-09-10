using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectRotate : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 r = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed;

        transform.Rotate(r);
    }
}

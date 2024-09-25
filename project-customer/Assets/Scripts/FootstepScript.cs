using UnityEngine;
using System.Collections;

public class FootStepScript : MonoBehaviour
{
    public float stepRateWalk = 0.5f;
    public float stepRateRun = 0.25f;
    private float stepRate = 0.5f;
    public float stepCoolDown;
    public AudioClip footStep;
    public AudioSource audioSource;
    public SimplePhysicsControls playercontrols;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Sprint"))
        {
            stepRate = stepRateRun;
        }
        else
        {
            stepRate = stepRateWalk;
        }

        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f && playercontrols.canMove)
        {
            audioSource.pitch = 1f + Random.Range(-0.2f, 0.3f);
            audioSource.volume = Random.Range(10, 100);
            audioSource.clip = footStep;
            audioSource.Play();
            stepCoolDown = stepRate;
        }
        else if(stepCoolDown < 0f)
        {
            audioSource.Stop();
        }
    }
}
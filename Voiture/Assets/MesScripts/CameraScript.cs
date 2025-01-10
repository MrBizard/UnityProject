using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera FirstPersonCam; 
    public CinemachineVirtualCamera ThirdPersonCam;

    private bool isCamera1Active = true;

    public AnimationCurve transitionCurve;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
       
       
    }

    void SwitchCamera()
    {
        if (isCamera1Active)
        {
            FirstPersonCam.Priority = 10; 
            ThirdPersonCam.Priority = 20; 
        }
        else
        {
            FirstPersonCam.Priority = 20; 
            ThirdPersonCam.Priority = 10; 
        }

        isCamera1Active = !isCamera1Active; 
    }


}
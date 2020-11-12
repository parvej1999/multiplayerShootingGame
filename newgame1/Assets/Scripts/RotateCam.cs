using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    public int  CameraSensitivityX = 10;
    void Update()
    {
        if(Input.GetButton("E")){
            if(Input.GetAxis("Mouse X")<0){
                transform.Rotate(0, -CameraSensitivityX, 0);
            }
            if(Input.GetAxis("Mouse X")>0){
                transform.Rotate(0, CameraSensitivityX, 0);
            }
        }
    }
}

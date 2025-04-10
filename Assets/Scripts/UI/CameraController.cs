/*****************************************************************************
// File Name : CameraController.cs
// Author : Lucas Fehlberg
// Creation Date : April 6, 2025
// Last Updated : April 6, 2025
//
// Brief Description : Camera Controls
*****************************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    const float CAMERA_SPEED = 1f;
    const float CAMERA_ROT_SPEED = 45f;

    [SerializeField] private PlayerInput pInput;
    private InputAction upDown;
    private InputAction leftRight;

    [SerializeField] private float rotation = 0;

    private float percentage = 0;

    [SerializeField] private GameObject startCameraPos;
    [SerializeField] private GameObject topCameraPos;

    /// <summary>
    /// Setup actions
    /// </summary>
    private void Start()
    {
        upDown = pInput.currentActionMap.FindAction("CameraUpDown");
        leftRight = pInput.currentActionMap.FindAction("CameraLeftRight");
    }

    /// <summary>
    /// Smooth camera movement
    /// </summary>
    private void Update()
    {
        rotation += -1 * leftRight.ReadValue<float>() * CAMERA_ROT_SPEED * Time.deltaTime;

        percentage += upDown.ReadValue<float>() * CAMERA_SPEED * Time.deltaTime;
        percentage = Mathf.Clamp01(percentage);

        transform.position = Vector3.Lerp(startCameraPos.transform.position, topCameraPos.transform.position, 
            percentage);

        transform.rotation = Quaternion.Lerp(startCameraPos.transform.rotation, topCameraPos.transform.rotation,
            percentage);

        if(transform.parent.rotation.eulerAngles.z != rotation)
        {
            transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, 
                Quaternion.Euler(0, rotation, 0), 1f);
        }
    }
}

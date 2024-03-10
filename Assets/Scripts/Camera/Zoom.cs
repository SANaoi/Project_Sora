using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class Zoom : MonoBehaviour
{
    private CinemachineFramingTransposer cinemachineFramingTransposer;
    private CinemachineInputProvider inputProvider;
    private float currentDistance;
    [SerializeField][Range(0f, 10f)] private float minimumDistance = 1f;
    [SerializeField][Range(0f, 10f)] private float maximumDistance = 6f;
    [SerializeField][Range(0f, 10f)] private float DefaultDistance = 3f;
    private void Awake()
    {
        cinemachineFramingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        inputProvider = GetComponent<CinemachineInputProvider>();
        currentDistance = DefaultDistance;
        cinemachineFramingTransposer.m_CameraDistance = DefaultDistance;

    }
    private void LateUpdate()
    {
        CameraZoom();
    }

    public void CameraZoom()
    {
        currentDistance = Mathf.Clamp(inputProvider.GetAxisValue(2) + currentDistance, minimumDistance, maximumDistance);
        cinemachineFramingTransposer.m_CameraDistance = currentDistance;
    }
}

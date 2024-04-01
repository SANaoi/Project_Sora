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
    private float m_targetDistance;
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

    public void CameraZoom(float targetDistance = 0)
    {
        if (targetDistance != 0)
        {
            m_targetDistance = targetDistance;
        }
        if (m_targetDistance != 0f)
        {
            if (currentDistance > m_targetDistance)
                currentDistance = Mathf.Lerp(currentDistance, targetDistance, 2f * Time.deltaTime);
            else
                currentDistance = Mathf.Lerp(targetDistance, currentDistance, 2f * Time.deltaTime);
        }

        if (Mathf.Abs(currentDistance - m_targetDistance) < 0.1f) currentDistance = m_targetDistance;
        if (currentDistance == m_targetDistance) m_targetDistance = 0f;

        currentDistance = Mathf.Clamp(inputProvider.GetAxisValue(2) + currentDistance, minimumDistance, maximumDistance);
        cinemachineFramingTransposer.m_CameraDistance = currentDistance;
    }
}

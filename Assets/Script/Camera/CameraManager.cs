using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public Vector3 GetCameraPosition()
    {
        return _cinemachineVirtualCamera.transform.position;
    }

    public void SetCameraPosition(Vector3 newCameraPosition)
    {
        _cinemachineVirtualCamera.transform.position = newCameraPosition;
    }

    public CinemachineVirtualCamera GetCinemachineVirtualCamera()
    {
        return _cinemachineVirtualCamera;
    }
}

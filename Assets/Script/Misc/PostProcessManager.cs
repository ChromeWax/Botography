using Botography.Map;
using Botography.Player;
using Botography.Player.StatusEffects;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager Instance { get; private set; }

    [SerializeField] private Volume _postProcess;
    [SerializeField] private PostProcessType[] _postProcessTypes;
    public string _startPostProcessType;
    [SerializeField] private float _transitionSpeed = 1f;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private PostProcessType previousPostProcessType;
    private PostProcessType currentPostProcessType;

    private Vignette masterVignette;
    private ColorAdjustments masterColorAdjustments;
    private WhiteBalance masterWhiteBalance;

    private Vignette currentVignette;
    private ColorAdjustments currentColorAdjustments;
    private WhiteBalance currentWhiteBalance;
    private float currentLensSize;

    public bool Underwater;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        cinemachineVirtualCamera = CameraManager.Instance.GetCinemachineVirtualCamera();

        previousPostProcessType = null;

        _postProcess.profile.TryGet(out masterVignette);
        _postProcess.profile.TryGet(out masterColorAdjustments);
        _postProcess.profile.TryGet(out masterWhiteBalance);

        ChangePostProcessType(_startPostProcessType);

        masterVignette.color.value = currentVignette.color.value;
        masterVignette.intensity.value = currentVignette.intensity.value;

        masterColorAdjustments.postExposure.value = currentColorAdjustments.postExposure.value;
        masterColorAdjustments.saturation.value = currentColorAdjustments.saturation.value;

        masterWhiteBalance.temperature.value = currentWhiteBalance.temperature.value;

        currentLensSize = currentPostProcessType.lenSize;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = currentLensSize;
    }

    private void Update()
    {
        if (StatusEffectsHandler.Instance.IsNearDeath())
            ChangeDangerPostProcessType();
        else
            ChangeNormalPostProcessType();

        masterVignette.color.value = Color.Lerp(masterVignette.color.value, currentVignette.color.value, Time.deltaTime * _transitionSpeed);
        masterVignette.intensity.value = Mathf.Lerp(masterVignette.intensity.value, currentVignette.intensity.value, Time.deltaTime * _transitionSpeed);

        masterColorAdjustments.postExposure.value = Mathf.Lerp(masterColorAdjustments.postExposure.value, currentColorAdjustments.postExposure.value, Time.deltaTime * _transitionSpeed);
        masterColorAdjustments.saturation.value = Mathf.Lerp(masterColorAdjustments.saturation.value, currentColorAdjustments.saturation.value, Time.deltaTime * _transitionSpeed);

        masterWhiteBalance.temperature.value = Mathf.Lerp(masterWhiteBalance.temperature.value, currentWhiteBalance.temperature.value, Time.deltaTime * _transitionSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, currentLensSize, Time.deltaTime * _transitionSpeed);
    }

    public void ChangePostProcessType(string type)
    {
        if (!Underwater)
        {
            for (int i = 0; i < _postProcessTypes.Length; i++)
            {
                if (_postProcessTypes[i].type == type)
                {
                    previousPostProcessType = currentPostProcessType;
                    currentPostProcessType = _postProcessTypes[i];

                    currentPostProcessType.postProcessProfile.TryGet(out currentVignette);
                    currentPostProcessType.postProcessProfile.TryGet(out currentColorAdjustments);
                    currentPostProcessType.postProcessProfile.TryGet(out currentWhiteBalance);
                    currentLensSize = currentPostProcessType.lenSize;
                    return;
                }
            }
        }
    }

    private void ChangeDangerPostProcessType()
    {
        currentPostProcessType.postNearDeathProcessProfile.TryGet(out currentVignette);
        currentPostProcessType.postNearDeathProcessProfile.TryGet(out currentColorAdjustments);
        currentPostProcessType.postNearDeathProcessProfile.TryGet(out currentWhiteBalance);
        currentLensSize = currentPostProcessType.lenSize - 0.5f;
    }

    private void ChangeNormalPostProcessType()
    {
        currentPostProcessType.postProcessProfile.TryGet(out currentVignette);
        currentPostProcessType.postProcessProfile.TryGet(out currentColorAdjustments);
        currentPostProcessType.postProcessProfile.TryGet(out currentWhiteBalance);
        currentLensSize = currentPostProcessType.lenSize;
    }

    public string GetPreviousProcessType()
    {
        return previousPostProcessType.type;
    }

    public string GetCurrentProcessType()
    {
        return currentPostProcessType.type;
    }
	
	public void SetCurrentProcessType(string processType)
	{
        _startPostProcessType = processType;
	}

    public void SetUnderwater(bool enabled)
    {
        Underwater = enabled;

        if (enabled)
        {
            for (int i = 0; i < _postProcessTypes.Length; i++)
            {
                if (_postProcessTypes[i].type == "Underwater")
                {
                    previousPostProcessType = currentPostProcessType;
                    currentPostProcessType = _postProcessTypes[i];

                    currentPostProcessType.postProcessProfile.TryGet(out currentVignette);
                    currentPostProcessType.postProcessProfile.TryGet(out currentColorAdjustments);
                    currentPostProcessType.postProcessProfile.TryGet(out currentWhiteBalance);
                    currentLensSize = currentPostProcessType.lenSize;
                    return;
                }
            }
        }
        else
        {
            SunlightLevel light = PlayerManager.Instance.GetLightLevel();
            string text = "";

            switch (light)
            {
                case SunlightLevel.fullShade:
                {
                    text = "Midnight";
                    break;
                }
                case SunlightLevel.partialShade:
                {
                    text = "Cold";
                    break;
                }
                case SunlightLevel.halfAndHalf:
                {
                    text = "Forest";
                    break;
                }
                case SunlightLevel.partialSun:
                {
                    text = "Swamp";
                    break;
                }
                case SunlightLevel.fullSun:
                {
                    text = "Hot";
                    break;
                }
            }

            for (int i = 0; i < _postProcessTypes.Length; i++)
            {
                if (_postProcessTypes[i].type == text)
                {
                    previousPostProcessType = currentPostProcessType;
                    currentPostProcessType = _postProcessTypes[i];

                    currentPostProcessType.postProcessProfile.TryGet(out currentVignette);
                    currentPostProcessType.postProcessProfile.TryGet(out currentColorAdjustments);
                    currentPostProcessType.postProcessProfile.TryGet(out currentWhiteBalance);
                    currentLensSize = currentPostProcessType.lenSize;
                    return;
                }
            }
        }

        

    }
}

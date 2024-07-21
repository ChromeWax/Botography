using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PostProcessType
{
    public VolumeProfile postProcessProfile;
    public VolumeProfile postNearDeathProcessProfile;
    public float lenSize;
    public string type;
}

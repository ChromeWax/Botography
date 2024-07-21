using System.Collections;
using System.Collections.Generic;
using Botography.Map;
using UnityEngine;

public class SignInteractableObject : InteractableObject
{
    [SerializeField] private RegionSO _region;

    public RegionSO Region => _region;
}

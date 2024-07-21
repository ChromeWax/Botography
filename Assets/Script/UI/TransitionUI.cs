using System.Collections;
using System.Collections.Generic;
using Botography.Dependencies;
using Botography.Dependencies.Interfaces;
using UnityEngine;

public class TransitionUI : UiCollectionBase
{
    [SerializeField] private Animator _fadeToBlack;

    public Animator FadeToBlack => _fadeToBlack;
}

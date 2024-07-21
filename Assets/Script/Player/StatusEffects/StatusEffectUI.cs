using Botography.Player.StatusEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [HideInInspector] public TimedStatusEffect TEffect;
    [HideInInspector] public bool BubbleOut;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _icon;
    [SerializeField] private Animator _anim;
    
    public bool TimerActive
    {
        get
        {
            return TEffect != null && TEffect.CurrentTime > 0;
        }
    }

    [SerializeField] private TextMeshProUGUI _timerText;

    public void SetTimer(TimedStatusEffect effect)
    {
        TEffect = effect;
        _timerText.gameObject.SetActive(true);
        _timerText.text = effect.CurrentTime.ToString();
        _fill.gameObject.SetActive(true);
        _fill.fillAmount = effect.CurrentTime / effect.Duration;
        _icon.gameObject.SetActive(true);
        StartEffect();
	}

    public void AddToTimer(TimedStatusEffect addeffect)
    {
        TEffect += addeffect;
    }

	public void UpdateTime(float deltaTime)
    {
		if (TimerActive)
		{
			TEffect.CurrentTime -= deltaTime;
			_timerText.text = ((int)TEffect.CurrentTime).ToString();
			_fill.fillAmount = TEffect.CurrentTime / TEffect.Duration;
            if (TEffect.CurrentTime <= 0)
            {
                EndTimer();
			}
		}
	}

    public void StartEffect()
    {
		_anim.SetBool("Status Active", true);
		_anim.SetBool("Status Inactive", false);
        BubbleOut = true;
	}

    public void EndEffect()
    {
		_anim.SetBool("Status Inactive", true);
		_anim.SetBool("Status Active", false);
        BubbleOut = false;
	}

    public void EndTimer(Action callback = null)
    {
        if (TEffect == null)
        {
            return;
        }

		TEffect = null;
		_timerText.gameObject.SetActive(false);
		_fill.gameObject.SetActive(false);
        EndEffect();

        if (callback != null)
        {
            callback.Invoke();
        }
	}
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CardComponent : MonoBehaviour
{
    public TMP_Text cardValueText;
    public int cardValue;
    public bool isTurned = false;
    public bool isMatched = false;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(PrepareCard());
    }

    private IEnumerator PrepareCard()
    {
        isTurned = true;
        yield return new WaitForEndOfFrame();
        AnimationManager.Instance.SmoothRotateY180(transform, 0.5f);
        yield return new WaitForSeconds(1f);
        AnimationManager.Instance.SmoothRotateYReverse(transform, 0.5f);
        isTurned = false;
    }

    private void OnMouseDown()
    {
        if(isTurned) return;
        isTurned = true;
        ScoreManager.Instance.SetTurnsCount();
        AudioManager.Instance.PlaySound(AudioManager.SoundType.Tap);
        AnimationManager.Instance.SmoothRotateY180(transform, 0.5f);
        Invoke("DelayTapResponse", 0.5f);
    }

    private void DelayTapResponse()
    {
        CardManager.Instance.OnCardTapped(this);
    }

    public void SetCardValue(int value)
    {
        cardValue = value;
        cardValueText.text = cardValue.ToString();
    }

    public void CardMatched()
    {
        isMatched = true;
        Vector3 targetScale = initialScale * 1.2f;
        AudioManager.Instance.PlaySound(AudioManager.SoundType.CardMatched);
        AnimationManager.Instance.ScaleUpAndShrink(transform, targetScale, 0.2f, 0.5f);
    }

    public IEnumerator CardNotMatched()
    {
        yield return new WaitForEndOfFrame();
        isTurned = false;
        AudioManager.Instance.PlaySound(AudioManager.SoundType.CardMismatched);
        AnimationManager.Instance.ShakeObject(transform, 0.75f, 0.1f);
        yield return new WaitForSeconds(0.5f);
        AnimationManager.Instance.SmoothRotateYReverse(transform, 0.5f);
        yield return null;
    }
}
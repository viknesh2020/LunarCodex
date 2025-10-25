using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviourSingleton<AnimationManager>
{
    public void SmoothRotateY180(Transform targetTransform, float duration)
    {
        if (targetTransform == null) return;
        StartCoroutine(RotateCoroutine(targetTransform, Quaternion.Euler(0, 180, 0), duration));
    }

    public void SmoothRotateYReverse(Transform targetTransform, float duration)
    {
        if (targetTransform == null) return;
        StartCoroutine(RotateCoroutine(targetTransform, Quaternion.Euler(0, -180, 0), duration));
    }

    public void ShakeObject(Transform targetTransform, float duration, float magnitude)
    {
        if (targetTransform == null) return;
        StartCoroutine(ShakeCoroutine(targetTransform, duration, magnitude));
    }

    public void ScaleUpAndShrink(Transform targetTransform, Vector3 targetScale, float scaleUpDuration, float shrinkDuration)
    {
        if (targetTransform == null) return;
        StartCoroutine(ScaleUpAndShrinkCoroutine(targetTransform, targetScale, scaleUpDuration, shrinkDuration));
    }

    private IEnumerator RotateCoroutine(Transform target, Quaternion relativeRotation, float duration)
    {
        if (target == null || duration <= 0) yield break;

        float timer = 0f;
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = startRotation * relativeRotation; 

        while (timer < duration)
        {
            float t = timer / duration;
            t = t * t * (3f - 2f * t); 

            target.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            timer += Time.deltaTime;
            yield return null; 
        }

        if (target != null)
        {
            target.rotation = endRotation;
        }
    }

    private IEnumerator ShakeCoroutine(Transform target, float duration, float magnitude)
    {
        if (target == null || duration <= 0) yield break;

        Vector3 originalLocalPosition = target.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            target.localPosition = originalLocalPosition + Random.insideUnitSphere * magnitude;

            timer += Time.deltaTime;
            yield return null; 
        }

        if (target != null)
        {
            target.localPosition = originalLocalPosition;
        }
    }

    private IEnumerator ScaleUpAndShrinkCoroutine(Transform target, Vector3 targetScale, float scaleUpDuration, float shrinkDuration)
    {
        if (target == null) yield break;

        if (scaleUpDuration > 0)
        {
            Vector3 originalScale = target.localScale;
            float timer = 0f;

            while (timer < scaleUpDuration)
            {
                float t = timer / scaleUpDuration;
                t = t * t * (3f - 2f * t); 

                target.localScale = Vector3.Lerp(originalScale, targetScale, t);

                timer += Time.deltaTime;
                yield return null;
            }
        }

        if (target == null) yield break; 
        target.localScale = targetScale;

        if (shrinkDuration > 0)
        {
            Vector3 shrinkFromScale = target.localScale; 
            Vector3 shrinkToScale = Vector3.zero;     
            float timer = 0f;

            while (timer < shrinkDuration)
            {
                float t = timer / shrinkDuration;
                t = t * t * (3f - 2f * t); 

                target.localScale = Vector3.Lerp(shrinkFromScale, shrinkToScale, t);

                timer += Time.deltaTime;
                yield return null;
            }
        }

        if (target != null)
        {
            target.localScale = Vector3.zero;
        }
    }
}


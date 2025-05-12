using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    private Vector3 originalPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        originalPos = transform.localPosition;
    }

    public static CameraShake Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.1f)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlashEffect : MonoBehaviour
{
    public Image slashImage;     // 칼 자국 이미지
    public float displayTime = 0.2f;    // 표시 시간
    public float fadeTime = 0.3f;       // 페이드아웃 시간

    [Range(0f, 1f)]
    public float initialAlpha = 0.5f;     // 초기 투명도 (0: 완전 투명, 1: 완전 불투명)
    [Range(0f, 1f)]
    public float finalAlpha = 0f;       // 최종 투명도 (0: 완전 투명, 1: 완전 불투명)
    
    public void ShowSlashEffect()
    {
        StopAllCoroutines();
        StartCoroutine(DisplaySlashEffect());
    }
    
    private IEnumerator DisplaySlashEffect()
    {
        // 알파값 초기화
        Color c = slashImage.color;
        c.a = initialAlpha;
        slashImage.color = c;
        
        slashImage.gameObject.SetActive(true);
        
        // 표시 시간 대기
        yield return new WaitForSeconds(displayTime);
        
        // 페이드아웃 (또는 투명도 변경)
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(initialAlpha, finalAlpha, elapsed / fadeTime);
            slashImage.color = c;
            yield return null;
        }
        
        // 최종 투명도가 0이면 오브젝트를 비활성화, 아니면 유지
        if (finalAlpha <= 0.01f)
            slashImage.gameObject.SetActive(false);
    }
}
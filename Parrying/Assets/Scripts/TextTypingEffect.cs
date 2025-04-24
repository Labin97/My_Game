using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextTypingEffect : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float typingSpeed = 20f; // 초당 표시할 글자 수
    public float initialDelay = 0f;

    private string fullText;

    private void Start()
    {
        // 필요한 경우 여기서 초기화
    }

    public void TypeText(string text)
    {
        fullText = text;
        
        // 텍스트 초기화
        textMeshPro.text = fullText;
        textMeshPro.maxVisibleCharacters = 0;
        
        // 기존 Tween 중지
        DOTween.Kill(textMeshPro);
        
        // DOTween을 사용한 타이핑 효과
        textMeshPro.DOMaxVisibleCharacters(fullText.Length, fullText.Length / typingSpeed)
            .SetDelay(initialDelay)
            .SetEase(Ease.Linear);
    }

    // 타이핑을 즉시 완료하는 함수
    public void CompleteTyping()
    {
        DOTween.Kill(textMeshPro);
        textMeshPro.maxVisibleCharacters = fullText.Length;
    }
}
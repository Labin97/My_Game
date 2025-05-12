using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private SpriteRenderer spriteRenderer;
    private Material scrollMaterial;
    private Vector2 offset = Vector2.zero;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 기존 Material을 복제하여 인스턴스 생성 (다른 오브젝트에 영향 없도록)
        scrollMaterial = new Material(spriteRenderer.material);
        spriteRenderer.material = scrollMaterial;
    }
    
    void Update()
    {
        // X축으로 스크롤 (오른쪽에서 왼쪽으로 이동)
        offset.x += scrollSpeed * Time.deltaTime;
        
        // Material의 mainTextureOffset 속성을 변경하여 스크롤 효과 생성
        scrollMaterial.mainTextureOffset = offset;
    }
    
    // 씬 전환 시 메모리 누수 방지
    void OnDestroy()
    {
        if (scrollMaterial != null)
        {
            Destroy(scrollMaterial);
        }
    }
}
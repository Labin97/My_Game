using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private BoxCollider2D weaponCollider;
    private Vector2 defaultSize;

    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<BoxCollider2D>();
        defaultSize = weaponCollider.size;
    }

    public void SetColliderSize(Vector2 newSize)
    {
        weaponCollider.size = newSize;
    }

    public void ResetColliderSize()
    {
        weaponCollider.size = defaultSize;
    }
}

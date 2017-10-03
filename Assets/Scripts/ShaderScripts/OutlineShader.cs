using UnityEngine;

public class OutlineShader : MonoBehaviour
{
    public Color outlineColor = Color.white;
    [Range(1,5)]
    public int outlineWidth = 1;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock materialPB;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialPB = new MaterialPropertyBlock();

        UpdateOutline(true);
    }

    void onDestroy()
    {
        UpdateOutline(false);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void UpdateOutline(bool outline)
    {
        spriteRenderer.GetPropertyBlock(materialPB);
        materialPB.SetFloat("_Outline", outline ? 1f : 0);
        materialPB.SetColor("_OutlineColor", outlineColor);
        materialPB.SetFloat("_OutlineSize", outlineWidth);
        spriteRenderer.SetPropertyBlock(materialPB);
    }
}
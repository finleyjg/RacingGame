using UnityEngine;
using UnityEngine.UI;

public class FlashingButton : MonoBehaviour
{
    public float flashSpeed = 2f;
    public Color baseColor = Color.white;       
    public Color highlightColor = Color.green;  

    private Text buttonText;

    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (buttonText == null) return;

        float t = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f;

        Color flashColor = Color.Lerp(baseColor, highlightColor, t);
        flashColor.a = 1f; 

        buttonText.color = flashColor;

        float scale = 1 + 0.05f * Mathf.Sin(Time.time * flashSpeed);
        transform.localScale = new Vector3(scale, scale, 1);

    }
}

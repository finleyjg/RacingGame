using UnityEngine;
using UnityEngine.UI;

public class MenuStartKey : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public float flashSpeed = 2f;

    void Update()
    {
        
        canvasGroup.alpha = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
   
            Debug.Log("Game Started!");
        }
    }
}

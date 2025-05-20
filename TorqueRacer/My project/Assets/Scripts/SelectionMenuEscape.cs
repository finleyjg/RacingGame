using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionMenuEscape : MonoBehaviour
{
    public Animator transition;  
    public float transitionTime = 0.5f; 
    public string mainMenuSceneName = "MainMenu";  

    public AudioSource escapeKeySFX; 

    private bool isTransitioning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            StartCoroutine(LoadMainMenuWithTransition());
        }
    }

    private IEnumerator LoadMainMenuWithTransition()
    {
        isTransitioning = true;

        if (escapeKeySFX != null)
        {
            escapeKeySFX.Play();
            yield return new WaitForSeconds(escapeKeySFX.clip.length);
        }

        if (transition != null)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}

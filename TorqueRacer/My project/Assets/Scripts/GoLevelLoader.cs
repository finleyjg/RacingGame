using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoLevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;

    public AudioSource goButtonSFX;
    public AudioSource backgroundMusic;
    public float musicFadeDuration = 2f;

    private bool isTransitioning = false;


    public void OnGoButtonPressed()
    {
        if (!isTransitioning)
        {
            StartCoroutine(PlaySFXFadeMusicAndLoad());
        }
    }

    private IEnumerator PlaySFXFadeMusicAndLoad()
    {
        isTransitioning = true;

        if (goButtonSFX != null)
        {
            goButtonSFX.Play();
            yield return new WaitForSeconds(goButtonSFX.clip.length);
        }

        if (backgroundMusic != null)
        {
            yield return StartCoroutine(FadeOutMusic());
        }

        if (transition != null)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }

        LoadNextLevel();
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = backgroundMusic.volume;
        float t = 0f;

        while (t < musicFadeDuration)
        {
            t += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0, t / musicFadeDuration);
            yield return null;
        }

        backgroundMusic.Stop();
        backgroundMusic.volume = startVolume;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

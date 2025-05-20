using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public AudioSource spaceKeySFX;  
    public AudioSource backgroundMusic;  
    public float musicFadeDuration = 2f;

    private bool isTransitioning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning)
        {
            StartCoroutine(PlaySFXFadeMusicAndLoad());
        }
    }

    private IEnumerator PlaySFXFadeMusicAndLoad()
    {
        isTransitioning = true;

        if (spaceKeySFX != null)
        {
            spaceKeySFX.Play();
            yield return new WaitForSeconds(spaceKeySFX.clip.length);
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

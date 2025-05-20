using System.Collections;
using UnityEngine;

public class MusicFader : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public float fadeDuration = 2f;

    public IEnumerator FadeOutMusic()
    {
        float startVolume = backgroundMusic.volume;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        backgroundMusic.Stop();
        backgroundMusic.volume = startVolume;
    }
}

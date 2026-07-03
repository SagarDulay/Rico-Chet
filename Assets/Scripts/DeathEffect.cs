using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathEffect : MonoBehaviour
{
    public static DeathEffect Instance;

    [SerializeField] private Image flashImage;
    [SerializeField] private float flashInDuration = 0.2f;
    [SerializeField] private float flashOutDuration = 0.4f;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDeathFlash()
    {
        StartCoroutine(FlashSequence());
    }

    private IEnumerator FlashSequence()
    {
        
        yield return StartCoroutine(Fade(0f, 1f, flashInDuration));
        
        yield return StartCoroutine(Fade(1f, 0f, flashOutDuration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color c = flashImage.color;
            c.a = Mathf.Lerp(from, to, elapsed / duration);
            flashImage.color = c;
            yield return null;
        }
    }
}
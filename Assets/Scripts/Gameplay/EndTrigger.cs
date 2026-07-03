using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class EndTrigger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject menuButton;

    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float textDelayTime = 1f;
    [SerializeField] private float textFadeInTime = 1f;

    private bool _triggered = false;
    private GameObject _player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (!_triggered) return;

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.M))
            SceneManager.LoadScene("MainMenu");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        _triggered = true;
        _player = other.gameObject;
        StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence()
    {
        
        _player.GetComponent<PlayerController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
        restartButton.SetActive(false);
        menuButton.SetActive(false);

        
        winCanvas.SetActive(true);
        SetAlpha(backgroundImage, 0f);
        SetTextAlpha(winText, 0f);
        SetTextAlpha(subtitleText, 0f);

       
        yield return StartCoroutine(FadeImage(backgroundImage, 0f, 1f, fadeInDuration));

        
        yield return new WaitForSeconds(textDelayTime);

        
        yield return StartCoroutine(FadeText(winText, 0f, 1f, textFadeInTime));

       
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeText(subtitleText, 0f, 1f, textFadeInTime));

        
        yield return new WaitForSeconds(0.5f);
        restartButton.SetActive(true);
        menuButton.SetActive(true);
    }

    private IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(img, Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetAlpha(img, to);
    }

    private IEnumerator FadeText(TextMeshProUGUI tmp, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetTextAlpha(tmp, Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetTextAlpha(tmp, to);
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    private void SetTextAlpha(TextMeshProUGUI tmp, float alpha)
    {
        Color c = tmp.color;
        c.a = alpha;
        tmp.color = c;
    }
}
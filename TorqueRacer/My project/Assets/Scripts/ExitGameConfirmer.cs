using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameConfirmer : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject titleText;
    public GameObject pressSpaceText;

    private bool isConfirmingExit = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isConfirmingExit)
            {
                ShowExitConfirmation();
            }
            else
            {
                HideExitConfirmation();
            }
        }
    }

    public void ShowExitConfirmation()
    {
        confirmationPanel.SetActive(true);
        if (titleText != null) titleText.SetActive(false);
        if (pressSpaceText != null) pressSpaceText.SetActive(false);
        isConfirmingExit = true;
    }

    public void HideExitConfirmation()
    {
        confirmationPanel.SetActive(false);
        if (titleText != null) titleText.SetActive(true);
        if (pressSpaceText != null) pressSpaceText.SetActive(true);
        isConfirmingExit = false;
    }

    public void ConfirmExit()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        HideExitConfirmation();
    }
}

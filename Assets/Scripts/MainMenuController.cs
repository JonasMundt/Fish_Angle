using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Main Menu")]
    [SerializeField] private Button continueButton;

    [Header("Options")]
    [SerializeField] private Slider volumeSlider;

    private const string VolumeKey = "MasterVolume";

    private void Start()
    {
        LoadVolume();
        UpdateContinueButton();
    }

    public void StartNewGame()
    {
        SaveManager.DeleteSave();

        SaveManager.RequestNewGame();

        SceneManager.LoadScene(
            "SampleScene"
        );
    }

    public void ContinueGame()
    {
        if (!SaveManager.SaveExists())
        {
            return;
        }

        SaveManager.RequestContinue();

        SceneManager.LoadScene(
            "SampleScene"
        );
    }

    private bool HasSaveGame()
    {
        return SaveManager.SaveExists();
    }

    private void UpdateContinueButton()
    {
        if (continueButton != null)
        {
            continueButton.interactable =
                HasSaveGame();
        }
    }

    public void OpenOptions()
    {
        if (mainMenuPanel)
        {
            mainMenuPanel.SetActive(false);
        }

        if (optionsPanel)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        if (optionsPanel)
        {
            optionsPanel.SetActive(false);
        }

        if (mainMenuPanel)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume =
            value / 100f;

        PlayerPrefs.SetFloat(
            VolumeKey,
            value
        );

        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float savedVolume =
            PlayerPrefs.GetFloat(
                VolumeKey,
                100f
            );

        if (volumeSlider != null)
        {
            volumeSlider.SetValueWithoutNotify(
                savedVolume
            );
        }

        AudioListener.volume =
            savedVolume / 100f;
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying =
            false;
#endif
    }
}
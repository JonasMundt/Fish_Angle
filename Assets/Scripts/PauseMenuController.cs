using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMainPanel;
    [SerializeField] private GameObject pauseOptionsPanel;

    [Header("Options")]
    [SerializeField] private Slider volumeSlider;

    [Header("Save")]
    [SerializeField] private GameSaveController gameSaveController;

    private bool isPaused = false;

    private const string VolumeKey = "MasterVolume";

    private void Start()
    {
        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }

        if (pauseOptionsPanel)
        {
            pauseOptionsPanel.SetActive(false);
        }

        Time.timeScale = 1f;

        LoadVolume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pausePanel)
        {
            pausePanel.SetActive(true);
        }

        if (pauseMainPanel)
        {
            pauseMainPanel.SetActive(true);
        }

        if (pauseOptionsPanel)
        {
            pauseOptionsPanel.SetActive(false);
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenOptions()
    {
        if (pauseMainPanel)
        {
            pauseMainPanel.SetActive(false);
        }

        if (pauseOptionsPanel)
        {
            pauseOptionsPanel.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        if (pauseOptionsPanel)
        {
            pauseOptionsPanel.SetActive(false);
        }

        if (pauseMainPanel)
        {
            pauseMainPanel.SetActive(true);
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

    public void ReturnToMainMenu()
    {
        if (gameSaveController)
        {
            gameSaveController.SaveCurrentGame();
        }

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            "MainMenu"
        );
    }

    public void QuitGame()
    {
        if (gameSaveController)
        {
            gameSaveController.SaveCurrentGame();
        }

        Time.timeScale = 1f;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying =
            false;
#endif
    }
}
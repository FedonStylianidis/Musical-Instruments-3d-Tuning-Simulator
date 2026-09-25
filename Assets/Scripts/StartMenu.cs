using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [Header("Instructions")]
    public GameObject InstructionsPanel;


    // Shared between the StartMenu and EgoController.
    //
    // FALSE = first time the game has started.
    // TRUE  = gameplay has already begun.
    //
    // Because it is static, it survives
    // MainSceneFinal being reloaded.
    public static bool GameHasStarted = false;


    void Start()
    {
        // Always begin with the instructions hidden.
        InstructionsPanel.SetActive(false);
    }


    public void newGame()
    {
        // FIRST START
        //
        // The scene is already freshly loaded,
        // so there is no reason to reload it.
        if (!GameHasStarted)
        {
            GameHasStarted = true;


            EgoController ego =
                FindFirstObjectByType<EgoController>();


            if (ego != null)
            {
                ego.startGameplay();
            }


            return;
        }


        // RESTART
        //
        // Gameplay has already begun, so NEW
        // now means restart the simulation.
        //
        // GameHasStarted remains TRUE because
        // it is static.
        Time.timeScale = 1f;


        SceneManager.LoadScene(
            "MainSceneFinal"
        );
    }


    public void showInstructions()
    {
        InstructionsPanel.SetActive(true);
    }


    public void hideInstructions()
    {
        InstructionsPanel.SetActive(false);
    }


    public bool instructionsAreOpen()
    {
        return InstructionsPanel.activeSelf;
    }


    public void quitGame()
    {
        Application.Quit();


#if UNITY_EDITOR

        UnityEditor.EditorApplication
            .isPlaying = false;

#endif
    }
}
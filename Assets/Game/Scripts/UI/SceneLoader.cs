using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField] private Animator FadeAnimator;
    [SerializeField] private GameObject FadeImage;
    [SerializeField] private float transitionTime = 1.0f;


    // --------------------------------------------------------------------
    private void Awake() {
        FadeImage.SetActive(true);
    }


    // --------------------------------------------------------------------
    public void StartGame() {
        StartCoroutine(LoadScene("GameScene"));
    }


    // --------------------------------------------------------------------
    public void Retry() {
        StartGame();
    }


    // --------------------------------------------------------------------
    public void ReturnToMainMenu() {
        StartCoroutine(LoadScene("MainMenu"));
    }


    // --------------------------------------------------------------------
    public void ExitGame() {
        Application.Quit();
    }


    // --------------------------------------------------------------------
    private IEnumerator LoadScene(string sceneName) {
        FadeAnimator.SetTrigger("FadeTrigger");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

}
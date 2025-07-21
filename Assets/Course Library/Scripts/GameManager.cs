using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int _score;
    int live = 3;

    public float spawnRate = 1;
    public bool isGameActive;
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public bool isPause;
    public  TextMeshProUGUI liveText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public List<GameObject> targetPF;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            ChangePause();
        }
    }

    public void StartGame(int difficulty) {
        spawnRate /= difficulty;
        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        _score = 0;
        StartCoroutine(SpawnTarget());
    }

    IEnumerator SpawnTarget() {
        while(isGameActive){
            yield return new WaitForSeconds(spawnRate);
            int _index = Random.Range(0, targetPF.Count);
            Instantiate(targetPF[_index]);
        };
    }

    public void UpdateScore(int addScore) {
        _score += addScore;
        scoreText.text = "Score: " + _score;
    }

    public void UpdateLive()
    {
        if(isGameActive){
            live--;
            liveText.text = "Live: " + live;
            if (live == 0)
            {
                StopGame();
            }
        }
    }

    public void StopGame() {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ChangePause() {
        if(!isPause){
            isPause = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }else{
            isPause = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
}

using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    Button button;
    GameManager _gameManager;
    public int difficulty;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void SetDifficulty() {
        _gameManager.StartGame(difficulty);
    }

}

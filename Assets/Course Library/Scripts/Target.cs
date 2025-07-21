using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    float _torqueRange = 2;
    GameManager _gameManager;
    Rigidbody _targetRB;

    public int pointValue;
    public ParticleSystem tapEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        _targetRB = GetComponent<Rigidbody>();
        _targetRB.AddForce(Vector2.up * Random.Range(12, 16), ForceMode.Impulse);
        _targetRB.AddTorque(Random.Range(-_torqueRange, _torqueRange), Random.Range(-_torqueRange, _torqueRange), Random.Range(-_torqueRange, _torqueRange), ForceMode.Impulse);
        transform.position = new Vector2(Random.Range(-4, 4), -1);
    }

   /*  private void OnMouseDown() {
        if(_gameManager.isGameActive){
            Destroy(gameObject);
            Instantiate(tapEffect, transform.position, tapEffect.transform.rotation);
            _gameManager.UpdateScore(pointValue);
        }
    } */

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
        if(!gameObject.CompareTag("Bad")){
            _gameManager.UpdateLive();
        }
    }

    public void DestroyTarget() {
        if(_gameManager.isGameActive){
            Destroy(gameObject);
            Instantiate(tapEffect, transform.position, tapEffect.transform.rotation);
            _gameManager.UpdateScore(pointValue);
        }
    }
}

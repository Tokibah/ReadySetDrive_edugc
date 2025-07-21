using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class ClickAndTrace : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;
    private bool swiping = false;

    void Awake() {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();    
    }

    void UpdateMousePosition(){
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        transform.position = mousePos;
    }

    void UpdateComponent() {
        trail.enabled = swiping;
        col.enabled = swiping;
    }

    void Update() {
        if(gameManager.isGameActive){
            if(Input.GetMouseButtonDown(0)){
                swiping = true;
                UpdateComponent();
            }else if (Input.GetMouseButtonUp(0)){
                swiping = false;
                UpdateComponent();
            }

            if(swiping){
                UpdateMousePosition();
            }
        }    
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<Target>()){
            other.gameObject.GetComponent<Target>().DestroyTarget();
        }
    }
}

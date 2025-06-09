using Unity.VisualScripting;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class interact : MonoBehaviour
{
    public Transform interactorSource;
    public float interactRange;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r,out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}

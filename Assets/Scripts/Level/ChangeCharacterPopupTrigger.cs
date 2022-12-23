using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterPopupTrigger : MonoBehaviour
{
    [SerializeField] GameObject popupContainer;
    [SerializeField] GameObject popupText;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerInput input;
    [SerializeField] HelicopterController helicopter;

    private bool isActivated = false;

    void Update()
    {
        if (isActivated && input.switchCharacterInput)
        {
            EndPopup();
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        popupContainer.SetActive(true);
        popupText.SetActive(true);
        playerMovement.DisableMovement();
        isActivated = true;
    }


    void EndPopup()
    {
        popupContainer.SetActive(false);
        popupText.SetActive(false);
        playerMovement.EnableMovement();
        helicopter.Activate();
        GameObject.Destroy(gameObject);
    }
}

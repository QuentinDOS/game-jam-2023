using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGameUi : MonoBehaviour
{
    public GameObject acornImagePrefab;
    public GameObject acornContainer;
    public GameObject gameOverPanel;

    private CharController player;

    private void Awake()
    {
        player = FindObjectOfType<CharController>();
        if (player != null)
        {
            for (int i = 0; i < acornContainer.transform.childCount; i++)
            {
                Destroy(acornContainer.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < player.currentHealth; i++)
            {
                Instantiate(acornImagePrefab, acornContainer.transform);
            }
        }
    }

    private void OnEnable()
    {
        CharController.OnHealthChanged += CharControllerOnOnHealthChanged;
        CharController.OnPlayerGameOver += CharControllerOnOnPlayerGameOver;
    }

    private void CharControllerOnOnPlayerGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    private void OnDisable()
    {
        CharController.OnHealthChanged -= CharControllerOnOnHealthChanged;
        CharController.OnPlayerGameOver -= CharControllerOnOnPlayerGameOver;

    }

    private void CharControllerOnOnHealthChanged(CharController charControl, int val)
    {
        if (charControl != null)
        {
            for (int i = 0; i < acornContainer.transform.childCount; i++)
            {
                Destroy(acornContainer.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < charControl.currentHealth; i++)
            {
                Instantiate(acornImagePrefab, acornContainer.transform);
            }

        }

    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

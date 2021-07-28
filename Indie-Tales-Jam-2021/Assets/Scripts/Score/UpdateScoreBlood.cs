using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreBlood : MonoBehaviour
{
    [SerializeField] private Text bloodScoreText;
    private int bloodScoreInt = 0;


    private void UpdateStainScore()
    {
        bloodScoreInt++;
        bloodScoreText.text = bloodScoreInt.ToString();
    }

    private void OnEnable()
    {
        PlayerController.OnStainCleaned += UpdateStainScore;
    }

    private void OnDisable()
    {
        PlayerController.OnStainCleaned -= UpdateStainScore;
    }
}

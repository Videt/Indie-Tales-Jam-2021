using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BloodSplash : MonoBehaviour
{
    [SerializeField] private int endGameDelay;
    [SerializeField] private Text textField;

    private void Start()
    {
        StartCoroutine(EndGameTimer());
    }

    IEnumerator EndGameTimer()
    {
        while (endGameDelay >= 0)
        {
            textField.text = endGameDelay.ToString();

            yield return new WaitForSeconds(1f);

            endGameDelay--;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

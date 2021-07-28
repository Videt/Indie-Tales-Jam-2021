using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreEnemy : MonoBehaviour
{
    [SerializeField] private Text enemyScoreText;
    private int enemyScoreInt = 0;

    public void UpdateEnemyScore()
    {
        enemyScoreInt++;
        enemyScoreText.text = enemyScoreInt.ToString();
    }

    private void OnEnable()
    {
        EnemyController.OnEnemyDied += UpdateEnemyScore;
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyDied -= UpdateEnemyScore;
    }
}

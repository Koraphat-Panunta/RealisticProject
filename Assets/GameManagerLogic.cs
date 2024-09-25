using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerLogic : MonoBehaviour
{
    [SerializeField] private float timeRemain;
    [SerializeField] private float targetRemain;
    [SerializeField] private CarModel carModel;
    [SerializeField] private List<Enemy> target;
    public float TarScore;
    public float Score;
    [SerializeField] private TextMeshProUGUI EndScreenText;
    [SerializeField] private TextMeshProUGUI TimeCountText;
    [SerializeField] private TextMeshProUGUI ScoreShow;
    // Start is called before the first frame update
    void Start()
    {
        EndScreenText = GameObject.Find("EndText").GetComponent<TextMeshProUGUI>();
        TimeCountText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        ScoreShow = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        timeRemain = 60;
        foreach (Enemy enemy in Resources.FindObjectsOfTypeAll(typeof(Enemy)) as Enemy[]) 
        {
            target.Add(enemy);
        }
        carModel = FindAnyObjectByType<CarModel>();
    }

    // Update is called once per frame
    void Update()
    {
        RuleLogic();
        TimeCountText.text = "Time remian " + timeRemain.ConvertTo<int>() + " Seconds";
        ScoreShow.text = "Score :" + TarScore;
        UpdateScore();
    }
    private void RuleLogic()
    {
       
        if (timeRemain <= 0)
        {
            EndGame(false);
        }
        else
        {
            if (carModel.GetHP() > 0)
            {
                for (int i = target.Count - 1; i >= 0; i--)
                {
                    if (target[i] != null && target[i].GetHP() <= 0)
                    {
                        target.RemoveAt(i);
                        TarScore += 50;
                    }
                }
                timeRemain -= Time.deltaTime;
            }
            else
            {
                EndGame(true);
            }
        }
    }
    private void UpdateScore()
    {
        if (Score < TarScore)
        {
            Score += 1;
            Score = Mathf.Clamp(Score, 0, TarScore);
        }
    }
    private void EndGame(bool IsDead)
    {
        if (IsDead == true) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            StartCoroutine(EndScene());
        }
    }
    private IEnumerator EndScene()
    {
        EndScreenText.text = "Mission Complete Score = " +TarScore;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{

    List<Challenge> challenges;
    public GameObject challengePrefab;
    public GameObject challengesPanel;
    public Button closeReadMeButton;

    private void Awake()
    {
        Debug.Log(PlayerPrefs.GetString(Utils.IS_GAME_STARTED_PLAYER_PREF, Utils.FALSE));
        challenges = new List<Challenge>();
        for(int i = 0; i < 10; i++)
        {
            Challenge challenge = new Challenge();
            challenge.challenge = "Challenge: " + i;
            challenge.reward = 10 * (i + 1);
            challenges.Add(challenge);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!challengesPanel.transform.GetChild(0).gameObject.activeSelf)
        {
            if (PlayerPrefs.GetString(Utils.IS_CHALLENGE_COMPLETED, Utils.FALSE).Equals("false"))
            {
                if(!PlayerPrefs.HasKey(Utils.START_TIME))
                {
                    GenerateNewChallenge();
                }
                else if((DateTime.Now - DateTime.Parse(PlayerPrefs.GetString(Utils.START_TIME))).Seconds < 120)
                {
                    GenerateExistedChallenge();
                    StartCoroutine(GeneratenewChallengeAfterSometime());
                }
                else
                {
                    GenerateNewChallenge();
                }
            }
            else
            {
                StartCoroutine(GenerateNewChallengeOnComplted());
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNewChallenge()
    {
        int challengeId = UnityEngine.Random.Range(0, 10);
        Challenge currentChallenge = challenges[challengeId];
        SetChallengeObject(currentChallenge);
        Debug.Log("Challenge is generated: "+currentChallenge);
        PlayerPrefs.SetString(Utils.START_TIME, DateTime.Now.ToString());
        PlayerPrefs.SetString(Utils.IS_CHALLENGE_COMPLETED, Utils.FALSE);
        PlayerPrefs.SetInt(Utils.CURRENT_CHALLENGE_ID, challengeId);
        PlayerPrefs.SetFloat(Utils.CHALLENGE_REMAINING_TIME, 120);
        StartCoroutine(GeneratenewChallengeAfterSometime());
    }

    void GenerateExistedChallenge()
    {
        Challenge currentChallenge = challenges[PlayerPrefs.GetInt(Utils.CURRENT_CHALLENGE_ID)];
        Debug.Log("Generating existed challenge");
        SetChallengeObject(currentChallenge);
    }

    void SetChallengeObject(Challenge currentChallenge)
    {
        challengePrefab.transform.GetChild(0).gameObject.GetComponent<Text>().text = currentChallenge.challenge;
        challengePrefab.transform.GetChild(1).gameObject.GetComponent<Text>().text = currentChallenge.reward.ToString();
        challengePrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Not Completed.";
        challengePrefab.GetComponent<Button>().enabled = true;
        challengePrefab.GetComponent<Button>().onClick.RemoveAllListeners();
        challengePrefab.GetComponent<Button>().onClick.AddListener(() => {
            Shop.INSTANCE.AddCoinsOnChallenge(currentChallenge.reward);
            challengePrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Completed.";
            challengePrefab.GetComponent<Button>().enabled = false;
            PlayerPrefs.SetFloat(Utils.CHALLENGE_REMAINING_TIME, 60.0f);
            PlayerPrefs.SetString(Utils.IS_CHALLENGE_COMPLETED, Utils.TRUE);
            StopCoroutine(GeneratenewChallengeAfterSometime());
            StartCoroutine(GenerateNewChallengeOnComplted());
        });
        challengePrefab.SetActive(true);
    }

    IEnumerator GeneratenewChallengeAfterSometime()
    {
        yield return new WaitForSeconds(PlayerPrefs.GetFloat(Utils.CHALLENGE_REMAINING_TIME));
        Debug.Log("old is expired, new challenge is generated: " + PlayerPrefs.GetInt(Utils.CURRENT_CHALLENGE_ID));
        GenerateNewChallenge();
    }

    IEnumerator GenerateNewChallengeOnComplted()
    {
        yield return new WaitForSeconds(PlayerPrefs.GetFloat(Utils.CHALLENGE_REMAINING_TIME));
        GenerateNewChallenge();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class QuestManager : MonoBehaviour
{
    public Quest[] activeQuests;
    public GameObject questCompletedPrefab;
    public GameObject questCompletedPrefab2;
    public TextMeshProUGUI questCompletedTextPrefab;
    public TextMeshProUGUI questCompletedTextPrefab2;
    public Quest levelQuest;
    public string levelType;
    public List<GameObject> levels = new List<GameObject>();
    private Queue<Quest> completedQuestQueue = new Queue<Quest>();
    private bool isProcessingQuestQueue = false;

    void Start()
    {
        foreach (var quest in activeQuests)
        {
            quest.Load();
        }
        levelQuest.Load();

        int unlockedLevels = (levelQuest.questIndex / 5) + 1;
        for (int i = 0; i < levels.Count; i++)
        {
            if (i < unlockedLevels && i < levels.Count)
            {
                levels[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                levels[i].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    void OnEnable()
    {
        if (levelType == "Jumping")
        {
            GameManager_Jumping.questUpdater += AddProgress;
            GameManager_Jumping.questUpdaterPlayerDied += SaveQuests;
            PlayerMovement_Jumping.questUpdater += AddProgress;
        }
        else
        {
            GameManager_BridgeLevel.questUpdater += AddProgress;
            GameManager_BridgeLevel.questUpdaterPlayerDied += SaveQuests;
            PlayerMovement_FloatingBridge.questUpdater += AddProgress;
            PlayerMovement_Ice.questUpdater += AddProgress;
        }
    }

    void OnDisable()
    {
        if (levelType == "Jumping")
        {
            GameManager_Jumping.questUpdater -= AddProgress;
            GameManager_Jumping.questUpdaterPlayerDied -= SaveQuests;
            PlayerMovement_Jumping.questUpdater -= AddProgress;
        }
        else
        {
            GameManager_BridgeLevel.questUpdater -= AddProgress;
            GameManager_BridgeLevel.questUpdaterPlayerDied -= SaveQuests;
            PlayerMovement_FloatingBridge.questUpdater -= AddProgress;
            PlayerMovement_Ice.questUpdater -= AddProgress;
        }
    }

    void AddProgress(QuestType type, int amount)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questType == type && !quest.isCompleted)
            {
                quest.UpdateProgress(amount);
                if (quest.isCompleted)
                {
                    EarnedXP();
                    switch (quest.questType)
                    {
                        case QuestType.Coins: quest.targetAmount += 4; break;
                        case QuestType.Jumps: quest.targetAmount += 5; break;
                        case QuestType.Score: quest.targetAmount += 400; break;
                        case QuestType.Slide: quest.targetAmount += 10; break;
                    }

                    quest.currentAmount = 0;
                    quest.questIndex += 1;
                    quest.Save();
                    quest.isCompleted = false;
                    completedQuestQueue.Enqueue(quest);
                    if (!isProcessingQuestQueue)
                    {
                        StartCoroutine(ProcessQuestQueue());
                    }

                //     questCompletedTextPrefab.text = quest.questTitle + " - " + quest.targetAmount.ToString();

                //     RectTransform rt = questCompletedPrefab.GetComponent<RectTransform>();
                //     Sequence s = DOTween.Sequence();
                //     s.Append(rt.DOAnchorPosY(-200f, 1f).SetEase(Ease.InOutSine));
                //     s.AppendInterval(2f);
                //     s.Append(rt.DOAnchorPosY(100f, 1f).SetEase(Ease.InOutSine));

                //     EarnedXP();
                //     switch (quest.questType)
                //     {
                //         case QuestType.Coins:
                //             quest.targetAmount += 4;
                //             break;

                //         case QuestType.Jumps:
                //             quest.targetAmount += 5;
                //             break;

                //         case QuestType.Score:
                //             quest.targetAmount += 400;
                //             break;

                //         case QuestType.Slide:
                //             quest.targetAmount += 10;
                //             break;
                //     }
                //     quest.currentAmount = 0;
                //     quest.questIndex += 1;
                //     quest.Save();
                //     quest.isCompleted = false;
                }
            }
        }
    }

    IEnumerator ProcessQuestQueue()
    {
        isProcessingQuestQueue = true;

        while (completedQuestQueue.Count > 0)
        {
            Quest quest = completedQuestQueue.Dequeue();

            int oldTarget = 0;
            switch (quest.questType)
            {
                case QuestType.Coins: oldTarget = quest.targetAmount - 4; break;
                case QuestType.Jumps: oldTarget = quest.targetAmount - 5; break;
                case QuestType.Score: oldTarget = quest.targetAmount - 400; break;
                case QuestType.Slide: oldTarget = quest.targetAmount - 10; break;
            }
            questCompletedTextPrefab.text = quest.questTitle1 + " " + oldTarget.ToString() + " " + quest.questTitle2;

            // Animate
            RectTransform rt = questCompletedPrefab.GetComponent<RectTransform>();
            Sequence s = DOTween.Sequence();
            s.Append(rt.DOAnchorPosY(-200f, 1f).SetEase(Ease.InOutSine));
            s.AppendInterval(2f);
            s.Append(rt.DOAnchorPosY(100f, 1f).SetEase(Ease.InOutSine));

            yield return s.WaitForCompletion();

            // Reward XP
            // EarnedXP();

            // // Increase Quest Target
            // switch (quest.questType)
            // {
            //     case QuestType.Coins:
            //         quest.targetAmount += 4;
            //         break;
            //     case QuestType.Jumps:
            //         quest.targetAmount += 5;
            //         break;
            //     case QuestType.Score:
            //         quest.targetAmount += 400;
            //         break;
            //     case QuestType.Slide:
            //         quest.targetAmount += 10;
            //         break;
            // }

            // quest.currentAmount = 0;
            // quest.questIndex += 1;
            // quest.Save();
            // quest.isCompleted = false;

            // Small delay between animations (optional)
            yield return new WaitForSeconds(0.5f);
        }

        isProcessingQuestQueue = false;
    }


    void SaveQuests()
    {
        foreach (var quest in activeQuests)
        {
            quest.Save();
        }
        levelQuest.Save();
    }

    public void ResetAllQuests()
    {
        foreach (var quest in activeQuests)
        {
            quest.ResetProgress();
        }
    }

    void EarnedXP()
    {
        levelQuest.UpdateProgress(10);
        int requiredXP = GetXPRequirement(levelQuest.questIndex);
        // if (levelQuest.isCompleted)
        if (levelQuest.currentAmount >= requiredXP)
        {
            questCompletedTextPrefab2.text = levelQuest.questTitle1 + " - " + levelQuest.questIndex.ToString();
            RectTransform rt = questCompletedPrefab2.GetComponent<RectTransform>();
            Sequence s = DOTween.Sequence();
            s.Append(rt.DOAnchorPosY(-400f, 1f).SetEase(Ease.InOutSine));
            s.AppendInterval(2f);
            s.Append(rt.DOAnchorPosY(100f, 1f).SetEase(Ease.InOutSine));

            levelQuest.questIndex += 1;
            levelQuest.targetAmount = GetXPRequirement(levelQuest.questIndex);
            levelQuest.currentAmount = 0;
            levelQuest.Save();
            levelQuest.isCompleted = false;

            if (levelQuest.questIndex % 5 == 0 && levelQuest.questIndex <= 25)
            {
                Debug.Log("HHIIIII");
                int a = levelQuest.questIndex / 5;
                levels[a].transform.GetChild(2).gameObject.SetActive(false);

            }
        }
    }

    int GetXPRequirement(int level)
    {
        if (level <= 5)
            return 50;
        else if (level <= 10)
            return 80;
        else if (level <= 15)
            return 100;
        else if (level <= 20)
            return 120;
        else
            return 150;
    }
}

using UnityEngine;

public enum QuestType
{
    Coins,
    Jumps,
    Score,
    Level,
    Slide
}

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questTitle1;
    public int targetAmount;
    public string questTitle2;
    public QuestType questType;
    public int originalTargetAmount;
    public int currentAmount;
    public bool isCompleted;
    public int questIndex;

    public void Save()
    {
        PlayerPrefs.SetInt(questID + "_current", currentAmount);
        PlayerPrefs.SetInt(questID + "_target", targetAmount);
        PlayerPrefs.SetInt(questID + "_index", questIndex);
        PlayerPrefs.SetInt(questID + "_completed", isCompleted ? 1 : 0);
    }

    public void Load()
    {
        currentAmount = PlayerPrefs.GetInt(questID + "_current", 0);
        targetAmount = PlayerPrefs.GetInt(questID + "_target", originalTargetAmount);
        questIndex = PlayerPrefs.GetInt(questID + "_index", 1);
        isCompleted = PlayerPrefs.GetInt(questID + "_completed", 0) == 1;
        isCompleted = false;
    }

    public void ResetProgress()
    {
        currentAmount = 0;
        isCompleted = false;
    }

    public void UpdateProgress(int amount)
    {
        if (isCompleted) return;
        currentAmount += amount;
        if (currentAmount >= targetAmount)
        {
            currentAmount = targetAmount;
            isCompleted = true;
            // Debug.Log($"Quest Completed: {questTitle}");
        }
    }
}

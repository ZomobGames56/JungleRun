using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuestUIManager : MonoBehaviour
{
    public QuestManager questManager;
    public GameObject questItemPrefab;
    public Transform questPanel;
    public Transform questPanel2;
    public TextMeshProUGUI title1;
    public TextMeshProUGUI title2;
    public TextMeshProUGUI title3;
    public TextMeshProUGUI prog1;
    public TextMeshProUGUI prog2;
    public Slider missionSlider;
    public Slider pauseSlider;
    public Quest levelQuest;

    private List<QuestUIItem> questUIItems = new List<QuestUIItem>();

    void Start()
    {
        PopulateQuestUI();
    }

    void Update()
    {
        UpdateQuestUI();
        title1.text = $"{levelQuest.questIndex}";
        title2.text = $"Lvl. {levelQuest.questIndex}";
        title3.text = $"Lvl. {levelQuest.questIndex}";
        prog1.text = $"{levelQuest.currentAmount}/{levelQuest.targetAmount}";
        prog2.text = $"{levelQuest.currentAmount}/{levelQuest.targetAmount}";
        missionSlider.maxValue = levelQuest.targetAmount;
        missionSlider.value = levelQuest.currentAmount;
        pauseSlider.maxValue = levelQuest.targetAmount;
        pauseSlider.value = levelQuest.currentAmount;
    }

    void PopulateQuestUI()
    {
        foreach (Quest quest in questManager.activeQuests)
        {
            CreateQuestUIItem(quest, questPanel);
            CreateQuestUIItem(quest, questPanel2);
        }
    }

    void CreateQuestUIItem(Quest quest, Transform panel)
    {
        GameObject item = Instantiate(questItemPrefab, panel);
        QuestUIItem uiItem = new QuestUIItem
        {
            titleText = item.transform.Find("Title").GetComponent<TextMeshProUGUI>(),
            progressText = item.transform.Find("Slider/ProgressText").GetComponent<TextMeshProUGUI>(),
            slider = item.transform.Find("Slider").GetComponent<Slider>(),
            quest = quest
        };
        uiItem.titleText.text = quest.questTitle1 + " " + quest.targetAmount + " " + quest.questTitle2;
        uiItem.slider.maxValue = quest.targetAmount;
        questUIItems.Add(uiItem);
    }

    void UpdateQuestUI()
    {
        foreach (var item in questUIItems)
        {
            item.progressText.text = $"{item.quest.currentAmount}/{item.quest.targetAmount}";
            item.slider.value = item.quest.currentAmount;
        }
    }

    private class QuestUIItem
    {
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI progressText;
        public Slider slider;
        public Quest quest;
    }
}

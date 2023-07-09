using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsWindow : MonoBehaviour {

	[Header("Settings")]
	[SerializeField] private LogsLine linePrefab;
	[SerializeField] private int maxLines = 50;

	[SerializeField] private RectTransform content;

	[Header("Content")]
	[SerializeField] private string[] rageQuitMsgs;
	[SerializeField] private string[] questFailedMsgs;
	[SerializeField] private string[] questStartedMsgs;
	[SerializeField] private string[] questSucceededMsgs;

	private static LogsWindow Instance;

	private void Awake() {
		if(Instance) {
			Debug.LogError("You put two instances of LogsWindows. Remove '" + name + "'. Problem has been mitigated.");
			enabled = false;
			return;
		}
		Instance = this;
	}

	private void NewLine(LogsEventType type, string val) {
		// create the new line
		var line = Instantiate(linePrefab);
		line.SetValues(type, val);
		line.transform.SetParent(content);

		// remove the first element if needed
		if(content.transform.childCount > maxLines) {
			Destroy(content.transform.GetChild(0).gameObject);
		}

		// update cursor position ?
	}

	private string RandomRageQuit() {
		if(rageQuitMsgs.Length == 0)
			return "";
		return rageQuitMsgs[Random.Range(0, rageQuitMsgs.Length)];
	}

	private string RandomQuestFailed() {
		if(questFailedMsgs.Length == 0)
			return "";
		return questFailedMsgs[Random.Range(0, questFailedMsgs.Length)];
	}

	private string RandomQuestStarted() {
		if(questStartedMsgs.Length == 0)
			return "";
		return questStartedMsgs[Random.Range(0, questStartedMsgs.Length)];
	}

	private string RandomQuestSuccess() {
		if(questSucceededMsgs.Length == 0)
			return "";
		return questSucceededMsgs[Random.Range(0, questSucceededMsgs.Length)];
	}

	public enum LogsEventType {
		QuestSucceeded,
		QuestFailed,
		QuestStarted,
		QuestImpossible,

		HeroDied,
		HeroLevelUp,
		
		CollectedCat,

		ExpPoint,
		ExpLevel,
		UnlockedZone,
		//


	};

	// =================================

	public static void Event_QuestSucceeded(Quest quest) {
		Instance.NewLine(
			LogsEventType.QuestSucceeded, 
			$"Quest {quest.title} has been accomplished by {quest.assignedHero.displayName}. {Instance.RandomQuestSuccess()}"
		);
	}

	public static void Event_QuestFailed(Quest quest) {
		Instance.NewLine(
			LogsEventType.QuestFailed,
			$"Quest {quest.title} has been failed... {Instance.RandomQuestFailed()}"
		);
	}

	public static void Event_QuestStarted(Quest quest) {
		Instance.NewLine(
			LogsEventType.QuestStarted,
			$"Quest {quest.title} was started by {quest.assignedHero.displayName}. {Instance.RandomQuestStarted()}"
		);
	}

	public static void Event_QuestRemovedBecauseItsImpossibleAndTooHardAndNobodyCanDoItWeAreAllDoomed(Quest quest) {
		Instance.NewLine(
			LogsEventType.QuestImpossible,
			$"Quest {quest.title} was impossible to complete : it was removed from queue. Check your items."
		);
	}

	public static void Event_HeroDied(Hero hero) {
		Instance.NewLine(
			LogsEventType.HeroDied,
			$"The player {hero.displayName} died while trying a quest of yours. {Instance.RandomRageQuit()}"
		);
	}

	public static void Event_HeroLevelup(Hero hero) {
		Instance.NewLine(
			LogsEventType.HeroLevelUp,
			$"The player {hero.displayName} leveled up. He's now level {hero.level}."
		);
	}

	public static void Event_GainExperience(int amount) {
		Instance.NewLine(
			LogsEventType.ExpPoint,
			$"You gained {amount} points of experience."
		);
	}

	public static void Event_GainLevel(int newLevel) {
		Instance.NewLine(
			LogsEventType.ExpPoint,
			$"You leveled up ! You are now level {newLevel}. GG."
		);
	}


	/// <param name="zoneLevel">The number of the zone between 1 and 6.</param>
	public static void Event_UnlockedZone(int zoneLevel) {
		Instance.NewLine(
			LogsEventType.ExpPoint,
			$"You've just unlocked the {zoneLevel}th zone."
		);
	}

	public static void Event_CollectedCat(int catNumber) {
		Instance.NewLine(
			LogsEventType.ExpPoint,
			$"Woooow ! You've obtained the {catNumber}th cat of destiny. Will you save the world ???"
		);
	}

}

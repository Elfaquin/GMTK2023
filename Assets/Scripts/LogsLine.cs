using UnityEngine;
using UnityEngine.UI;

public class LogsLine : MonoBehaviour {

    [Header("Strcture of the line")]
    [SerializeField] private Image logo;
    [SerializeField] private TMPro.TMP_Text informations;

    [Header("Logos")]
    [SerializeField] private Sprite icon_questStarted;
    [SerializeField] private Sprite icon_questFailed;
    [SerializeField] private Sprite icon_questSuccess;
    [SerializeField] private Sprite icon_questImpossible;

	[SerializeField] private Sprite icon_heroDied;
	[SerializeField] private Sprite icon_heroLvlUp;

	[SerializeField] private Sprite icon_moreExp;
	[SerializeField] private Sprite icon_moreLevel;
    [SerializeField] private Sprite icon_newZone;

	[SerializeField] private Sprite icon_lostlevel;
	[SerializeField] private Sprite icon_win;
	[SerializeField] private Sprite icon_lost;

	public void SetValues(LogsWindow.LogsEventType iconType, string text) {
		informations.text = text;
		switch(iconType) {
			case LogsWindow.LogsEventType.QuestStarted:
				logo.sprite = icon_questStarted;
				break;
			case LogsWindow.LogsEventType.QuestFailed:
				logo.sprite = icon_questFailed;
				break;
			case LogsWindow.LogsEventType.QuestSucceeded:
				logo.sprite = icon_questSuccess;
				break;
			case LogsWindow.LogsEventType.QuestImpossible:
				logo.sprite = icon_questImpossible;
				break;
			case LogsWindow.LogsEventType.HeroDied:
				logo.sprite = icon_heroDied;
				break;
			case LogsWindow.LogsEventType.HeroLevelUp:
				logo.sprite = icon_heroLvlUp;
				break;
			case LogsWindow.LogsEventType.ExpPoint:
				logo.sprite = icon_moreExp;
				break;
			case LogsWindow.LogsEventType.ExpLevel:
				logo.sprite = icon_moreLevel;
				break;
			case LogsWindow.LogsEventType.UnlockedZone:
				logo.sprite = icon_newZone;
				break;
			case LogsWindow.LogsEventType.LevelLost:
				logo.sprite = icon_lostlevel;
				break;
			case LogsWindow.LogsEventType.Win:
				logo.sprite = icon_win;
				break;
			case LogsWindow.LogsEventType.Lost:
				logo.sprite = icon_lost;
				break;
			default:
				Debug.LogError("forgot to put case for even type: " + iconType);
				break;
		}
    }



}

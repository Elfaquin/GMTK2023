using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {

    [SerializeField] private GameObject mapOverlay;
    [SerializeField] private Image mapImage;

	[Header("sprites")]
    [SerializeField] private Sprite map_1;
    [SerializeField] private Sprite map_2;
    [SerializeField] private Sprite map_3;
    [SerializeField] private Sprite map_4;
    [SerializeField] private Sprite map_5;
    [SerializeField] private Sprite map_6;

	public void ClickButton() {
		if(mapOverlay.activeInHierarchy) {
			Close_Map();
		} else {
			Open_Map();
		}
        AudioManager.PlaySoundEffect(AudioManager.SoundEffect.MapEffect);
    }

	private void Open_Map() {
		// update
		mapOverlay.SetActive(true);

		// change texture ??
		int level = GameLibrary.PlayerXpManager.currentLevel;
		if(level <= 1) {
			mapImage.sprite = map_1;
		} else if(level <= 3) {
			mapImage.sprite = map_2;
		} else if(level <= 5) {
			mapImage.sprite = map_3;
		} else if(level <= 7) {
			mapImage.sprite = map_4;
		} else if(level <= 9) {
			mapImage.sprite = map_5;
		} else {
			mapImage.sprite = map_6;
		}
	}

    public void Close_Map() {
		mapOverlay.SetActive(false);

	}

}

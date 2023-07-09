using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MuteButton : MonoBehaviour {

	[SerializeField] private bool mute = false;
	[SerializeField] private Sprite img_mute;
	[SerializeField] private Sprite img_notMute;

	private Image _image;

	private void Awake() {
		_image = GetComponent<Image>();
		_image.sprite = img_notMute;
	}

	public void Click_on_the_button() {
		mute = !mute;
		_image.sprite = mute ? img_mute : img_notMute;
		AudioManager.SetMute(mute);
	}

}

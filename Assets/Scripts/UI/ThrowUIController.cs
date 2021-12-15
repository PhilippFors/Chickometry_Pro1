using UnityEngine;
using UnityEngine.UI;
using UsefulCode.Utilities;

namespace UI
{
    public class ThrowUIController : SingletonBehaviour<ThrowUIController>
    {
        [SerializeField] private Slider slider;

        public void SetSliderValue(float currentTime, float maxTime)
        {
            slider.value = Mathf.Lerp(0, 1, (currentTime / maxTime) * 1.5f);
        }

        public void ShowSlider() => slider.gameObject.SetActive(true);

        public void HideSlider()
        {
            slider.gameObject.SetActive(false);
            slider.value = 0;
        }
    }
}
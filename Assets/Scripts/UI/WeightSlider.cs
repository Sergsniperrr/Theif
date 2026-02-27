using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WeightSlider : MonoBehaviour
{
  private Bag _bag;
  private Slider _slider;
  private Player _player;
  private float _duration = 1.5f;

  [Inject]
  private void Contrusctor(Player player) => _player = player;

  private void Awake()
  {
    _bag = GetComponentInChildren<Bag>();
    _slider = GetComponentInChildren<Slider>();
  }

  public void OnEnable() => _player.WeightIncreased += ChangeSliderValue;

  private void OnDisable() => _player.WeightIncreased -= ChangeSliderValue;

  private void ChangeSliderValue(float value)
  {
    float scale = 0.1f;
    Vector3 targetBagScale = new Vector3(_bag.transform.localScale.x + scale, _bag.transform.localScale.y + scale,
      _bag.transform.localScale.z + scale);
    _bag.transform.DOScale(targetBagScale, _duration);
    _slider.DOValue(_slider.value + value, _duration);
  }
}
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    [SerializeField] private Button _sellButton;

    private ParticleSystem _sellParticle;
    private Player _player;
    private Coroutine _jumpToPlayerRoutine;
    private Tween _jumpToPlayer;

    public event Action<int> ItemSold;

    private void OnEnable() => _sellButton.onClick.AddListener(SellItems);

    private void OnDisable() => _sellButton.onClick.RemoveListener(SellItems);

    private void Start()
    {
        _sellParticle = GetComponentInChildren<ParticleSystem>();
        _sellButton.interactable = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            _player = player;
            _sellButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            _player = null;
            _sellButton.interactable = false;
        }
    }

    private void SellItems()
    {
        if (_player != null)
        {
            if (_jumpToPlayerRoutine != null)
                StopCoroutine(_jumpToPlayerRoutine);

            _jumpToPlayerRoutine = StartCoroutine(JumpToPlayer());
        }
    }

    private IEnumerator JumpToPlayer()
    {
        Item item;
        
        while (_player.Items.Count != 0)
        {
            for (int i = 0; i < _player.Items.Count; i++)
            {
                item = _player.Items[i];
                item.gameObject.SetActive(true);
                item.DeactivateItem();

                var soldItem = item;
                
                _jumpToPlayer = item.transform.DOJump(transform.position, 1, 1, 0.5f).OnComplete(() =>
                {
                    soldItem.gameObject.SetActive(false);
                    ItemSold?.Invoke(soldItem.Index);
                    
                    _player.DeleteItem(soldItem);
                    _sellParticle.Play();
                });

                yield return _jumpToPlayer.WaitForCompletion();
            }
        }
    }
    
    
}
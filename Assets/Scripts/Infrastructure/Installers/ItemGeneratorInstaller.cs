using UnityEngine;
using Zenject;

public class ItemGeneratorInstaller : MonoInstaller
{
  [SerializeField] private ItemGenerator _itemGenerator;
  
  public override void InstallBindings()
  {
    Container.Bind<ItemGenerator>().FromInstance(_itemGenerator).AsSingle().NonLazy();
  }
}
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
  [SerializeField] private Player _player;
  [SerializeField] private DynamicJoystick _dynamicJoystick;
  [SerializeField] private PathGenerator _pathGenerator;

  public override void InstallBindings()
  {
    Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
    Container.Bind<PathGenerator>().FromInstance(_pathGenerator).AsSingle().NonLazy();
    Container.Bind<DynamicJoystick>().FromInstance(_dynamicJoystick).AsSingle().NonLazy();
  }
}
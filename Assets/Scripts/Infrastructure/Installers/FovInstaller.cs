using UnityEngine;
using Zenject;

public class FovInstaller : MonoInstaller
{
  [SerializeField] private ObjectsViewPoint _viewPointObjects;
  [SerializeField] private WallsViewPoint _viewPointWalls;

  public override void InstallBindings()
  {
    Container.Bind<ObjectsViewPoint>().FromInstance(_viewPointObjects).AsSingle().NonLazy();
    Container.Bind<WallsViewPoint>().FromInstance(_viewPointWalls).AsSingle().NonLazy();
  }
}
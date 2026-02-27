using Zenject;

public class WallVisibility : Visibility
{
  [Inject]
  private void Construct(WallsViewPoint viewPoint)
  {
    view = viewPoint.gameObject;
  }

}
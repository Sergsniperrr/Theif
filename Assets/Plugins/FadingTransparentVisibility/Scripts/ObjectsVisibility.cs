using Zenject;

public class ObjectsVisibility : Visibility
{
  [Inject]
  private void Construct(ObjectsViewPoint viewPoint)
  {
    view = viewPoint.gameObject;
  }

}
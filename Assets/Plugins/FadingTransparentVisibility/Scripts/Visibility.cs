using UnityEngine;

public class Visibility : MonoBehaviour
{
  [HideInInspector] public GameObject view;

  ViewPoint _viewScript;
  Renderer render;
  Bounds bounds;

  void Start()
  {
    render = GetComponent<MeshRenderer>();
    bounds = render.bounds;

    if (view != null)
      _viewScript = view.GetComponent<ViewPoint>();
  }

  void Update()
  {
    if (render != null && view != null)
    {
      for (int i = 0; i < render.materials.Length; i++)
      {
        if (render.materials[i].shader.name == "Visibility/VisibilityTransparentStandard")
        {
          render.materials[i].SetVector("_ViewPosition", view.transform.position);
          render.materials[i].SetVector("_Forward", view.transform.forward);

          if (render.materials[i].GetFloat("_Override") == 0)
          {
            render.materials[i].SetFloat("_CenterAlpha", _viewScript.CenterAlpha);
            render.materials[i].SetFloat("_RangeAlpha", _viewScript.RangeAlpha);
            render.materials[i].SetFloat("_LimitAlpha", _viewScript.LimitAlpha);
            render.materials[i].SetFloat("_LimitDistance", _viewScript.LimitDistance);
            render.materials[i].SetFloat("_AngleAlpha", _viewScript.AngleAlpha);
            render.materials[i].SetFloat("_VisibleDistance", _viewScript.VisibilityDistance);
            render.materials[i].SetFloat("_VisibleAngle", _viewScript.VisibilityAngle);
            render.materials[i].SetFloat("_OutlineInternal", _viewScript.OutlineInternal);
            render.materials[i].SetFloat("_OutlineExternal", _viewScript.OutlineExternal);
            render.materials[i].SetColor("_OutlineColour", _viewScript.OutlineColor);
            render.materials[i].SetFloat("_OutlineAlpha", _viewScript.OutlineAlpha);
          }

          bounds = render.bounds;
          Vector3 pointInBounds = bounds.ClosestPoint(view.transform.position);

          float distanceToPoint = Vector3.Distance(pointInBounds, view.transform.position);
          float fullDistance = render.materials[i].GetFloat("_VisibleDistance") +
                               render.materials[i].GetFloat("_LimitDistance");
          if (distanceToPoint > fullDistance)
          {
            render.materials[i].EnableKeyword("BEYOND_BOUNDS");
          }
          else
          {
            render.materials[i].DisableKeyword("BEYOND_BOUNDS");
          }

          float angle = render.materials[i].GetFloat("_VisibleAngle") / 2;
          Vector3 toCenter = bounds.center - view.transform.position;
          Vector3 coneVector = Vector3.RotateTowards(view.transform.forward, toCenter, Mathf.Deg2Rad * angle, 0);
          if (RaySphereIntersection(view.transform.position, coneVector, bounds.center, bounds.extents.magnitude))
          {
            render.materials[i].SetFloat("_VisibleAngleCosine",
              Mathf.Cos(Mathf.Deg2Rad * render.materials[i].GetFloat("_VisibleAngle") / 2));

            render.materials[i].DisableKeyword("BEYOND_ANGLE");
          }
          else
          {
            render.materials[i].EnableKeyword("BEYOND_ANGLE");
          }
        }
      }
    }
  }

  bool RaySphereIntersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius)
  {
    if (Vector3.Distance(rayOrigin, sphereCenter) < sphereRadius)
      return true;

    Vector3 m = rayOrigin - sphereCenter;
    float b = Vector3.Dot(m, rayDirection);
    float c = Vector3.Dot(m, m) - sphereRadius * sphereRadius;

    if (c > 0.0f && b > 0.0f)
      return false;

    float discr = b * b - c;
    if (discr < 0.0f)
      return false;

    return true;
  }
}
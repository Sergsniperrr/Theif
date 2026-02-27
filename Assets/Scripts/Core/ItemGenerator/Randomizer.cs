using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Randomizer
{
  private int _amount;
  private int _lastSelected;
  private List<int> _occurances;
  private bool _isFirstTry = true;

  public Randomizer(int amount)
  {
    _amount = amount;
    _lastSelected = Random.Range(0, amount - 1);
    _occurances = new List<int>();
  }

  public int Select(bool isClear = false)
  {
    int currentIndex = Random.Range(0, _amount);

    if (_isFirstTry)
    {
      int firstValue = Random.Range(0, _amount);
      _lastSelected = firstValue;
      _isFirstTry = false;
      _occurances.Add(_lastSelected);
      return _lastSelected;
    }

    while (DuplicateCheck(currentIndex))
    {
      currentIndex = Random.Range(0, _amount);
    }

    if (isClear == true)
      _occurances.Clear();

    _lastSelected = currentIndex;
    _occurances.Add(_lastSelected);
    return _lastSelected;
  }

  private bool DuplicateCheck(int current)
  {
    foreach (int index in _occurances)
    {
      if (index == current)
        return true;
    }

    return false;
  }
}
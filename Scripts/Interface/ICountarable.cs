using UnityEngine;

public interface ICountarable
{
    public bool CanBeCountered { get;}
    public void HandleCounter();
}

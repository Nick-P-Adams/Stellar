using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargeter
{
    void Register(GameObject target);
    void Unregister(GameObject target);
}

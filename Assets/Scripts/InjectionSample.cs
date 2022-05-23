using System;
using System.Collections.Generic;
using UActions.Runtime;
using UnityEngine;

[Serializable]
public class Person
{
}

[CreateAssetMenu(menuName = "Injectable/Person", fileName = "Person")]
public class InjectionSample : InjectableObject<InjectionSample>
{
    [SerializeField] private string url;
    [SerializeField] private string key;
    [SerializeField] private int number;
}
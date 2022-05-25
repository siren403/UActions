using System;
using System.Collections.Generic;
using UActions.Runtime;
using UnityEngine;

[CreateAssetMenu(menuName = "Injectable/Sample", fileName = nameof(InjectionSample))]
public class InjectionSample : InjectableObject<InjectionSample>
{
    [SerializeField] private string url;
    [SerializeField] private string key;
    [SerializeField] private int number;
}
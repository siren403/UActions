using System;
using System.Collections.Generic;
using UActions.Runtime;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Injectable/Sample", fileName = nameof(InjectionSample))]
public class InjectionSample : InjectableObject<InjectionSample>
{
    public string url;
    public string key;
    public int number;
    public BuildTarget mode;
}
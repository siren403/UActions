using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UActions.Runtime
{
    public abstract class InjectableObjectBase : ScriptableObject
    {
        public abstract void Inject(Dictionary<object, object> data);
    }

    public abstract class InjectableObject<T> : InjectableObjectBase where T : InjectableObject<T>
    {
        public override void Inject(Dictionary<object, object> data)
        {
            var type = typeof(T);

            var fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (data.TryGetValue(field.Name, out var value) &&
                    value.GetType() == field.FieldType)
                {
                    field.SetValue(this, value);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UActions.Editor.Extensions;
using YamlDotNet.Serialization;

namespace UActions.Editor
{
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogException(Exception e);
    }

    public class Logger : ILogger
    {
        public void Log(string message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
#else
            Console.WriteLine(message);
#endif
        }

        public void LogWarning(string message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#else
            Console.WriteLine(message);
#endif
        }

        public void LogException(Exception e)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogException(e);
#else
            Console.WriteLine(e);
#endif
        }
    }

    public class WorkflowActionRunner
    {
        private const string AssemblyName = "UActions.Editor";
        private readonly Dictionary<string, Type> _actionTypes;
        private readonly Type[] _registrations;

        public ILogger Logger { set; private get; }

        public WorkflowActionRunner()
        {
            _actionTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(_ => _.GetName().Name == AssemblyName ||
                            _.GetReferencedAssemblies().Select(r => r.Name).Contains(AssemblyName))
                .SelectMany(_ => { return _.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IAction))); })
                .ToDictionary(_ => _.GetActionName());

            _registrations = AppDomain.CurrentDomain.GetAssemblies()
                .Where(_ => _.GetName().Name == AssemblyName)
                .SelectMany(_ => { return _.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IRegistration))); })
                .ToArray();
        }

        public WorkflowActionRunner(Dictionary<string, Type> actions)
        {
            _actionTypes = actions;
            _registrations = Array.Empty<Type>();
        }

        public void Registration(DeserializerBuilder builder)
        {
            foreach (var type in _registrations)
            {
                var registration = Activator.CreateInstance(type) as IRegistration;
                registration?.Register(builder);
            }
        }

        public void Run(WorkflowContext context, string name, Dictionary<string, object> with = null)
        {
            context.WithData = with;

            if (string.IsNullOrEmpty(name)) return;
            if (!_actionTypes.TryGetValue(name, out var type)) return;

            try
            {
                var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);


                var withKeys = with?.Keys.ToArray() ?? Array.Empty<string>();
                var withValues = with?.Values.ToArray() ?? Array.Empty<string>();
                var withTypes = with?.Select(_ => _.Value.GetType()).ToArray() ?? Array.Empty<Type>();

                var parameters = new List<object>();
                ConstructorInfo matchedConstructor = null;
                foreach (var constructorInfo in constructors)
                {
                    var parameterInfos = constructorInfo.GetParameters();
                    parameters.Clear();

                    if ((with == null || !with.Any()) && !parameterInfos.Any())
                    {
                        matchedConstructor = constructorInfo;
                        break;
                    }

                    if (parameterInfos.Length == 1 &&parameterInfos[0].ParameterType == typeof(Dictionary<string, object>))
                    {
#if UNITY_2021_2_OR_NEWER
                        parameters.Add(with!);
#else
                        parameters.Add(with);
#endif
                    }
                    else
                    {
                        for (int i = 0; i < parameterInfos.Length; i++)
                        {
                            var info = parameterInfos[i];
                            var infoName = info.Name.PascalToKebabCase();
                            if (!info.HasDefaultValue
                                && withTypes[i] == info.ParameterType
                                && withKeys[i] == infoName)
                            {
                                parameters.Add(withValues[i]);
                            }
#if UNITY_2021_2_OR_NEWER
                            else if (with!.TryGetValue(infoName, out var value))
#else
                            else if (with.TryGetValue(infoName, out var value))
#endif
                            {
                                parameters.Add(value);
                            }
                            else
                            {
                                parameters.Add(Type.Missing);
                            }
                        }
                    }

                    matchedConstructor = constructorInfo;
                }

                if (matchedConstructor?.Invoke(parameters.ToArray()) is IAction instance)
                {
                    if (instance.Targets == TargetPlatform.All ||
                        (instance.Targets & context.CurrentTargets.TargetPlatform) > 0)
                    {
                        Logger?.Log($"[{nameof(UActions)}] run action - {name}");
                        instance.Execute(context);
                    }
                    else
                    {
                        Logger?.LogWarning($"[Action] {name} is not support {context.CurrentTargets}");
                    }
                }
            }
            catch (Exception e)
            {
                // Debug.Log($"[{name}] failed! find constructor");
                Logger?.LogException(e);
                throw;
            }
        }
    }
}
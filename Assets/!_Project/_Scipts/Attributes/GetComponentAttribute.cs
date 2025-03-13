using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Game.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentAttribute : Attribute { }

    public static class GetComponentInjector
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InjectComponents()
        {
            foreach (var mono in UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                var fields = mono.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.IsDefined(typeof(GetComponentAttribute), false));

                foreach (var field in fields)
                {
                    if (!typeof(Component).IsAssignableFrom(field.FieldType))
                    {
                        Debug.LogError($"[GetComponent] Error in {mono.gameObject.name}: Field '{field.Name}' is not a Component.");
                        continue;
                    }

                    var component = mono.GetComponent(field.FieldType);
                    if (component == null)
                    {
                        Debug.LogError($"[GetComponent] Missing {field.FieldType.Name} in {mono.gameObject.name}. Ensure the component is attached.");
                        continue;
                    }

                    field.SetValue(mono, component);
                }
            }
        }
    }
}
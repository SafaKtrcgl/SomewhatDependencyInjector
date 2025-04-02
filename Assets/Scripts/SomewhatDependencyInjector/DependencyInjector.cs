using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SomewhatDependencyInjector
{
    public class DependencyInjector
    {
        private const BindingFlags MemberSearchFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        private readonly Dictionary<Type, object> _dependencies;

        public DependencyInjector()
        {
            _dependencies = new Dictionary<Type, object>();
        }

        public void Register<T>(T instance)
        {
            _dependencies.Add(typeof(T), instance);
        }

        public void InjectDependencies(object target)
        {
            if (target == null)
                return;

            var targetType = target.GetType();
            var members = new List<MemberInfo>();

            members.AddRange(targetType.GetFields(MemberSearchFlags)
                .Where(x => !x.FieldType.IsPrimitive));

            members.AddRange(targetType.GetProperties(MemberSearchFlags)
                .Where(x => !x.PropertyType.IsPrimitive && x.GetIndexParameters().Length == 0));

            foreach (var member in members)
            {
                Type memberType = null;

                switch (member)
                {
                    case FieldInfo field:
                        memberType = field.FieldType;
                        break;
                    case PropertyInfo property:
                        memberType = property.PropertyType;
                        break;
                }

                if (memberType == null)
                    continue;

                if (member.GetCustomAttribute<InjectAttribute>() != null)
                {
                    if (_dependencies.TryGetValue(memberType, out var dependency))
                    {
                        if (member is FieldInfo field)
                            field.SetValue(target, dependency);
                        else if (member is PropertyInfo property)
                            property.SetValue(target, dependency);
                        
                        Debug.Log($"{member.Name} has been injected for {targetType}");
                    }
                    else
                    {
                        Debug.LogWarning($"Attempt to inject type {memberType.Name} without registration.");
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DependencyInjector
{
    private const BindingFlags MemberSearchFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;// | BindingFlags.DeclaredOnly;
    private readonly Dictionary<Type, object> _dependencies;
    private readonly Dictionary<Type, List<MemberInfo>> _membersToInject;

    public DependencyInjector()
    {
        _dependencies = new Dictionary<Type, object>();
        _membersToInject = new Dictionary<Type, List<MemberInfo>>();
    }

    public void Register<T>(T instance)
    {
        _dependencies.Add(typeof(T), instance);
    }

    public void InjectDependencies(object target)
    {
        InjectDependencies(target, new HashSet<Type>());
    }

    private void InjectDependencies(object target, HashSet<Type> injectedObjectTypes)
    {
        if (target == null)
            return;

        var targetType = target.GetType();
        if (!injectedObjectTypes.Add(targetType))
            return;

        if (!_membersToInject.TryGetValue(targetType, out var members))
        {
            members = new List<MemberInfo>();

            members.AddRange(targetType.GetFields(MemberSearchFlags)
                .Where(x => !x.GetType().IsPrimitive));

            members.AddRange(targetType.GetProperties(MemberSearchFlags)
                .Where(x => !x.GetType().IsPrimitive && x.GetIndexParameters().Length == 0));

            _membersToInject[targetType] = members;
        }

        foreach (var member in members)
        {
            Type memberType = null;
            object memberInstance = null;

            switch (member)
            {
                case FieldInfo field:
                    memberType = field.FieldType;
                    memberInstance = field.GetValue(target);
                    break;
                case PropertyInfo property:
                    memberType = property.PropertyType;
                    memberInstance = property.GetValue(target);
                    break;
            }

            if (memberType == null)
                continue;

            if (member.GetCustomAttribute<InjectAttribute>() == null)
            {
                if (memberInstance != null)
                {
                    InjectDependencies(memberInstance, injectedObjectTypes);
                }
            }
            else
            {
                if (_dependencies.TryGetValue(memberType, out var dependency))
                {
                    if (member is FieldInfo field)
                        field.SetValue(target, dependency);
                    else if (member is PropertyInfo property)
                        property.SetValue(target, dependency);

                    InjectDependencies(dependency, injectedObjectTypes);
                }
                else
                {
                    Debug.LogWarning($"Attempt to inject type {memberType.Name} without registration.");
                }
            }
        }
    }
}

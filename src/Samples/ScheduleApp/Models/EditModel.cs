using System.Linq.Expressions;
using System.Reflection;

namespace ScheduleApp.Models;

public abstract class EditModel<T>
{
    private readonly Dictionary<MemberInfo, object?> _changes = new();
    private readonly object _changesLock = new();
    private readonly object _modelLock = new();
    private T? _model;
    
    public T? Model
    {
        get => _model;
        set
        {
            lock (_modelLock)
            {
                _model = value;
            }
        }
    }

    public System.Collections.Comparer Comparer { get; set; } = System.Collections.Comparer.Default;

    public void ApplyChanges()
    {
        lock (_changesLock)
        {
            lock (_modelLock)
            {
                foreach (var (member, value) in _changes)
                {
                    if (member is FieldInfo field)
                        field.SetValue(Model, value);

                    else if (member is PropertyInfo property)
                        property.SetValue(Model, value);
                }

                _changes.Clear();
            }
        }
    }

    public bool HasChanges()
    {
        lock (_changesLock)
        {
            lock (_modelLock)
            {
                foreach (var (member, value) in _changes)
                    if (Comparer.Compare(GetModelValue(member), value) != 0)
                        return true;

                return false;
            }
        }
    }

    protected void Set<TValue>(Expression<Func<T, TValue>> member, TValue value)
    {
        lock (_changesLock)
        {
            _changes[GetSupportedMember(member)] = value;
        }
    }

    protected TValue Get<TValue>(Expression<Func<T, TValue>> member)
    {
        lock (_changesLock)
        {
            var id = GetSupportedMember(member);
            if (_changes.TryGetValue(id, out var value))
                return (TValue?)value!;

            return (TValue?)GetModelValue(id)!;
        }
    }

    private static MemberInfo GetSupportedMember<TValue>(Expression<Func<T, TValue>> member)
    {
        if (member.Body is not MemberExpression memberExpression)
            throw new NotSupportedException();

        if (memberExpression.Member is not (FieldInfo or PropertyInfo))
            throw new NotSupportedException();

        return memberExpression.Member;
    }

    private object? GetModelValue(MemberInfo member)
    {
        if (Model is null)
            return null;

        if (member is FieldInfo field)
            return field.GetValue(Model);

        if (member is PropertyInfo property)
            return property.GetValue(Model);

        throw new NotSupportedException();
    }
}

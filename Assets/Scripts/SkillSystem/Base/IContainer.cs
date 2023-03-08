using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContainer
{
    /// <summary>
    /// add a aspect with singleton key and type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T AddAspect<T>(string key = null) where T : IAspect, new();
    T AddAspect<T>(T aspect, string key = null) where T : IAspect;

    /// <summary>
    /// get a existing aspect
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T GetAspect<T>(string key = null) where T : IAspect;

    /// <summary>
    /// get all aspects
    /// </summary>
    /// <returns></returns>
    ICollection<IAspect> Aspects();

}

public class Container : IContainer
{
    Dictionary<string, IAspect> aspects = new Dictionary<string, IAspect>();

    public T AddAspect<T>(string key = null) where T : IAspect, new()
    {
        return AddAspect<T>(new T(), key);
    }

    public T AddAspect<T>(T aspect, string key = null) where T : IAspect
    {
        key = key ?? typeof(T).Name;
        aspects.Add(key, aspect);
        aspect.container = this;
        return aspect;

    }

    public ICollection<IAspect> Aspects()
    {
        return aspects.Values;
    }

    public T GetAspect<T>(string key = null) where T : IAspect
    {
        key = key ?? typeof(T).Name;
        T aspect = aspects.ContainsKey(key) ? (T)aspects[key] : default(T);
        return aspect;
    }
}

public interface IAspect
{
    IContainer container { get; set; }
}

public class Aspect : IAspect
{
    public IContainer container { get ; set ; }
}


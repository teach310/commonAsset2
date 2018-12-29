using System;
using UnityEngine;

[System.AttributeUsage (System.AttributeTargets.Field,Inherited = true, AllowMultiple = false)]
public class ComponentRestrictionAttribute : PropertyAttribute
{
    public Type type;
    public ComponentRestrictionAttribute(Type type){
        this.type = type;
    }
}


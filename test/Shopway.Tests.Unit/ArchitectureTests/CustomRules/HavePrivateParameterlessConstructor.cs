﻿using Mono.Cecil;
using Mono.Cecil.Rocks;
using NetArchTest.Rules;
using System.Reflection;

namespace Shopway.Tests.Unit.ArchitectureTests.CustomRules;

public sealed class HavePrivateParameterlessConstructor : ICustomRule
{
    private readonly Func<TypeDefinition, bool> _test;

    public HavePrivateParameterlessConstructor()
    {
        _test = typeDefinition => typeDefinition
            .GetConstructors()
            .Any(c => c.IsPrivate && c.HasParameters is false);
    }

    public bool MeetsRule(TypeDefinition type)
    {
        return _test(type);
    }
}
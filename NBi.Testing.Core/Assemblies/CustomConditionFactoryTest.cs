using Moq;
using NBi.Extensibility.Resolving;
using NBi.Core.Assemblies.Decoration;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using NBi.Extensibility.Decoration;
using Resource;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Assemblies;

public class CustomConditionFactoryTest
{
    [Test]
    public void Instantiate_WithoutCtorParameter_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var instance = factory.Instantiate
        (
            typeof(CustomConditionWithoutParameter),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>())
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());
    }

    [Test]
    public void Instantiate_WithoutCtorOneParameter_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var instance = factory.Instantiate
        (
            typeof(CustomConditionWithOneParameter),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>() { { "name", "myName" } })
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());
    }

    [Test]
    public void Instantiate_WithoutCtorTwoParameters_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var instance = factory.Instantiate
        (
            typeof(CustomConditionWithTwoParameters),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>()
            {
                { "name", "myName" },
                { "count", 5 },
            })
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());
    }

    [Test]
    public void Instantiate_WithoutCtorTwoParametersReversed_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var instance = factory.Instantiate
        (
            typeof(CustomConditionWithTwoParameters),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>()
            {
                { "count", "5" },
                { "name", "myName" },
            })
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());
    }

    [Test]
    public void Instantiate_WithoutCtorTwoParametersIncorrectCase_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var instance = factory.Instantiate
        (
            typeof(CustomConditionWithTwoParameters),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>()
            {
                { "Count", "5" },
                { "naME", "myName" },
            })
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());
    }

    [Test]
    public void Instantiate_MultipleConstructors_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var instance = factory.Instantiate
        (
            typeof(CustomConditionWithMulipleCtors),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>()
            {
                { "count", "5" },
                { "name", "myName" },
            })
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());

        instance = factory.Instantiate
        (
            typeof(CustomConditionWithMulipleCtors),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>()
            {
                { "name", "myName" },
            })
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());

        instance = factory.Instantiate
        (
            typeof(CustomConditionWithMulipleCtors),
            new ReadOnlyDictionary<string, object?>(new Dictionary<string, object?>()
            { })
        );
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.AssignableTo<ICustomCondition>());
    }

    [Test]
    public void GetType_ExistingTypeName_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var type = factory.GetType
        (
            Assembly.GetExecutingAssembly()
            , typeof(CustomConditionWithMulipleCtors).Name
        );
        Assert.That(type, Is.EqualTo(typeof(CustomConditionWithMulipleCtors)));
    }

    [Test]
    public void GetType_ExistingTypeFullName_Instantiated()
    {
        var factory = new CustomConditionFactory();
        var type = factory.GetType
        (
            Assembly.GetExecutingAssembly()
            , typeof(CustomConditionWithMulipleCtors).FullName ?? throw new NullReferenceException()
        );
        Assert.That(type, Is.EqualTo(typeof(CustomConditionWithMulipleCtors)));
    }

    private class CustomConditionFactoryProxy : CustomConditionFactory
    {
        protected internal override Assembly GetAssembly(string path) => Assembly.GetExecutingAssembly();
    }

    [Test]
    public void Instantiate_NotExistingType_NotInstantiated()
    {
        var factory = new CustomConditionFactoryProxy();
        void instantiate() => factory.Instantiate
        (
            Mock.Of<ICustomConditionArgs>(x =>
                x.AssemblyPath == new LiteralScalarResolver<string>(".") &&
                x.TypeName == new LiteralScalarResolver<string>("NotExistingType") &&
                x.Parameters == null
            )
        );
        Assert.Throws<NBiException>(instantiate);
    }

    [Test]
    public void Instantiate_NotExistingNamespaceType_NotInstantiated()
    {
        var factory = new CustomConditionFactoryProxy();
        void instantiate() => factory.Instantiate
        (
            Mock.Of<ICustomConditionArgs>(x =>
                x.AssemblyPath == new LiteralScalarResolver<string>(".") &&
                x.TypeName == new LiteralScalarResolver<string>("Namespace.NotExistingType") &&
                x.Parameters == null
            )
        );
        Assert.Throws<NBiException>(instantiate);
    }

    [Test]
    public void Instantiate_NotImplementingInterface_NotInstantiated()
    {
        var factory = new CustomConditionFactoryProxy();
        void instantiate() => factory.Instantiate
        (
            Mock.Of<ICustomConditionArgs>(x =>
                x.AssemblyPath == new LiteralScalarResolver<string>(".") &&
                x.TypeName == new LiteralScalarResolver<string>(this.GetType().Name) &&
                x.Parameters == null
            )
        );
        Assert.Throws<NBiException>(instantiate);
    }

    [Test]
    public void Instantiate_ConstructorNotFound_NotInstantiated()
    {
        var factory = new CustomConditionFactoryProxy();
        void instantiate() => factory.Instantiate
        (
            Mock.Of<ICustomConditionArgs>(x =>
                x.AssemblyPath == new LiteralScalarResolver<string>(".") &&
                x.TypeName == new LiteralScalarResolver<string>(typeof(CustomConditionWithMulipleCtors).Name) &&
                x.Parameters == new ReadOnlyDictionary<string, IScalarResolver>(new Dictionary<string, IScalarResolver>() {
                    { "NotExistingParameter", new LiteralScalarResolver<string>("null") }
                })
            )
        );
        Assert.Throws<NBiException>(instantiate);
    }
}

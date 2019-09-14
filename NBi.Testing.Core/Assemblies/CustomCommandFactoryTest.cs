using Moq;
using NBi.Core;
using NBi.Core.Assemblies;
using NBi.Core.Assemblies.Decoration;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using NBi.Extensibility.Decoration;
using NBi.Testing.Core.Assemblies.Resource;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Assemblies
{
    public class CustomCommandFactoryTest
    {
        [Test]
        public void Instantiate_WithoutCtorParameter_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var instance = factory.Instantiate
            (
                typeof(CustomCommandWithoutParameter),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>())
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());
        }

        [Test]
        public void Instantiate_WithoutCtorOneParameter_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var instance = factory.Instantiate
            (
                typeof(CustomCommandWithOneParameter),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>() { { "name", "myName" } })
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());
        }

        [Test]
        public void Instantiate_WithoutCtorTwoParameters_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var instance = factory.Instantiate
            (
                typeof(CustomCommandWithTwoParameters),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
                {
                    { "name", "myName" },
                    { "count", 5 },
                })
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());
        }

        [Test]
        public void Instantiate_WithoutCtorTwoParametersReversed_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var instance = factory.Instantiate
            (
                typeof(CustomCommandWithTwoParameters),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
                {
                    { "count", "5" },
                    { "name", "myName" },
                })
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());
        }

        [Test]
        public void Instantiate_WithoutCtorTwoParametersIncorrectCase_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var instance = factory.Instantiate
            (
                typeof(CustomCommandWithTwoParameters),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
                {
                    { "Count", "5" },
                    { "naME", "myName" },
                })
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());
        }

        [Test]
        public void Instantiate_MultipleConstructors_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var instance = factory.Instantiate
            (
                typeof(CustomCommandWithMulipleCtors),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
                {
                    { "count", "5" },
                    { "name", "myName" },
                })
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());

            instance = factory.Instantiate
            (
                typeof(CustomCommandWithMulipleCtors),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
                {
                    { "name", "myName" },
                })
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());

            instance = factory.Instantiate
            (
                typeof(CustomCommandWithMulipleCtors),
                new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
                { })
            );
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.AssignableTo<ICustomCommand>());
        }

        [Test]
        public void GetType_ExistingTypeName_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var type = factory.GetType
            (
                Assembly.GetExecutingAssembly()
                , typeof(CustomCommandWithMulipleCtors).Name
            );
            Assert.That(type, Is.EqualTo(typeof(CustomCommandWithMulipleCtors)));
        }

        [Test]
        public void GetType_ExistingTypeFullName_Instantiated()
        {
            var factory = new CustomCommandFactory();
            var type = factory.GetType
            (
                Assembly.GetExecutingAssembly()
                , typeof(CustomCommandWithMulipleCtors).FullName
            );
            Assert.That(type, Is.EqualTo(typeof(CustomCommandWithMulipleCtors)));
        }

        private class CustomCommandFactoryProxy : CustomCommandFactory
        {
            protected internal override Assembly GetAssembly(string path) => Assembly.GetExecutingAssembly();
        }

        [Test]
        public void Instantiate_NotExistingType_NotInstantiated()
        {
            var factory = new CustomCommandFactoryProxy();
            void instantiate() => factory.Instantiate
            (
                Mock.Of<ICustomCommandArgs>(x =>
                    x.AssemblyPath==new LiteralScalarResolver<string>(".") &&
                    x.TypeName == new LiteralScalarResolver<string>("NotExistingType") &&
                    x.Parameters == null
                )
            );
            Assert.Throws<NBiException>(instantiate);
        }

        [Test]
        public void Instantiate_NotExistingNamespaceType_NotInstantiated()
        {
            var factory = new CustomCommandFactoryProxy();
            void instantiate() => factory.Instantiate
            (
                Mock.Of<ICustomCommandArgs>(x =>
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
            var factory = new CustomCommandFactoryProxy();
            void instantiate() => factory.Instantiate
            (
                Mock.Of<ICustomCommandArgs>(x =>
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
            var factory = new CustomCommandFactoryProxy();
            void instantiate() => factory.Instantiate
            (
                Mock.Of<ICustomCommandArgs>(x =>
                    x.AssemblyPath == new LiteralScalarResolver<string>(".") &&
                    x.TypeName == new LiteralScalarResolver<string>(typeof(CustomCommandWithMulipleCtors).Name) &&
                    x.Parameters == new ReadOnlyDictionary<string, IScalarResolver>(new Dictionary<string, IScalarResolver>() {
                        { "NotExistingParameter", new LiteralScalarResolver<string>("foo") }
                    })
                )
            );
            Assert.Throws<NBiException>(instantiate);
        }
    }
}
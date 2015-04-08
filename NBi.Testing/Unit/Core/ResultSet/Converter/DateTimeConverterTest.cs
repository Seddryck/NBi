﻿using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;
using NBi.Core.ResultSet.Converter;

namespace NBi.Testing.Unit.Core.ResultSet.Converter
{
    [TestFixture]
    public class DateTimeConverterTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        //Valid dates
        [Test]
        public void IsValidDateTime_xxxxyyyy_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("10/10/2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmmyyyy_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("16/10/2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_mmddyyyy_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("10/16/2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmmyy_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("16/10/13"), Is.True);
        }

        [Test]
        public void IsValidDateTime_mmddyy_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("10/16/13"), Is.True);
        }

        [Test]
        public void IsValidDateTime_dmyy_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("5.12.78"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmyy_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("10.5.2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_yyyymmdd_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("2013-10-16"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmm_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("16.12"), Is.True);
        }

        [Test]
        public void IsValidDateTime_mmdd_True()
        {
            
            Assert.That(new DateTimeConverter().IsValid("12/17"), Is.True);
        }

        [Test]
        public void IsValidDateTime_String_False()
        {
            
            Assert.That(new DateTimeConverter().IsValid("DateTime"), Is.False);
        }
    }
}

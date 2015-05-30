using System.Collections.Generic;
using NUnit.Framework;
using NBi.Core.Structure;
using NBi.Core.Structure.Olap.Builders;

namespace NBi.Testing.Unit.Core.Structure.Olap.Builders
{
    [TestFixture]
    public class SetDiscoveryCommandTest
    {
        [Test]
        public void GetCommandText_FieldsContainDisplayFolder_DisplayFolderIsNotInCommandFilter()
        {
            var filters = new CaptionFilter[]
            {
                new CaptionFilter(Target.Perspectives, "my perspective")
                , new CaptionFilter(Target.DisplayFolders, "my display-folder")
            };

            var builder = new SetDiscoveryCommandBuilder();
            builder.Build(filters);
            var commandText = builder.GetCommandText().ToLower();
            var fieldsText = commandText.Substring(0,commandText.IndexOf(" from "));

            Assert.That(fieldsText, Is.StringContaining("display").And.StringContaining("folder"));

        }
        
        [Test]
        public void GetCommandText_FiltersContainDisplayFolder_DisplayFolderIsNotInCommandFilter()
        {
            var filters = new CaptionFilter[]
            {
                new CaptionFilter(Target.Perspectives, "my perspective")
                , new CaptionFilter(Target.DisplayFolders, "my display-folder")
            };

            var builder = new SetDiscoveryCommandBuilder();
            builder.Build(filters);
            var commandText = builder.GetCommandText().ToLower();
            var filtersText = commandText.Substring(commandText.IndexOf(" from "));

            Assert.That(filtersText, Is.Not.StringContaining("display").And.Not.StringContaining("folder"));

        }

        [Test]
        public void GetCommandText_FiltersContainDisplayFolder_CommandFiltersDoesNotContainDoubleAnd()
        {
            var filters = new CaptionFilter[]
            {
                new CaptionFilter(Target.Perspectives, "my perspective")
                , new CaptionFilter(Target.DisplayFolders, "my display-folder")
            };

            var builder = new SetDiscoveryCommandBuilder();
            builder.Build(filters);
            var commandText = builder.GetCommandText();

            Assert.That(commandText.Replace(" ", "").ToLower(), Is.Not.StringContaining("andand"));
        }

        [Test]
        public void GetPostFilters_FiltersContainDisplayFolder_PostCommandFilterIsNotEmpty()
        {
            var filters = new CaptionFilter[]
            {
                new CaptionFilter(Target.Perspectives, "my perspective")
                , new CaptionFilter(Target.DisplayFolders, "my display-folder")
            };

            var builder = new SetDiscoveryCommandBuilder();
            builder.Build(filters);
            var postFilters = builder.GetPostFilters();

            Assert.That(postFilters, Is.Not.Null.And.Not.Empty);
        }

    }
}

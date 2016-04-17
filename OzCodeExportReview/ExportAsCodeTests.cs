using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OzCodeExportReview.SampleClasses;
using Xunit;

namespace OzCodeExportReview
{
    public class ExportAsCodeTests
    {
        [Fact]
        public void ExportsWithDifferentPropertyTypes()
        {
            var objectToExport = new DifferentExportableItems("first sample object")
            {
                EnumProperty = SampleEnum.SecondOption,
                DateTimePropertyExpectingHighPrecision = new DateTime(2000, 12, 31, 23, 59, 59, 987),
                IntegerProperty = 42,
                InternalStringProperty = "this string is internal, how is it exported?",
                NullableIntegerProperty = 23,
                PublicStringField = "a field is not a property. probably bad style, but perfectly valid code"
            };

            Assert.Equal(23, objectToExport.NullableIntegerProperty);
            // internal properties are always included, non-optional.
            // properties with private setter are missing in export, even when defined in constructor
            // bool properties are exported as string "true"/"false", not as constant true/false.

            // checkboxes in export-dialog are inverted: known issue according to tweet response, fixed in next release.
        }

        [Fact]
        public void ExportNullableNull()
        {
            var objectToExport = new DifferentExportableItems("first sample object")
            {
                NullableIntegerProperty = null
            };

            Assert.Equal(23, objectToExport.NullableIntegerProperty);
            // a nullable property is not included when it is null.
        }

        [Fact]
        public void ExportDateTimeWithRoundValue()
        {
            var objectToExport = new DateTime(2001, 1, 20, 20, 15, 03);
            
            Assert.Equal(0, objectToExport.Millisecond);
            // even with round values up to the second the ticks-constructor is used - correct but hard to read the code.
        }

        [Fact]
        public void ExportsWithAsymetricSetterAndGetter()
        {
            var objectToExport = new AsymmetricalSetterAndGetter()
            {
                TheNumber = 1773,
                UseFallbackValue = true
            };
            Assert.Equal(1773, objectToExport.TheNumber);

            objectToExport.UseFallbackValue = true;
            Assert.Equal(objectToExport.FallbackValue, objectToExport.TheNumber);

            // set's the wrong value(s) in generated code when getter does not always return the backing field.
                // as properties can overwrite fields, but not vice versa, probably export ordered by visibility (most visible first) might help heuristically.
        }

        [Fact]
        public void ExportsWithEqualObjects()
        {
            var mother = new Woman() { Name = "Anna" };
            var bert1 = new Man() {Name = "Bert", Mother = mother };
            var bert2 = new Man() {Name = "Bert", Mother = mother };
            mother.Children.Add(bert1);
            mother.Children.Add(bert2);
            
            Assert.Equal(bert2, bert1);

            //identical objects are exported as different ones (break at Assertion and export to depth 5)
            //"bi-directional properties" may be incomplete
        }

        [Fact]
        public void ExportsWithIdenticalObjects()
        {
            var mother = new Woman() { Name = "Anna" };
            var bert1 = new Man() { Name = "Bert", Mother = mother };
            mother.Children.Add(bert1);
            mother.Children.Add(bert1);

            Assert.Equal(2, mother.Children.Count);
        }

        [Fact]
        public void ExportsWithCycles()
        {
            throw new NotImplementedException("not yet relevant as the export feature seems not to support referenceequality considerations at all yet.");
        }

        private Man SetupFredFlintStonesFamily()
        {
            var fred = new Man() {Name = "Fred Flintstone"};
            var wilma = new Woman() {Name = "Wilma Flintstone"};
            var pebbles = new Woman()
            {
                Name = "Pebbles Flintstone",
                Mother = wilma,
                Father = fred
            };
            fred.Children.Add(pebbles);
            wilma.Children.Add(pebbles);

            var chip = new Man()
            {
                Name = "Chip",
                Mother = pebbles
            };
            var roxy = new Woman()
            {
                Name = "Roxy",
                Mother = pebbles
            };
            pebbles.Children.Add(chip);
            pebbles.Children.Add(roxy);

            return fred;
        }
    }
}

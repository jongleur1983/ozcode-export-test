using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeExportReview.SampleClasses
{
    public class DifferentExportableItems
    {
        public DifferentExportableItems(string name)
        {
            this.NameWithPrivateSetter = name;
        }

        public int IntegerProperty { get; set; }

        public DateTime DateTimePropertyExpectingHighPrecision { get; set; }

        public int? NullableIntegerProperty { get; set; }

        public string PublicStringField;

        internal string InternalStringProperty { get; set; }

        public SampleEnum EnumProperty { get; set; }

        public string NameWithPrivateSetter { get; private set; }

        public bool BooleanProperty { get; set; }
    }
}

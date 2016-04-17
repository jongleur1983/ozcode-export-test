using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzCodeExportReview.SampleClasses
{
    class AsymmetricalSetterAndGetter
    {
        private int theNumber;

        public bool UseFallbackValue { get; set; }

        public int FallbackValue => -1;

        public int TheNumber
        {
            get
            {
                if (UseFallbackValue)
                {
                    return this.FallbackValue;
                }
                else
                {
                    return theNumber;
                }
            }
            set
            {
                this.theNumber = value;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace OzCodeExportReview.SampleClasses
{
    public abstract class Person
    {
        public Person()
        {
            this.Children = new List<Person>();
        }

        public string Name { get; set; }

        public Woman Mother { get; set; }

        public Man Father { get; set; }

        public List<Person> Children { get; set; }

        public IEnumerable<Person> Siblings =>
            this.Mother.Children
                .Union(this.Father.Children)
                .Where(person => !ReferenceEquals(person, this));

        public IEnumerable<Woman> Aunts =>
            this.Mother.Siblings
            .Union(this.Father.Siblings)
                .Distinct()
                .OfType<Woman>()
                .Where(person => !ReferenceEquals(person, this));

        public IEnumerable<Man> Uncles =>
            this.Mother.Siblings
            .Union(this.Father.Siblings)
                .Distinct()
                .OfType<Man>()
                .Where(person => !ReferenceEquals(person, this));

        public IEnumerable<Person> Descendants =>
            this.Children
                .Union(this.Children.SelectMany(c => c.Children));

        public abstract Gender Gender { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person) obj);
        }

        protected bool Equals(Person other)
        {
            return string.Equals(Name, other.Name) && 
                Equals(Mother, other.Mother) && 
                Equals(Father, other.Father) && 
                Children.SequenceEqual(other.Children);
        }
    }
}

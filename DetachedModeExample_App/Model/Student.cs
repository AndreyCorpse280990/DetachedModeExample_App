using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetachedModeExample_App.Model
{
    // Student - класс, описывающий сущность студента
    internal class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Rate { get; set; }
        public decimal? Grants { get; set; }

        public Student()
        {
            LastName = string.Empty;
            FirstName = string.Empty;
        }

        public Student(Student other) : this(other.Id, other.LastName, other.FirstName, other.Rate, other.Grants) { }

        public Student(string lastName, string firstName, int rate, decimal? grants)
        {
            LastName = lastName;
            FirstName = firstName;
            Rate = rate;
            Grants = grants;
        }
        public Student(int id, string lastName, string firstName, int rate, decimal? grants) :
            this(lastName, firstName, rate, grants)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id} - {LastName} - {FirstName} - {Rate} - {Grants}";
        }
    }
}

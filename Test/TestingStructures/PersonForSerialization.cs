using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archiver;

namespace Test
{
    public enum TGender
    {
        Male,
        Female
    }

    public class TPersonChildren
    {
        public String Name { get; set; }
        public Int32 Age { get; set; }
        public TGender Gender { get; set; }

        public TPersonChildren()
        {
            
        }
    }

    public class TEmployeeInfo
    {
        public String CompanyName { get; set; }
        public Int32 EmployeeId { get; set; }
        public String Position { get; set; }

        public TEmployeeInfo()
        {
            
        } 
    }

    public class TPersonForSerialization
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Int32 Age { get; set; }
        public Int32 Weight { get; set; }
        public Int32 Height { get; set; }

        public TEmployeeInfo EmployeeInfo { get; set; }
        public List<TPersonChildren> PersonChildrens { get; set; } = new List<TPersonChildren>();
       
        public TPersonForSerialization()
        {
            
        }


        public static void Serialize(TPersonForSerialization person, CDomContainer container)
        {
            container.Set("FirstName", person.FirstName);
            container.Set("LastName", person.LastName);
            container.Set("Age", person.Age);
            container.Set("Weight", person.Weight);
            container.Set("Height", person.Height);

            CDomSection section = container.CreateSection("Employee");
            section.Set("Company", person.EmployeeInfo.CompanyName);
            section.Set("EmployeeId", person.EmployeeInfo.EmployeeId);
            section.Set("Position", person.EmployeeInfo.Position);

            CDomListSection listSection = container.CreateListSection("children");

            foreach (TPersonChildren child in person.PersonChildrens)
            {
                CDomSection childSection = listSection.CreateSection();
                childSection.Set("Name", child.Name);
                childSection.Set("Age", child.Age);
                childSection.Set("Gender", (Int32)child.Gender);
            }
        }

        public static TPersonForSerialization Deserialize(CDomContainer container)
        {
            TPersonForSerialization person = new TPersonForSerialization();

            person.FirstName = container.GetString("FirstName");
            person.LastName = container.GetString("LastName");
            person.Age = container.GetInt32("Age");
            person.Weight = container.GetInt32("Weight");
            person.Height = container.GetInt32("Height");

            CDomSection employeeSection = container.GetSection("Employee");
            person.EmployeeInfo = new TEmployeeInfo()
            {
                CompanyName = employeeSection.GetString("Company"),
                Position = employeeSection.GetString("Position"),
                EmployeeId = employeeSection.GetInt32("EmployeeId")
            };
            
            return person;
        }
    }
}

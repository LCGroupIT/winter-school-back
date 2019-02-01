using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService
{
    public class UIPassportData
    {
        private readonly int series;
        private readonly long number;
        private readonly string placeIssue;
        private readonly DateTime dateIssue;
        private readonly string departmentCode;
        private readonly string lastName;
        private readonly string firstName;
        private readonly string secondName;
        private readonly string sex;
        private readonly DateTime birthDate;
        private readonly string birthPlace;

        public UIPassportData(Passport dataBasePassport)
        {
            series = int.Parse(dataBasePassport.Series);
            number = long.Parse(dataBasePassport.Number);
            placeIssue = dataBasePassport.IssuedBy;
            dateIssue = dataBasePassport.IssuedOn;
            departmentCode = dataBasePassport.IssuedDepartment;
            lastName = dataBasePassport.Lastname;
            firstName = dataBasePassport.Firstname;
            secondName = dataBasePassport.Secondname;
            sex = SexTypeToString(dataBasePassport.Sex);
            birthDate = dataBasePassport.Birthday;
            birthPlace = dataBasePassport.Address;
        }

        public Passport ParseToDatabasePassport()
        {
            return new Passport
            {
                Series = series.ToString(),
                Number = number.ToString(),
                IssuedBy = placeIssue,
                IssuedOn = dateIssue,
                IssuedDepartment = departmentCode,
                Lastname = lastName,
                Firstname = firstName,
                Secondname = secondName,
                Sex = StringTosexType(sex),
                Birthday = birthDate,
                Address = birthPlace
            };
        }
        
        private static string SexTypeToString (SexType sextype)
        => sextype == SexType.Male ? "male" : "female";

        private static SexType StringTosexType (string sex)
        {
            if(sex == "male")
            {
                return SexType.Male;
            }
            else if(sex == "female")
            {
                return SexType.Female;
            }
            else
            {
                throw new ArgumentException("Конвертер в ApiService не смог распознать пол");
            }
        }
    }
}

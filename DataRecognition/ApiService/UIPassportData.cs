using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService
{
    public class UIPassportData
    {
        private readonly string series;
        private readonly string number;
        private readonly string placeIssue;
        private readonly string dateIssue;
        private readonly string departmentCode;
        private readonly string lastName;
        private readonly string firstName;
        private readonly string secondName;
        private readonly string sex;
        private readonly string birthDate;
        private readonly string birthPlace;

        public UIPassportData(Passport dataBasePassport)
        {
            series = dataBasePassport.Series;
            number = dataBasePassport.Number;
            placeIssue = dataBasePassport.IssuedBy;
            dateIssue = dataBasePassport.IssuedOn.ToString("ddMMyyyy");
            departmentCode = dataBasePassport.IssuedDepartment;
            lastName = dataBasePassport.Lastname;
            firstName = dataBasePassport.Firstname;
            secondName = dataBasePassport.Secondname;
            sex = SexTypeToString(dataBasePassport.Sex);
            birthDate = dataBasePassport.Birthday.ToString("ddMMyyyy");
            birthPlace = dataBasePassport.Address;
        }

        public Passport ParseToDatabasePassport()
        {
            return new Passport
            {
                Series = series,
                Number = number,
                IssuedBy = placeIssue,
                IssuedOn = StringToDateTime(dateIssue),
                IssuedDepartment = departmentCode,
                Lastname = lastName,
                Firstname = firstName,
                Secondname = secondName,
                Sex = StringTosexType(sex),
                Birthday = StringToDateTime(birthDate),
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

        private static DateTime StringToDateTime(string date)
        => DateTime.Parse(date.Substring(0, 2) + "." + date.Substring(2, 2) + "." + date.Substring(4));
    }
}

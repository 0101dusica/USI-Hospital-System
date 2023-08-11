using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.Utils.Converters
{
    public class DoctorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string doctorUsername = value as string;

            if (string.IsNullOrEmpty(doctorUsername))
                return null;

            DoctorService doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            Doctor? doctor = doctorService.GetByUsername(doctorUsername);

            string parameterValue = parameter as string;

            switch (parameterValue)
            {
                case "Specialization":
                    return doctor!.Specialization;
                case "Name":
                    return doctor!.Username;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


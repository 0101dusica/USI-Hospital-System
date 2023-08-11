using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.FreeDays.Commands;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.FreeDays.ViewModel
{
    public class AddFreeDaysViewModel : BaseViewModel
    {

        private int _days;
        public int Days
        {
            get { return _days; }
            set
            {
                _days = value;
                OnPropertyChanged(nameof(Days));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public Doctor LoggedDoctor { get; set; }

        public ICommand AddFreeDaysCommand { get; }

        public AddFreeDaysViewModel(Doctor loggedDoctor)
        {
            LoggedDoctor = loggedDoctor;
            SelectedDate = DateTime.Now;

            AddFreeDaysCommand = new AddFreeDaysCommand(this);
            ((AddFreeDaysCommand)AddFreeDaysCommand).FreeDaysRequestAdded += OnFreeDaysRequestAdded;
        }

        private void OnFreeDaysRequestAdded(object? sender, bool success)
        {
            if (success)
            {
                MessageBox.Show("Free days request added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add free days request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

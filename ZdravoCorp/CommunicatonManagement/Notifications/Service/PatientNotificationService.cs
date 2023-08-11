using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using ZdravoCorp.CommunicatonManagement.Notifications.Model;
using ZdravoCorp.CommunicatonManagement.Notifications.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.CommunicatonManagement.Notifications.Service
{
    public class PatientNotificationService : INotifyPropertyChanged
    {
        public MedicineService MedicineService { get; set; }
        public NotificationService NotificationService { get; set; }
        public PrescriptionService PrescriptionService { get; set; }
        public DispatcherTimer Timer { get; set; }
        public PatientNotificationService()
        {
            MedicineService = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));
            NotificationService = new NotificationService(new NotificationRepository(new Serializer<Notification>()));
            PrescriptionService = new PrescriptionService(new PrescriptionRepository(new Serializer<Prescription>()));
            Timer = new DispatcherTimer();
        }
        public void StartNotificationTimer()
        {

            Timer.Interval = TimeSpan.FromSeconds(7);
            Timer.Tick += Timer_Tick;
            Timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            PrescriptionNotificationsCheck(currentTime);
            OtherNotificationsCheck(currentTime);
        }

        private void OtherNotificationsCheck(DateTime currentTime)
        {
            foreach (var notification in NotificationService.GetAll())
            {
                DateTime startTime = notification.TimeSlot.StartTime;
                DateTime endTime = notification.TimeSlot.EndTime;
                if (notification.NotificationStatus != NotificationStatus.Showed)
                {
                    if (currentTime >= startTime && currentTime <= endTime)
                    {
                        DateTime medicationTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                            notification.Time.Hours, notification.Time.Minutes, notification.Time.Seconds);

                        TimeSpan timeUntilMedication = medicationTime - currentTime;

                        if (timeUntilMedication > TimeSpan.Zero && timeUntilMedication <= TimeSpan.FromHours(1))
                            ShowOtherNotification(notification);
                    }
                }
            }
        }

        private void PrescriptionNotificationsCheck(DateTime currentTime)
        {
            foreach (var notification in PrescriptionService.GetAll())
            {
                DateTime startTime = notification.TimeSlot.StartTime;
                DateTime endTime = notification.TimeSlot.EndTime;
                if (notification.PrescriptionStatus != PrescriptionStatus.Showed)
                {
                    if (currentTime >= startTime && currentTime <= endTime)
                    {
                        DateTime medicationTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                            notification.Time.Hours, notification.Time.Minutes, notification.Time.Seconds);

                        TimeSpan timeUntilMedication = medicationTime - currentTime;

                        if (timeUntilMedication > TimeSpan.Zero &&
                            timeUntilMedication <= TimeSpan.FromMinutes(notification.TimeSet))
                            ShowPrescriptionNotification(notification);
                    }
                }
            }
        }

        private void ShowPrescriptionNotification(Prescription notification)
        {
            Medicine? medicine = MedicineService.GetById(notification.MedicineId);
            MessageBox.Show("Its time for your therapy: " + medicine!.Name);
            notification.PrescriptionStatus = PrescriptionStatus.Showed;
            //StopNotificationTimer();
        }

        private void ShowOtherNotification(Notification notification)
        {
            MessageBox.Show("Its time for your notification: " + notification.Description);
            notification.NotificationStatus = NotificationStatus.Showed;
            //StopNotificationTimer();
        }

        public void StopNotificationTimer()
        {
            Timer.Stop();
            Timer.Tick -= Timer_Tick;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



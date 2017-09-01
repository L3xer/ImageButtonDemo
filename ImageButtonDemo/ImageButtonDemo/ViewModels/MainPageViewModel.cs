using Prism.Mvvm;

namespace ImageButtonDemo.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                SetProperty(ref _phoneNumber, value);

                RaisePropertyChanged(nameof(IsPhoneButtonDisabled));
            }
        }

        public bool IsPhoneButtonDisabled => string.IsNullOrWhiteSpace(_phoneNumber);
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Contacts.Validators
{
    public class ContactValidator : ObservableValidator
    {
        private  Contact _contact;

        public ContactValidator(Contact contact)
        {
            _contact = contact;
            ErrorsChanged += ContactValidator_ErrorsChanged;
            // To change the state of the button
            ValidateAllProperties();
        }
        ~ContactValidator()
        {
            ErrorsChanged -= ContactValidator_ErrorsChanged;
        }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName
        {
            get => _contact.FirstName;
            set => SetProperty(
                _contact.FirstName,
                value,
                _contact,
                (contact, firstName) => contact.FirstName = firstName,
                true);
        }
        [EmailAddressAllowEmpty]
        public string Email
        {
            get => _contact.Email;
            set => SetProperty(
                _contact.Email,
                value,
                _contact,
                (contact, email) => contact.Email = email,
                true);
        }
        public string FirstNameErrors => GetPropertyErrors(nameof(FirstName));
        public string EmailErrors => GetPropertyErrors(nameof(Email));
        private string GetPropertyErrors(string propertyName)
        {
            return string.Join(
                   Environment.NewLine,
                   from ValidationResult e in GetErrors(propertyName) select e.ErrorMessage);
        }

        private void ContactValidator_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FirstNameErrors));
            OnPropertyChanged(nameof(EmailErrors));
        }

    }
}

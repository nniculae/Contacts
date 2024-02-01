using System.ComponentModel.DataAnnotations;

namespace Contacts.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
public sealed class EmailAddressAllowEmptyAttribute : DataTypeAttribute
{
    public EmailAddressAllowEmptyAttribute()
           : base(DataType.EmailAddress)
    {
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is string str && string.IsNullOrEmpty(str))
        {
            return true;
        }

        if (value is not string valueAsString)
        {
            return false;
        }

        // only return true if there is only 1 '@' character
        // and it is neither the first nor the last character
        int index = valueAsString.IndexOf('@');

        return
            index > 0 &&
            index != valueAsString.Length - 1 &&
            index == valueAsString.LastIndexOf('@');
    }
}

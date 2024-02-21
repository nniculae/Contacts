using Contacts.Core.Models;

namespace Contacts.Core.Dto;

public class LabelsWithContactsCountDto
{
    public Label Label { get; set; } = null!;
    public int ContactsCount { get; set; }
}

using Contacts.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.ViewModels;
public sealed class ContactParameterWrapper(Contact Contact, string infoBarMessage = "")
{
    public Contact Contact { get; } = Contact;
    public string InfoBarMessage { get; } = infoBarMessage;
}

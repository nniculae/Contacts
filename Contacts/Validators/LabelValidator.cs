using CommunityToolkit.Mvvm.ComponentModel;
using Contacts.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Validators;
public class LabelValidator(ILabelService labelService ):ObservableValidator
{


    public async Task<Label?> GetLableByNameAsync(string name)
    {
        return await labelService.GetLabelByNameAsync(name);
    }
}

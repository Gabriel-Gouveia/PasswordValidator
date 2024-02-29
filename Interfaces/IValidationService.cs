using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidator.Interfaces
{
    public interface IValidationService
    {
        bool Validar(string senha);
    }
}

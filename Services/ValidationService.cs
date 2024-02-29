using PasswordValidator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidator.Services
{
    public class ValidationService : IValidationService
    {
        private string _specialChars = "!@#$%^&*()-+";

        public bool Validar(string senha)
        {
            return VerificaQuantidadeCaracteres(senha) &&
                   VerificaSeExisteDigito(senha) &&
                   VerificaSeExisteLetraMinuscula(senha) &&
                   VerificaSeExisteLetraMaiuscula(senha) &&
                   VerificaSeExisteCaractereEspecial(senha) &&
                   VerificaSeNaoExisteCaractereRepetido(senha) &&
                   VerificaSeNaoExisteEspacoEmBranco(senha);
        }

        private bool VerificaQuantidadeCaracteres(string senha)
        {
            return senha.Length >= 9;
        }

        private bool VerificaSeExisteDigito(string senha)
        {
            return senha.Any(c => char.IsDigit(c));
        }

        private bool VerificaSeExisteLetraMinuscula(string senha)
        {
            return senha.Any(c => char.IsLower(c));
        }

        private bool VerificaSeExisteLetraMaiuscula(string senha)
        {
            return senha.Any(c => char.IsUpper(c));
        }

        private bool VerificaSeExisteCaractereEspecial(string senha)
        {
            return senha.Any(c => _specialChars.Contains(c));
        }

        private bool VerificaSeNaoExisteCaractereRepetido(string senha)
        {
            return senha.Length == senha.Distinct().Count();
        }

        private bool VerificaSeNaoExisteEspacoEmBranco(string senha)
        {
            return !senha.Contains(' ');
        }
    }
}

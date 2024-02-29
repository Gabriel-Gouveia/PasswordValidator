using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordValidator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordValidator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private IValidationService _validationService;

        public PasswordController(IValidationService validationService)
        {
            _validationService = validationService;
        }

        [HttpPost]
        public ActionResult ValidarSenha(string senha)
        {
            try
            {
                if (_validationService.Validar(senha))
                    return Ok(true);
                return BadRequest(false);
            }
            catch (NullReferenceException nrex)
            {
                return BadRequest(false);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}

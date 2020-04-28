using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UbiChipher.WebDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {

        [HttpPost]
        [Route("Claims")]
        public IActionResult Claims([FromBody] List<Claim> claims)
        {
            if (claims == null)
            {
                string errorMessage = $"No claim set provided.";

                //logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            try
            {
                // TODO: stuff
                Dummy.Verified = true;

                return NoContent();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error trying to process cliams.";

                //logger.LogError(ex, errorMessage);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        [HttpGet]
        [Route("TestVerified")]
        public bool TestVerified()
        {
            return Dummy.Verified;
        }

    }
}
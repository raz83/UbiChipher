using System;
using System.Collections.Generic;
using Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UbiChipher.Services;

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
                var claimValidationService = new ClaimValidationService(null); // TODO: Remove null once DI added

                var claimsJson = JsonConvert.SerializeObject(claims); // TODO: Create an overload for ValidateClaim that accepts List<Claim>

                claimValidationService.ValidateClaim(claimsJson, out string hashOfClient, out string hashOnBlockChain, out bool match);

                var message = match ?
                    $"You are logged in, your claim fingerprint {hashOfClient} mathes fingerpring {hashOnBlockChain} on the blochain." :
                    $"You NOT are logged in, your claim fingerprint {hashOfClient} does not match fingerpring {hashOnBlockChain} on the blochain.";

                DummyBackend.Verified = true;
                DummyBackend.Message = message;

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
            return DummyBackend.Verified;
        }

    }
}
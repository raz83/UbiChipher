using Microsoft.AspNetCore.Mvc;
using UbiChipher.Data;

namespace UbiChipher.WebDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentController : ControllerBase
    {
        // TODO: add OpenAPI/ NSwag/ Swashbuckle/ code generation stuff...

        // For now, call using:
        // localhost:58787/api/Enrolment/EnrollClaims
        // localhost:58787/api/Enrolment/BitcoinAddressForEnrolmentPayment

        [HttpPost]
        [Route("EnrollClaims")]
        public string EnrollClaims([FromBody] Enrolment enrolment)
        {
            return $"TODO: Handle enrollment for {enrolment.Intermediary.Name}";
        }

        [HttpGet]
        [Route("BitcoinAddressForEnrolmentPayment")]
        public string BitcoinAddressForEnrolmentPayment()
        {
            var e = new Enrolment() { Intermediary = new Intermediary() { Name = "test" } };

            return "1FfmbHfnpaZjKFvyi1okTjJJusN455paPH"; // Api consumer will generate a QR from this output.
        }
    }
}
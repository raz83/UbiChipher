using Microsoft.AspNetCore.Mvc;
using System;
using UbiChipher.Data;

namespace UbiChipher.WebDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        // TODO: add OpenAPI/ NSwag/ Swashbuckle/ code generation stuff...

        // For now, call using:

        // 1. User enrollment partner business will have web forms to collect customer details,
        // creating website user "accounts" for their business as normal.
        // This will include KYC photo ID and declaration video.

        // 2. Then, after the enrollment partner business have finished collecting customer data,
        // They will pass it to a KYC verifying agency like W2Global for verification.

        // 3. They will add verification data to the customer account details, including the "certainty" value.

        // 4. When the customer sets up the payment options on the website of the enrollment partner business,
        // they will be given the option to create a 
        // "universal, portable, self-sovereign, privacy focused and decentralized digital identity wallet". 
        // This is based on the condition that the W2Global "certainty" value is at a threshold agreed by both
        // UbiChiper and the user enrollment partner business.


        // 5. Once the steps above are completed and all the conditions are met, the user enrollment partner business
        // will use this Api call to get the Bitcoin address for the enrollment processing fee and present it to 
        // the user as a QR code to scan and provide the payment.
        // localhost:58787/api/Enrollment/BitcoinAddressForEnrolmentPayment

        [HttpGet]
        [Route("BitcoinAddressForEnrolmentPayment")]
        public string BitcoinAddressForEnrolmentPayment()
        {
            return "1FfmbHfnpaZjKFvyi1okTjJJusN455paPH"; // Api consumer will generate a QR from this output.
        }

        // 6. Funds received will be held in escrow for 60 minutes to ensure payment confirmation and also
        // confirmation of user claims received in the next step. If timed out, Bitcoin is returned automatically.

        // 7. User enrollment partner business will then need to call this Api to carry out enrollment.
        // localhost:58787/api/Enrollment/EnrollClaims

        [HttpPost]
        [Route("EnrollClaims")]
        public string EnrollClaims([FromBody] Enrollment enrollment)
        {
            return $"TODO: This is where you will receive an enrollment request processing id for example:  {Guid.NewGuid()}";
        }

        // 8. There will be a temporary database for enrollments in progress.

        // 9. User enrollment partner business will call the next Api to get the
        // StoredCredentialsImportData when ready, using temporary id.

        [HttpGet]
        [Route("StoredCredentialsImportData")]
        public string StoredCredentialsImportData([FromQuery] string enrollmentProcessingId)
        {
            // Example returned: [{"Expires":"2021-04-21T15:24:12.6242835Z","PubKey":"21489122-ae06-4bb6-b01b-6e46bb15cc64","Claims":{"Name":"Murray"}}]
            
            // If processing not done, return nothing or wrap status code.
            return null; // Api car generate a QR from this output for the app, push notification, email, whatever.
        }
    }
}
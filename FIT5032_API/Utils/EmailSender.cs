using FIT5032_API.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace FIT5032_API.Utils
{
    public class EmailSender
    {
        private readonly SendGridClient _client;
        private string apiKey = "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";
        private static readonly string MessageId = "X-Message-Id";

        public EmailSender()
        {
            _client = new SendGridClient(apiKey);
        }

        public EmailResponse Send(SendEmailViewModel contract)
        {

            var emailMessage = new SendGridMessage()
            {
                From = new EmailAddress("scha0140@student.monash.edu"),
                Subject = contract.Subject,
                HtmlContent = contract.Contents
            };

            emailMessage.AddTo(new EmailAddress(contract.To));
            

            return ProcessResponse(_client.SendEmailAsync(emailMessage).Result);
        }

        private EmailResponse ProcessResponse(Response response)
        {
            if (response.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)
                || response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                return ToMailResponse(response);
            }

            //TODO check for null
            var errorResponse = response.Body.ReadAsStringAsync().Result;

            throw new EmailServiceException(response.StatusCode.ToString(), errorResponse);
        }

        private static EmailResponse ToMailResponse(Response response)
        {
            if (response == null)
                return null;

            var headers = (HttpHeaders)response.Headers;
            var messageId = headers.GetValues(MessageId).FirstOrDefault();
            return new EmailResponse()
            {
                UniqueMessageId = messageId,
                DateSent = DateTime.UtcNow,
            };
        }
    }
}
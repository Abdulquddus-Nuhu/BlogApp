using System;
using Infrastructure.Identity;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpOptions> _options;
        private readonly UserManager<Persona> _userManager;

        public SmtpEmailSender(IOptions<SmtpOptions> options, UserManager<Persona> userManager)
        {
            _options = options;
            _userManager = userManager;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            var apiKey = Environment.GetEnvironmentVariable("APIKEY") ?? _options.Value.ApiKey;
            var secretKey = Environment.GetEnvironmentVariable("SECRETKEY") ?? _options.Value.SecretKey;
            var user = await _userManager.FindByEmailAsync(email);

            MailjetClient client = new MailjetClient($"{apiKey}", $"{secretKey}")
            {
                //Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource,
            }
            .Property(Send.Messages, new JArray {
            new JObject {
            {
            "From",
            new JObject {
            {"Email", "abdulquddusnuhu@gmail.com"},
            {"Name", "Shopify Inventory"}
            }
            }, {
            "To",
            new JArray {
            new JObject {
                {
                "Email",
                email
                }, {
                "Name",
                user.FirstName
                }
            }
            }
            }, {
            "Subject",
            subject
            }, {
            "HTMLPart",
            htmlMessage
            },
            }
                });
            await client.PostAsync(request);
        }
    }
}


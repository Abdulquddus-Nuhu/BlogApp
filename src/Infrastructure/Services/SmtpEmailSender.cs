﻿using System;
using Infrastructure.Identity;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly UserManager<Persona> _userManager;
        private readonly ILogger<SmtpEmailSender> _logger;
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(UserManager<Persona> userManager,
            ILogger<SmtpEmailSender> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            //var apiKey = Environment.GetEnvironmentVariable("APIKEY") ?? _options.Value.ApiKey;
            //var secretKey = Environment.GetEnvironmentVariable("SECRETKEY") ?? _options.Value.SecretKey;

            var apiKey = _configuration["Smtp:Username"];
            var secretKey = _configuration["Smtp:Password"];
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

            _logger.LogInformation("Sending confirmation email to {0}", user.FirstName +" "+ user.LastName);
             await client.PostAsync(request);
        }
    }
}


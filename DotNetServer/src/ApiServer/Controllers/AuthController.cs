﻿using System.Net.Http;
using System.Web.Http;
using Common.Service;
using Core.Commands.AppUserCommands;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;

namespace WebApp.Controllers
{
    [Authorize]
    public class AuthController : SmartApiController
    {
        private readonly ICryptographer _cryptographer;
        private readonly IBus _bus;

        public AuthController(ICryptographer cryptographer, IBus bus)
        {
            _cryptographer = cryptographer;
            _bus = bus;
        }

        public HttpResponseMessage Put(ChangePasswordForm form)
        {
            var response = new WebApiResponseBase();

            var user = GetCurrentUser();

            var success = user != null && _cryptographer.GetPasswordHash(form.OldPassword, user.PasswordSalt).Equals(user.PasswordHash);

            if (!success)
            {
                response.AddError("OldPassword or NewPassword or ConfirmPassword", "is Invalid");
                return Content(response);
            }

            var command = new ChangePassword
            {
                Id = user.Id,
                OldPassword = form.OldPassword,
                NewPassword = form.NewPassword,
                ConfirmPassword = form.ConfirmPassword,
            };

            return ExecuteCommand(command);
        }

        public HttpResponseMessage Delete()
        {
            var response = new WebApiResponseBase();

            var token = Request.Headers.Authorization.Parameter;

            _bus.Send<RemoveTokenCommand>(c =>
            {
                c.TokenHash = _cryptographer.ComputeHash(token);
            });
            
            return Content(response);
        }
    }
}
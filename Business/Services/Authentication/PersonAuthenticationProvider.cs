﻿using Business.Adapters.SmsService;
using Business.Constants;
using Business.Services.Model;
using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Authentication
{
    public class PersonAuthenticationProvider : AuthenticationProviderBase, IAuthenticationProvider
    {
        private readonly IUserRepository _users;

        private readonly ITokenHelper _tokenHelper;

        public PersonAuthenticationProvider(AuthenticationProviderType providerType, IUserRepository users, IMobileLoginRepository mobileLogins, ITokenHelper tokenHelper, ISmsService smsService)
            : base(mobileLogins, smsService)
        {
            _users = users;
            ProviderType = providerType;
            _tokenHelper = tokenHelper;
        }

        public AuthenticationProviderType ProviderType { get; }

        public override async Task<LoginUserResult> Login(LoginUserCommand command)
        {
            var citizenId = command.AsCitizenId();
            var user = await _users.Query()
                .Where(u => u.CitizenId == citizenId)
                .FirstOrDefaultAsync();


            if (command.IsPhoneValid)
            {
                return await PrepareOneTimePassword(AuthenticationProviderType.Person, user.MobilePhones, user.CitizenId.ToString());
            }

            return new LoginUserResult
            {
                Message = Messages.TrueButCellPhone,

                Status = LoginUserResult.LoginStatus.PhoneNumberRequired,
                MobilePhones = new string[] { user.MobilePhones }
            };
        }

        public override async Task<DArchToken> CreateToken(VerifyOtpCommand command)
        {
            var citizenId = long.Parse(command.ExternalUserId);
            var user = await _users.GetAsync(u => u.CitizenId == citizenId);
            user.AuthenticationProviderType = ProviderType.ToString();

            var claims = _users.GetClaims(user.UserId);
            var accessToken = _tokenHelper.CreateToken<DArchToken>(user);
            accessToken.Provider = ProviderType;
            return accessToken;
        }
    }
}

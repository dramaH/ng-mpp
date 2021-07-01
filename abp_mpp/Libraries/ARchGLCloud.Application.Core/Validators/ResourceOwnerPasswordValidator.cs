// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;
using IdentityServer4.Services;
using IdentityServer4.Events;
using ARchGLCloud.Application.Core.Interfaces;
using ARchGLCloud.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ARchGLCloud.Application.Core.Validators
{ 
    /// <summary>
    /// IResourceOwnerPasswordValidator that integrates with ASP.NET Identity.
    /// </summary>
    /// <seealso cref="IdentityServer4.Validation.IResourceOwnerPasswordValidator" />
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    { 
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEventService _events;
        private readonly ICaptchaService _captchaService;
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOwnerPasswordValidator"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="events">The events.</param>
        /// <param name="captchaService">The captchaService.</param>
        /// <param name="logger">The logger.</param>
        public ResourceOwnerPasswordValidator(UserManager<AspNetUser> userManager,
                                              SignInManager<AspNetUser> signInManager,
                                              ICaptchaService captchaService,
                                              IEventService events,
                                              ILogger<ResourceOwnerPasswordValidator> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _captchaService = captchaService;
            _events = events;
            _logger = logger;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var clientId = context.Request?.Client?.ClientId;
            var user = await _userManager.FindByNameAsync(context.UserName);

            //解决老的用户无法使用手机授权的问题
            if (user == null)
            {
                user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.PhoneNumber == context.UserName);
            }

            if (user != null)
            {
                SignInResult result = SignInResult.Failed;
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    var captcha = await _captchaService.GetAsync(user.PhoneNumber);
                    if (context.Password.Equals(captcha?.Code))
                    {
                        result = SignInResult.Success;
                    }
                    else
                    {
                        result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                    }
                }
                else
                {
                    result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                }

                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);

                    _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, false, clientId));

                    context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                    return;
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "该用户账号被锁定", false, clientId));
                }
                else if (result.IsNotAllowed)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "该用户没有被授权", false, clientId));
                }
                else
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "无效的用户名或密码", false, clientId));
                }
            }
            else
            {
                _logger.LogInformation("No user found matching username: {username}", context.UserName);
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "该用户尚未注册", false, clientId));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}

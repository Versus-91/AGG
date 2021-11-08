using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Abp;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Microsoft.AspNetCore.Mvc;
using AFIAT.TST.Authorization.Users;
using AFIAT.TST.Identity;
using AFIAT.TST.MultiTenancy;
using AFIAT.TST.Url;
using AFIAT.TST.Web.Controllers;
using AFIAT.TST.Authorization;
using AFIAT.TST.Web.Models.Ui;
using Abp.UI;
using AFIAT.TST.Authorization.Accounts.Dto;
using AFIAT.TST.Authorization.Accounts;
using Abp.Authorization;
using Abp.Authorization.Users;

namespace AFIAT.TST.Web.Public.Controllers
{
    public class AccountController : TSTControllerBase
    {
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IWebUrlService _webUrlService;
        private readonly TenantManager _tenantManager;
        private readonly LogInManager _logInManager;
        private readonly IAccountAppService _accountAppService;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;

        public AccountController(
            UserManager userManager,
            SignInManager signInManager,
            IWebUrlService webUrlService,
            TenantManager tenantManager,
            LogInManager logInManager,
            IAccountAppService accountAppService,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webUrlService = webUrlService;
            _tenantManager = tenantManager;
            _logInManager = logInManager;
            _accountAppService = accountAppService;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
        }
        public  ActionResult Login()
        {
            return View();
        }
        public  ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "")
        {
            if (model.TenancyName != null)
            {
                var isTenantAvailable = await _accountAppService.IsTenantAvailable(new IsTenantAvailableInput
                {
                    TenancyName = model.TenancyName
                });

                switch (isTenantAvailable.State)
                {
                    case TenantAvailabilityState.InActive:
                        throw new UserFriendlyException(L("TenantIsNotActive", model.TenancyName));
                    case TenantAvailabilityState.NotFound:
                        throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", model.TenancyName));
                }
            }

            var loginResult = await GetLoginResultAsync(model.UserNameOrEmailAddress, model.Password, model.TenancyName);

            if (loginResult.User.ShouldChangePasswordOnNextLogin)
            {
                throw new UserFriendlyException(L("RequiresPasswordChange"));
            }

            var signInResult = await _signInManager.SignInOrTwoFactorAsync(loginResult, model.RememberMe);

            if (signInResult.RequiresTwoFactor)
            {
                throw new UserFriendlyException(L("RequiresTwoFactorAuth"));
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }
        public async Task<ActionResult> LoginUser(string accessToken, string userId, string tenantId = "", string returnUrl = "")
        {
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(userId))
            {
                return await RedirectToExternalLoginPageAsync();
            }

            var targetTenantId = string.IsNullOrEmpty(tenantId) ? null : (int?)Convert.ToInt32(Base64Decode(tenantId));
            CurrentUnitOfWork.SetTenantId(targetTenantId);

            var targetUserId = Convert.ToInt64(Base64Decode(userId));

            var user = await _userManager.GetUserAsync(new UserIdentifier(targetTenantId, targetUserId));
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!user.SignInToken.Equals(accessToken) || !(user.SignInTokenExpireTimeUtc >= Clock.Now.ToUniversalTime()))
            {
                return RedirectToAction("Index", "Home");
            }

            CurrentUnitOfWork.SetTenantId(targetTenantId);
            await _signInManager.SignInAsync(user, false);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Logout()
        {
            var tenancyName = await GetCurrentTenancyName();
            var websiteAddress = _webUrlService.GetSiteRootAddress(tenancyName);
            var serverAddress = _webUrlService.GetServerRootAddress(tenancyName);

            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        private async Task<ActionResult> RedirectToExternalLoginPageAsync()
        {
            var tenancyName = await GetCurrentTenancyName();
            var serverAddress = _webUrlService.GetServerRootAddress(tenancyName);
            var websiteAddress = _webUrlService.GetSiteRootAddress(tenancyName);
            var originalReturnUrl = Request.Query.ContainsKey("ReturnUrl") ? Request.Query["ReturnUrl"].ToString() : "";
            var returnUrl = websiteAddress.EnsureEndsWith('/') + "account/login?returnUrl=" + websiteAddress.EnsureEndsWith('/') + originalReturnUrl.TrimStart('/');
            return Redirect(serverAddress.EnsureEndsWith('/') + "account/login?ss=true&returnUrl=" + WebUtility.UrlEncode(returnUrl));
        }

        private async Task<string> GetCurrentTenancyName()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return "";
            }

            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
            return tenant.TenancyName;
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}

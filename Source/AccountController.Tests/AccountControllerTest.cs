// <copyright file="AccountControllerTest.cs">Copyright ©  2016</copyright>
using System;
using System.Web.Mvc;
using Journals.Model;
using Journals.Web.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AccountController.Tests
{
    /// <summary>This class contains parameterized unit tests for AccountController</summary>
    [PexClass(typeof(AccountController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class AccountControllerTest
    {
        /// <summary>Test stub for Disassociate(String, String)</summary>
        [PexMethod]
        public ActionResult DisassociateTest(
            [PexAssumeUnderTest]AccountController target,
            string provider,
            string providerUserId
        )
        {
            ActionResult result = target.Disassociate(provider, providerUserId);
            return result;
            // TODO: add assertions to method AccountControllerTest.DisassociateTest(AccountController, String, String)
        }

        /// <summary>Test stub for ExternalLoginCallback(String)</summary>
        [PexMethod]
        public ActionResult ExternalLoginCallbackTest([PexAssumeUnderTest]AccountController target, string returnUrl)
        {
            ActionResult result = target.ExternalLoginCallback(returnUrl);
            return result;
            // TODO: add assertions to method AccountControllerTest.ExternalLoginCallbackTest(AccountController, String)
        }

        /// <summary>Test stub for ExternalLoginConfirmation(RegisterExternalLoginModel, String)</summary>
        [PexMethod]
        public ActionResult ExternalLoginConfirmationTest(
            [PexAssumeUnderTest]AccountController target,
            RegisterExternalLoginModel model,
            string returnUrl
        )
        {
            ActionResult result = target.ExternalLoginConfirmation(model, returnUrl);
            return result;
            // TODO: add assertions to method AccountControllerTest.ExternalLoginConfirmationTest(AccountController, RegisterExternalLoginModel, String)
        }

        /// <summary>Test stub for ExternalLoginFailure()</summary>
        [PexMethod]
        public ActionResult ExternalLoginFailureTest([PexAssumeUnderTest]AccountController target)
        {
            ActionResult result = target.ExternalLoginFailure();
            return result;
            // TODO: add assertions to method AccountControllerTest.ExternalLoginFailureTest(AccountController)
        }

        /// <summary>Test stub for ExternalLogin(String, String)</summary>
        [PexMethod]
        public ActionResult ExternalLoginTest(
            [PexAssumeUnderTest]AccountController target,
            string provider,
            string returnUrl
        )
        {
            ActionResult result = target.ExternalLogin(provider, returnUrl);
            return result;
            // TODO: add assertions to method AccountControllerTest.ExternalLoginTest(AccountController, String, String)
        }

        /// <summary>Test stub for ExternalLoginsList(String)</summary>
        [PexMethod]
        public ActionResult ExternalLoginsListTest([PexAssumeUnderTest]AccountController target, string returnUrl)
        {
            ActionResult result = target.ExternalLoginsList(returnUrl);
            return result;
            // TODO: add assertions to method AccountControllerTest.ExternalLoginsListTest(AccountController, String)
        }

        /// <summary>Test stub for LogOff()</summary>
        [PexMethod]
        public ActionResult LogOffTest([PexAssumeUnderTest]AccountController target)
        {
            ActionResult result = target.LogOff();
            return result;
            // TODO: add assertions to method AccountControllerTest.LogOffTest(AccountController)
        }

        /// <summary>Test stub for Login(String, String)</summary>
        [PexMethod]
        public ActionResult LoginTest(
            [PexAssumeUnderTest]AccountController target,
            string returnUrl,
            string errorMessage
        )
        {
            ActionResult result = target.Login(returnUrl, errorMessage);
            return result;
            // TODO: add assertions to method AccountControllerTest.LoginTest(AccountController, String, String)
        }

        /// <summary>Test stub for Login(LoginModel, String)</summary>
        [PexMethod]
        public ActionResult LoginTest01(
            [PexAssumeUnderTest]AccountController target,
            LoginModel model,
            string returnUrl
        )
        {
            ActionResult result = target.Login(model, returnUrl);
            return result;
            // TODO: add assertions to method AccountControllerTest.LoginTest01(AccountController, LoginModel, String)
        }

        /// <summary>Test stub for Manage(Nullable`1&lt;ManageMessageId&gt;)</summary>
        [PexMethod]
        public ActionResult ManageTest(
            [PexAssumeUnderTest]AccountController target,
            AccountController.ManageMessageId? message
        )
        {
            ActionResult result = target.Manage(message);
            return result;
            // TODO: add assertions to method AccountControllerTest.ManageTest(AccountController, Nullable`1<ManageMessageId>)
        }

        /// <summary>Test stub for Manage(LocalPasswordModel)</summary>
        [PexMethod]
        public ActionResult ManageTest01(
            [PexAssumeUnderTest]AccountController target,
            LocalPasswordModel model
        )
        {
            ActionResult result = target.Manage(model);
            return result;
            // TODO: add assertions to method AccountControllerTest.ManageTest01(AccountController, LocalPasswordModel)
        }

        /// <summary>Test stub for Register()</summary>
        [PexMethod]
        public ActionResult RegisterTest([PexAssumeUnderTest]AccountController target)
        {
            ActionResult result = target.Register();
            return result;
            // TODO: add assertions to method AccountControllerTest.RegisterTest(AccountController)
        }

        /// <summary>Test stub for Register(RegisterModel)</summary>
        [PexMethod]
        public ActionResult RegisterTest01(
            [PexAssumeUnderTest]AccountController target,
            RegisterModel model
        )
        {
            ActionResult result = target.Register(model);
            return result;
            // TODO: add assertions to method AccountControllerTest.RegisterTest01(AccountController, RegisterModel)
        }

        /// <summary>Test stub for RemoveExternalLogins()</summary>
        [PexMethod]
        public ActionResult RemoveExternalLoginsTest([PexAssumeUnderTest]AccountController target)
        {
            ActionResult result = target.RemoveExternalLogins();
            return result;
            // TODO: add assertions to method AccountControllerTest.RemoveExternalLoginsTest(AccountController)
        }
    }
}

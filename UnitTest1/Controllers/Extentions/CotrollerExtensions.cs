using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest1.Controllers.Extentions
{
    public static class ControllerExtensions

    {

        public static void SetUserIsAuthenticated(this Controller controller, bool isAuthenticated)

        {

            var mockContext = new Mock<HttpContext>(MockBehavior.Default);

            mockContext.SetupGet(httpCon => httpCon.User.Identity.IsAuthenticated).Returns(isAuthenticated);

            controller.ControllerContext = new ControllerContext { HttpContext = mockContext.Object };

        }

    }



}

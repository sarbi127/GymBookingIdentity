using GymBookingNC19.Controllers;
using GymBookingNC19.Core;
using GymBookingNC19.Core.Models;
using GymBookingNC19.Core.ViewModels;
using GymBookingNC19.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTest1
{
    [TestClass]
    public class GymClassesControllerTests
    {
        private Mock<IGymClassesRepository> repository;
        private GymClassesController controller;

        [TestInitialize]
        public void SetUp()
        {

            // Repository
            //UnitOfWork
            //UserManager<ApplicationUser>
            //UserStore
            //Contoroller

          
            repository = new Mock<IGymClassesRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(u => u.gymClassesRepository).Returns(repository.Object);

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUsermanager = new Mock<UserManager<ApplicationUser>>
                (userStore.Object,null,null,null,null,null,null,null,null);

            controller = new GymClassesController(mockUsermanager.Object, mockUoW.Object);

        }

        private void SetUpUserIsAuthenticated(Controller controllers, bool isAuthenticated)
        {

            var mockContext = new Mock<HttpContext>(MockBehavior.Default);
            mockContext.SetupGet(httpCon => httpCon.User.Identity.IsAuthenticated).Returns(isAuthenticated);
            controller.ControllerContext = new ControllerContext { HttpContext = mockContext.Object };

        }




        [TestMethod]
        public async Task Index_ReturnsViewResult_shouldPass()
        {
            //Arrange
            SetUpUserIsAuthenticated(controller, true);
            var vm = new IndexViewModel { History = false };

            //Act
            var actual = await controller.Index(vm);

            //Assert
            Assert.IsInstanceOfType(actual, typeof(ViewResult));


        }
    }
}

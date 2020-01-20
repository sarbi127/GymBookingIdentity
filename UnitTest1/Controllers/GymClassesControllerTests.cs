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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTest1
{
    [TestClass]
    public class GymClassesControllerTests
    {
        private Mock<IGymClassesRepository> repository;
        private GymClassesController controller;
        private const int gymClassIsNotExits = 3;

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

        [TestMethod]
        public void Index_ReturnsAllGymClasses()
        {


            var classes = GetGymClassList();
            var expected = new IndexViewModel { GymClasses = classes };
            
            //var expected = GetGymClassList();
            repository.Setup(g => g.GetAllAsync()).ReturnsAsync(classes);
            var vm = new IndexViewModel { History = false };
            SetUpUserIsAuthenticated(controller, false);



            var viewResult = controller.Index(vm).Result as ViewResult;

            var actual = (IndexViewModel)viewResult.Model;

            Assert.AreEqual(expected.GymClasses, actual.GymClasses);

        }

        [TestMethod]
        public void Details_GetCorrectGeymClass()
        {
            var id = 1;
            var expected = GetGymClassList()[0];
            repository.Setup(g => g.GetAsync(expected.Id)).ReturnsAsync(expected);

            var actual = (ViewResult)controller.Details(id).Result;

            Assert.AreEqual(expected, actual.Model);

        }

        [TestMethod]
        public void Details_NoGymClassExistsWithGivenId_ShouldReturnNotFound()
        {
            var result = (StatusCodeResult)controller.Details(gymClassIsNotExits).Result;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void Create_ReturnDefultView_ShouldReturnNull()
        {

            var result = controller.Create() as ViewResult;
            Assert.IsNull(result.ViewName);


        }

        private List<GymClass> GetGymClassList()
        {
            return new List<GymClass> 
            {
                  new GymClass
                {

                    Id = 1,
                    Name = "Spinning",
                    Description = "Hard",
                    StartDate = DateTime.Now.AddDays(3),
                    Duration = new TimeSpan(0,60,0)

                },

                new GymClass
                {

                    Id = 2,
                    Name = "HyperFys",
                    Description = "Harder",
                    StartDate = DateTime.Now.AddDays(-3),
                    Duration = new TimeSpan(0,60,0)

                }
            };



        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using HtmlAgilityPack;
using TLP;
using TLP.Controllers;
using System.Web.Mvc;
using System.Web.WebPages;
using NUnit.Framework;
using TLP.Models;
using TLP.DAL;
using Moq;
using TLP.Security;
using UnitTestTLP.ModelTest;
using UnitTestTLP.Content;

namespace UnitTestTLP
{
    /**
     * Testing thr controller ability to retrieve the acpeted results
     * */
    [TestFixture]
    public class LoginControllerTest
    {
        TestDataAccessLayer tdal = new TestDataAccessLayer();
        
        [Test]
        [Ignore("stam")]
        public void Index_Returns_ActionResults() //<Action("what am i testing")>_<input>_<expected output>
        {
            // Arrange
            LoginController controller = new LoginController();
            // Act
            var result = controller.Index() as ViewResult;
            // Assert
            try
            {
                Assert.IsInstanceOf<ActionResult>(result);
            }
            catch
            {
                tdal.reports.Add(new reportFail {testType="Login Controller",
                                                 errorMessage= "Action Index failed",
                                                 timeLog=DateTime.UtcNow.Date });
            }
        }
        [Test]
        public void Authenticate_Returns_ActionResults() //<Action("what am i testing")>_<input>_<expected output>
        {
            
            List<TestCLAA> tests = tdal.testCLAA.ToList<TestCLAA>();
            
            for (int i = 0; i < tests.Count; i++)
            {
            
                TestCLAA testToDo = tests.ElementAt(i);

                test_authenticate(new User
                {
                    username = testToDo.username,
                    password = testToDo.password
                }, testToDo.excpectedResult);
            
            }
            
            
        }
        private void test_authenticate(User user, int expectedResult)
        {
            // Arrange
            Encryption enc = new Encryption();
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();

            mockUser.Setup(m => m.Users).Returns(tdal.mockDBusers.AsQueryable());
            LoginController controller = new LoginController(mockUser.Object);

            // Act
            if (expectedResult == 1)
            {
                var result = controller.Authenticate(user) as RedirectToRouteResult;
                // Assert
                try
                {
                    Assert.IsTrue(result.RouteName.Equals("Home"));
                }
                catch
                {
                    tdal.reports.Add(new reportFail
                    {
                        testType = "Login Controller",
                        errorMessage = "Action authenticate failed :" +
                                        "accpeted to be authenciated and authentication failed."+
                                        "userName"+user.username+" password:"+user.password ,
                        timeLog = DateTime.UtcNow.Date
                    });
                 
                }
      
            }
            else
            {
                var result = controller.Authenticate(user) as ActionResult;
                // Assert
                try
                {
                    Assert.IsInstanceOf<ActionResult>(result);
                }
                catch
                {
                    tdal.reports.Add(new reportFail
                    {
                        testType = "Login Controller",
                        errorMessage = "Action authenticate failed :" +
                                        "NOT accpeted to be authenciated and authentication succeded." +
                                        "userName" + user.username + " password:" + user.password,
                        timeLog = DateTime.UtcNow.Date
                    });
             
                }
            }

            
        }
        [Test]
        public void Registrate_Returns_ActionResults()
        {
            // Arrange
            LoginController controller = new LoginController();
            // Act
            var result = controller.Registrate() as ViewResult;
            // Assert
            try
            {
                Assert.IsInstanceOf<ActionResult>(result);
            }
            catch
            {
                tdal.reports.Add(new reportFail
                {
                    testType = "Login Controller",
                    errorMessage = "Action Registrate failed :" ,                                        
                    timeLog = DateTime.UtcNow.Date
                });
            }
        }
        [Test]
        public void test_AddNewUser()
        {
            List<TestCLAR> tests = tdal.testCLAR.ToList<TestCLAR>();
            
            for (int i = 0; i < tests.Count; i++)
            {
                //input
                TestCLAR testToDo = tests.ElementAt(i);

                test_registrate(new User
                {
                    username = testToDo.username,
                    password = testToDo.password
                }, testToDo.excpectedResult);
                
            }
            

        }
        private void test_registrate(User user, int expectedResult)
        {
            // Arrange
            Encryption enc = new Encryption();
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();

            mockUser.Setup(m => m.Users).Returns(tdal.mockDBusers.AsQueryable());
            LoginController controller = new LoginController(mockUser.Object);

            // Act
            if (expectedResult == 1)
            {
                var result = controller.AddNewUser(user) as RedirectToRouteResult;
                // Assert
                try
                {
                    Assert.IsTrue(result.RouteName.Equals("Home"));
                    
                }
                catch
                {
                    tdal.reports.Add(new reportFail
                    {
                        testType = "Login Controller",
                        errorMessage = "Action registrate failed :" +
                                        " accpeted to be registrate and registration failed." +
                                        "userName" + user.username + " password:" + user.password,
                        timeLog = DateTime.UtcNow.Date
                    });

                    
                }
                
            }
            else
            {
                var result = controller.AddNewUser(user) as ActionResult;
                // Assert
                try
                {
                    Assert.IsInstanceOf<ActionResult>(result);
                }
                catch
                {
                    tdal.reports.Add(new reportFail
                    {
                        testType = "Login Controller",
                        errorMessage = "Action registrate failed :" +
                                        "NOT accpeted to be registrate and registration succeded." +
                                        "userName" + user.username + " password:" + user.password,
                        timeLog = DateTime.UtcNow.Date
                    });
                }
            }

            cleam_up_mock_registration(user);
        }
        private void cleam_up_mock_registration(User user)
        {
            if(tdal.mockDBusers.First<User>(u=>u.username.Equals(user.username))!=null)
                tdal.mockDBusers.Remove(user);
        }
            }
}

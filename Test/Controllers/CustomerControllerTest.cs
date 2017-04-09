using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TLP.Controllers;
using TLP.DAL;
using TLP.Models;
using UnitTestTLP.ModelTest;

[assembly: InternalsVisibleTo("TLP.Controllers")]
namespace UnitTestTLP
{
    class CustomerControllerTest
    {
        TestDataAccessLayer tdal = new TestDataAccessLayer();
        [Test]
        public void GetAuctionsByJson_works()
        {
            
            List<TestCCAJ> tests=tdal.testCCAJ.ToList<TestCCAJ>();
            for (int i = 0; i < tests.Count; i++)
            {
                //input
                TestCCAJ testToDo = tests.ElementAt(i);
                DateTime[] dates = testToDo.givenDates();
                

                List<Auction> returnVal = GetAuctionsByJson_Action(dates[0], dates[1], dates[2]);

                int excpctedCount = (int)testToDo.excpectedCount;
                Assert.AreEqual(excpctedCount, returnVal.Count);
                switch (excpctedCount)
                {
                    case 1:
                        {
                            if(AssertJ1(returnVal, testToDo.excpectedNames())==0)
                            tdal.reports.Add(new reportFail
                            {
                                testType = "Customer Controller",
                                errorMessage = "Action GetAuctionsByJson failed : AssertJ1 failed." +
                                   " testid:" + testToDo.tid,
                                timeLog = DateTime.UtcNow.Date
                            });
                            break;

                        }
                    case 2: { if (AssertJ2(returnVal, testToDo.excpectedNames()) == 0)
                            {
                                tdal.reports.Add(new reportFail
                                {
                                    testType = "Customer Controller",
                                    errorMessage = "Action GetAuctionsByJson failed : AssertJ2 failed." +
                                   " testid:" + testToDo.tid,
                                    timeLog = DateTime.UtcNow.Date
                                });
                            }
                             break;}
                    case 3: {
                            if (AssertJ3(returnVal, testToDo.excpectedNames()) == 0)
                            {
                                tdal.reports.Add(new reportFail
                                {
                                    testType = "Customer Controller",
                                    errorMessage = "Action GetAuctionsByJson failed : AssertJ3 failed." +
                                 " testid:" + testToDo.tid,
                                    timeLog = DateTime.UtcNow.Date
                                });
                            }
                             break; }
                }

                Assert.IsTrue(DateTime.Today.CompareTo(returnVal[0].deadLine) <= 0);
            }
        }
        private int AssertJ1(List<Auction> toAssert, string[] excpectedNames)
        {

            try {  Assert.IsTrue(toAssert.ElementAt(0).auctionname == excpectedNames[0] ||
                   toAssert.ElementAt(0).auctionname == excpectedNames[1] ||
                   toAssert.ElementAt(0).auctionname == excpectedNames[2]);
                }
            catch {  return 0; }
            return 1;
        }
        private int AssertJ2(List<Auction> toAssert, string[] excpectedNames)
        {
            

            if (excpectedNames[0] == " ")
            {
                try {
                    Assert.IsTrue(toAssert.ElementAt(0).auctionname == excpectedNames[0] &&
                               toAssert.ElementAt(1).auctionname == excpectedNames[1]);
                    }
                catch  { return 0; }
            }
            else 
            {
                if (excpectedNames[1] != " ")
                {
                    try {
                        Assert.IsTrue(toAssert.ElementAt(0).auctionname == excpectedNames[1] &&
                                   toAssert.ElementAt(1).auctionname == excpectedNames[2]);
                        }
                    catch { return 0; }
                }
                else
                {
                    try {
                        Assert.IsTrue(toAssert.ElementAt(0).auctionname == excpectedNames[0] &&
                                     toAssert.ElementAt(1).auctionname == excpectedNames[2]);
                    }
                    catch { return 0; }
                }

            }
            return 1;
        }

        private int AssertJ3(List<Auction> toAssert, string[] excpectedNames)
        {
            try {
                Assert.IsTrue(toAssert.ElementAt(0).auctionname == excpectedNames[0] &&
                             toAssert.ElementAt(1).auctionname == excpectedNames[1] &&
                             toAssert.ElementAt(2).auctionname == excpectedNames[2]);
            }
            catch { return 0; }
            return 1;
        }
        private List<Auction> GetAuctionsByJson_Action(DateTime dl1, DateTime dl2, DateTime dl3)
        {
            // Arrange
            Mock<IAuctionRespository> mockAuction = new Mock<IAuctionRespository>();
            mockAuction.Setup(m => m.Auctions).Returns(new Auction[]
                {
                    new Auction { a_id=1, auctionname="computer", deadLine=dl1},
                    new Auction { a_id=2, auctionname="keyboard", deadLine=dl2},
                    new Auction { a_id=3, auctionname="mouse", deadLine=dl3}
                }.AsQueryable());
            CustomerController controller = new CustomerController(mockAuction.Object);
            var actual = controller.GetAuctionsByJson() as JsonResult;
            var tocheck = actual.ToString();
            //parse result
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string stam = serializer.Serialize(actual.Data);
            List<Auction> result = serializer.Deserialize<List<Auction>>(serializer.Serialize(actual.Data));
            return result;
            
            
        }
    }
}
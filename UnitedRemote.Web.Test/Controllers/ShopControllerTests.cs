using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Http.Results;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NetTopologySuite.Geometries;
using UnitedRemote.Core.Helpers;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories.Interfaces;
using UnitedRemote.Core.ViewModels;
using UnitedRemote.Web.Controllers;
using Xunit;

namespace UnitedRemote.Web.Test.Controllers
{
    public class ShopControllerTests
    {

        private readonly ShopController _shopController;
        private readonly List<Shop> FakeShopList = new List<Shop>
        {
            new Shop(){Name = "Shop 1" ,Location = new Point(10, 20){SRID = 4326}},
            new Shop(){Name = "Shop 2" ,Location = new Point(30, 40){SRID = 4326}},
            new Shop(){Name = "Shop 3" ,Location = new Point(50, 60){SRID = 4326}},
            new Shop(){Name = "Shop 4" ,Location = new Point(70, 20){SRID = 4326}}
        };

        public ShopControllerTests()

        {
            var errorHandler = new Mock<IErrorHandler>();
            var shopRepository = new Mock<IShopRepository>();
            shopRepository.Setup(x => x.GetAll()).Returns(FakeShopList);
            shopRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(FakeShopList[0]);


            _shopController = new ShopController(shopRepository.Object, errorHandler.Object);
        }


        [Fact]
        public void GetAll_Shops_ShouldResponseWithOK()
        {
            //Arrange is done in the constuctor 

            //Act
            var actionResult = _shopController.Get();
            var result = actionResult.Result as OkObjectResult;
            var dataResult = result.Value as IEnumerable<ShopViewModel>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(dataResult);
            Assert.IsAssignableFrom<IEnumerable<ShopViewModel>>(dataResult);
            Assert.Equal(FakeShopList.Count, dataResult.Count());
        }

        [Fact]
        public void Get_Shop_ShouldResponseWithOK()
        {
            //Arrange is done in the constuctor 

            //Act
            var actionResult = _shopController.Get(0);
            var result = actionResult.Result as OkObjectResult;
            var dataResult = result.Value as ShopViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(dataResult);
            Assert.IsAssignableFrom<ShopViewModel>(dataResult);
            Assert.Equal(FakeShopList[0].Name, dataResult.Name);
        }

        [Fact]
        public void Add_Shop_ShouldResponseWithOK()
        {
            //Arrange
            var fakeShop = new ShopViewModel
            {
                latitude = 5,
                longitude = 3,
                Name = "shop 5"
            };

            //Act
            var actionResult = _shopController.Post(fakeShop);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(actionResult);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using UnitedRemote.Core.Helpers;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories.Interfaces;
using UnitedRemote.Core.ViewModels;
using UnitedRemote.Core.ViewModels.Shop;

namespace UnitedRemote.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ShopController : ControllerBase
    {

        private readonly IShopRepository _shopRepository;
        private readonly IErrorHandler _errorHandler;


        public ShopController(IShopRepository shopRepository, IErrorHandler errorHandler)
        {
            _shopRepository = shopRepository;
            _errorHandler = errorHandler;
        }

        [ProducesResponseType(typeof(List<ShopViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("[action]")]
        [HttpPost]
        public ActionResult<IEnumerable<ShopViewModel>> Sorted([FromBody] UserLocationViewModel userLocation)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(
                    _errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation),
                    ModelState.Values.First().Errors.First().ErrorMessage));
            }

            var allShops = _shopRepository.GetSortedShops(userLocation.longitude, userLocation.latitude);

            if (allShops == null || allShops.Count() == 0)
            {
                return NoContent();
            }

            var viewModels = allShops.Select(shop => new ShopViewModel
            {
                Id = shop.Id,
                Name = shop.Name,
                latitude = shop.Location.Y,
                longitude = shop.Location.X
            });

            return Ok(viewModels);
        }

        [ProducesResponseType(typeof(List<ShopViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<ShopViewModel>> Get()
        {
            var allShops = _shopRepository.GetAll();

            if (allShops == null || allShops.Count() == 0)
            {
                return NoContent();
            }

            var viewModels = allShops.Select(shop => new ShopViewModel{
                Id = shop.Id,
                Name = shop.Name,
                latitude = shop.Location.Y,
                longitude = shop.Location.X
            });

            return Ok(viewModels);
        }

        [ProducesResponseType(typeof(ShopViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("{id}")]
        public ActionResult<ShopViewModel> Get(int id)
        {
            var selectedShop = _shopRepository.Get(id);
            if (selectedShop == null)
            {
                return NoContent();
            }
            var viewModels =  new ShopViewModel
            {
                Id = selectedShop.Id,
                Name = selectedShop.Name,
                latitude = selectedShop.Location.Y,
                longitude = selectedShop.Location.X
            };
            return Ok(viewModels);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public ActionResult Post([FromBody] ShopViewModel shop)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(
                    _errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation),
                    ModelState.Values.First().Errors.First().ErrorMessage));
            }

            _shopRepository.Add(new Shop
            {
                Name = shop.Name,
                Location = new Point(shop.longitude, shop.latitude)
                {
                    SRID = 4326
                }
            });

            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ShopViewModel shop)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(
                    _errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation),
                    ModelState.Values.First().Errors.First().ErrorMessage));
            }

            var selectedShop = _shopRepository.Get(id);
            if (selectedShop == null)
            {
                return NoContent();
            }

            selectedShop.Name = shop.Name;
            selectedShop.Location = new Point(shop.longitude, shop.latitude)
            {
                SRID = 4326
            };

            _shopRepository.Update(selectedShop);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var selectedShop = _shopRepository.Get(id);

            if (selectedShop == null)
            {
                return NoContent();
            }

            _shopRepository.Remove(selectedShop);

            return Ok();
        }
    }
}

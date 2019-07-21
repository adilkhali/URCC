using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnitedRemote.Core.Helpers;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories.Interfaces;
using UnitedRemote.Core.ViewModels;

namespace UnitedRemote.Web.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IErrorHandler _errorHandler;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public FavoriteController(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IFavoriteRepository favoriteRepository,
            IErrorHandler errorHandler,
            IShopRepository shopRepository
        ){
            _favoriteRepository = favoriteRepository;
            _shopRepository = shopRepository;
            _errorHandler = errorHandler;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        [ProducesResponseType(typeof(List<ShopViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopViewModel>>> Get()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var favoriteShops = await _favoriteRepository.GetFavoriteAsync(currentUser);

            if (favoriteShops == null || favoriteShops.Count() == 0)
            {
                return NoContent();
            }

            var viewModels = favoriteShops.Select(shop => new ShopViewModel
            {
                Id = shop.Id,
                Name = shop.Name,
                latitude = shop.Location.Y,
                longitude = shop.Location.X
            });

            return Ok(viewModels);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{id}")]
        public async Task<ActionResult> Post(int id)
        {
            var selectedShop = _shopRepository.Get(id);
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (selectedShop == null)
            {
                return NoContent();
            }

            var result = await _favoriteRepository.AddToFavoriteAsync(currentUser, selectedShop);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var selectedShop = _shopRepository.Get(id);
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (selectedShop == null)
            {
                return NoContent();
            }

            var result = await _favoriteRepository.RemoveFromFavoriteAsync(currentUser,id);

            return Ok();
        }
    }
}
using AutoMapper;
using KobaParts.Data.DatabaseContext;
using KobaParts.Exceptions;
using KobaParts.Models.Api;
using KobaParts.Models.Api.Client;
using KobaParts.Models.Api.Store;
using KobaParts.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace KobaParts.Services.FavoritesData
{
    public class FavoritesData : IFavoritesData
    {
        private readonly DataContext _context;
        private readonly Mapper _mapper;
        private readonly Mapper _requestMapper;
        public FavoritesData(DataContext dataContext)
        {
            _context = dataContext;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>().AfterMap((s, d) =>
            {
                if (s != null)
                {
                    if (s.Role != null)
                    {
                        d.Role = s.Role;
                    }
                }
            }));
            _mapper = new Mapper(config);

            var requestConfig = new MapperConfiguration(cfg => cfg.CreateMap<FavoritesProduct, FavoritesProductDto>()
            .ForMember("User", opt => opt.MapFrom(scr => _mapper.Map<User, UserDto>(scr.User)))
            );
            _requestMapper = new Mapper(requestConfig);
        }

        public async Task<BaseResponse<FavoritesProductDto>> GetFavorites(string? phoneNumber, int? articul)
        {
            try
            {
                if (phoneNumber == null || articul == null)
                    throw new BadRequestException();

                IQueryable<FavoritesProduct> request = _context.Favorties
                    .Include(t => t.User)
                    .Include(t => t.User.Role)
                    .Include(t => t.Product);

                request = request.Where(u => u.User.PhoneNumber == phoneNumber && u.Product.Articul == articul);

                var listFavorite = await request.ToListAsync();

                return new()
                {
                    Description = "Success",
                    StatusCode = 200,
                    TotalRecords = listFavorite.Count,
                    Values = _requestMapper.Map<List<FavoritesProduct>, List<FavoritesProductDto>>(listFavorite)
                };
            }
            catch (BadRequestException ex)
            {
                return new()
                {
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                    StatusCode = 409,
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                    StatusCode = 500,
                };
            }
        }


        public async Task<BaseResponse<FavoritesProductDto>> DeleteFavorites(string? phoneNumber, int? articul)
        {
            try
            {
                if (phoneNumber == null || articul == null)
                    throw new BadRequestException();

                IQueryable<FavoritesProduct> request = _context.Favorties
                    .Include(t => t.User)
                    .Include(t => t.User.Role)
                    .Include(t => t.Product);

                request = request.Where(t => t.User.PhoneNumber == phoneNumber && t.Product.Articul == articul);

                var list = await request.ToListAsync();
                var item = list[0];

                _context.Favorties.Remove(item);
                await _context.SaveChangesAsync();
                return new()
                {
                    Description = "Success",
                    StatusCode = 200,
                    TotalRecords = list.Count,
                    Values = _requestMapper.Map<List<FavoritesProduct>, List<FavoritesProductDto>>(list)
                };
            }
            catch (BadRequestException ex)
            {
                return new()
                {
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                    StatusCode = 409
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<BaseResponse<FavoritesProductDto>> AddFavorites(string? phoneNumber, int? articul)
        {
            try
            {
                if (phoneNumber == null || articul == null)
                    throw new BadRequestException();

                var user = (await _context.Users
                    .Include(t => t.Role)
                    .Where(u => u.PhoneNumber == phoneNumber)
                    .ToListAsync())[0];

                var product = (await _context.Products
                    .Where(p => p.Articul == articul)
                    .ToListAsync())[0];

                if (await IsExixstFavoriteAsync(user, product)) return new BaseResponse<FavoritesProductDto>
                {
                    StatusCode = 200,
                    Description = "Favorite already exists"
                };

                var favoriteProduct = new FavoritesProduct()
                {
                    Product = product,
                    User = user
                };

                await _context.Favorties.AddAsync(favoriteProduct);

                await _context.SaveChangesAsync();

                return new()
                {
                    Description = "Success",
                    StatusCode = 200,
                    TotalRecords = 1,
                    Values = new List<FavoritesProductDto> { _requestMapper.Map<FavoritesProduct, FavoritesProductDto>(favoriteProduct) }
                };
            }
            catch (BadRequestException ex)
            {
                return new()
                {
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                    StatusCode = 409
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                    StatusCode = 500
                };
            }
        }
        private async Task<bool> IsExixstFavoriteAsync(User user, Product product)
        {
            IQueryable<FavoritesProduct> exists = _context.Favorties
                    .Include(t => t.User)
                    .Include(t => t.User.Role)
                    .Include(t => t.Product);

            exists = exists.Where(t => t.User == user && t.Product == product);
            var list = await exists.ToListAsync();

            if (list.Count > 0) return true;

            return false;
        }
    }
}

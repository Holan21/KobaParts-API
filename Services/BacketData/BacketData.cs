using AutoMapper;
using KobaParts.Data.DatabaseContext;
using KobaParts.Exceptions;
using KobaParts.Models.Api;
using KobaParts.Models.Api.Client;
using KobaParts.Models.Api.Store;
using KobaParts.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace KobaParts.Services.BacketData
{
    public class BasketData : IBasketData
    {
        private readonly DataContext _context;
        private readonly Mapper _mapper;
        private readonly Mapper _requestMapper;

        public BasketData(DataContext context)
        {
            _context = context;
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

            var requestConfig = new MapperConfiguration(cfg => cfg.CreateMap<Purchase, PurchaseDto>()
            .ForMember("User", opt => opt.MapFrom(scr => _mapper.Map<User, UserDto>(scr.User)))
            );
            _requestMapper = new Mapper(requestConfig);
        }


        public async Task<BaseResponse<PurchaseDto>> GetBasket(string? phoneNumber)
        {
            try
            {
                IQueryable<Purchase> basketSqlRequest = _context.Backets
                    .Include(t => t.User)
                    .Include(t => t.User.Role)
                    .Include(t => t.Product);

                if (phoneNumber == null)
                    throw new BadRequestException("User is null");

                basketSqlRequest = basketSqlRequest.Where(b => b.User.PhoneNumber == phoneNumber);

                var purchaseList = await basketSqlRequest.ToListAsync();

                return new()
                {
                    Values = _requestMapper.Map<List<PurchaseDto>>(purchaseList),
                    TotalRecords = purchaseList.Count,
                    Description = "Success",
                    StatusCode = 200
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

        public async Task<BaseResponse<PurchaseDto>> AddOrderInBacket(int? articul, string? phoneNumber, int? count)
        {
            try
            {
                if (articul == null || phoneNumber == null || count == null)
                    throw new BadRequestException("Once or many paramaters is null");

                var user = (await _context.Users
                    .Include(t => t.Role)
                    .Where(u => u.PhoneNumber == phoneNumber)
                    .ToListAsync())[0];

                var product = (await _context.Products
                    .Where(p => p.Articul == articul)
                    .ToListAsync())[0];

                _context.Backets.Add(new Purchase()
                {
                    User = user,
                    Product = product,
                    Count = (int)count,
                });

                IQueryable<Purchase> purchases = _context.Backets
                    .Include(t => t.User)
                    .Include(t => t.User.Role)
                    .Include(t => t.Product);

                await _context.SaveChangesAsync();
                var purchase = (await purchases
                    .Where(p => p.User == user)
                    .Where(p => p.Product == product)
                    .Where(p => p.Count == count)
                    .ToListAsync())[0];

                return new()
                {
                    StatusCode = 200,
                    Description = "Success",
                    Values = new List<PurchaseDto>
                    {
                        _requestMapper.Map<PurchaseDto>(purchase)
                    },
                    TotalRecords = 1
                };
            }
            catch (BadRequestException ex)
            {
                return new()
                {
                    StatusCode = 409,
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    StatusCode = 500,
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                };
            }
        }


        public async Task<BaseResponse<PurchaseDto>> DeleteOrderFromBascket(int id)
        {
            try
            {
                var purchasse = await _context.Backets.FindAsync(id);
                if (purchasse == null)
                    throw new BadRequestException("Not found purchasse");

                _context.Backets.Remove(purchasse);
                await _context.SaveChangesAsync();
                return new()
                {
                    StatusCode = 200,
                    Description = "Success",
                };
            }
            catch (BadRequestException ex)
            {
                return new()
                {
                    StatusCode = 409,
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    StatusCode = 500,
                    Description = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                };
            }
        }
    }
}

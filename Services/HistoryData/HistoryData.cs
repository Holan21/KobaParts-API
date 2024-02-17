using AutoMapper;
using KobaParts.Data.DatabaseContext;
using KobaParts.Exceptions;
using KobaParts.Models.Api;
using KobaParts.Models.Api.Client;
using KobaParts.Models.Api.Store;
using KobaParts.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace KobaParts.Services.HistoryData
{
    public class HistoryData : IHistoryData
    {

        private readonly DataContext _context;
        private readonly Mapper _mapper;
        private readonly Mapper _requestMapper;
        public HistoryData(DataContext context)
        {
            _context = context;
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

            var requestConfig = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDto>()
            .ForMember("User", opt => opt.MapFrom(scr => _mapper.Map<User, UserDto>(scr.User)))
            );
            _requestMapper = new Mapper(requestConfig);
        }

        public async Task<BaseResponse<OrderDto>> GetHistoryByUser(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                    throw new BadRequestException();

                IQueryable<Order> request = _context.Orders
                    .Include(t => t.User)
                    .Include(t => t.User.Role)
                    .Include(t => t.Product);

                request = request.Where(t => t.User.PhoneNumber == phoneNumber);

                var list = await request.ToListAsync();

                return new()
                {
                    StatusCode = 200,
                    Description = "Success",
                    TotalRecords = list.Count,
                    Values = _requestMapper.Map<List<Order>, List<OrderDto>>(list)
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

        public async Task<BaseResponse<OrderDto>> AddOrderIntoHistory(string? phoneNumber, int? articul, int? count)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber) || articul == null || count == null)
                    throw new BadRequestException();

                var user = (await _context.Users
                    .Include(t => t.Role)
                    .Where(t => t.PhoneNumber == phoneNumber).ToListAsync())[0];

                var product = (await _context.Products
                    .Where(t => t.Articul == articul)
                    .ToListAsync())[0];

                var order = new Order()
                {
                    Count = (int)count,
                    Product = product,
                    User = user
                };

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                return new()
                {
                    StatusCode = 200,
                    Description = "Success",
                    TotalRecords = 1,
                    Values = new List<OrderDto> { _requestMapper.Map<Order, OrderDto>(order) }
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

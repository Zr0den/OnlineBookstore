using Microsoft.EntityFrameworkCore;
using OnlineBookstoreCore.Interfaces;
using OnlineBookstoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstoreApplication.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem)
        {
            return await _orderItemRepository.CreateAsync(orderItem);
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            await _orderItemRepository.DeleteAsync(id);
        }

        public async Task AddOrderItemsAsync(IEnumerable<OrderItem> orderItems)
        {
            await _orderItemRepository.AddOrderItemsAsync(orderItems);
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _orderItemRepository.GetByOrderIdAsync(orderId);
        }
    }
}

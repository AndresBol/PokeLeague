using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Infraestructure.Models;
using PokeLeague.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Implementations
{
    public class ServicePurchaseOrder : IServicePurchaseOrder
    {
        private readonly IRepositoryPurchaseOrder _repository;
        private readonly IMapper _mapper;

        public ServicePurchaseOrder(IRepositoryPurchaseOrder repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(PurchaseOrderDTO purchaseOrderDto)
        {
            var purchaseOrder = _mapper.Map<PurchaseOrder>(purchaseOrderDto);
            var id = await _repository.AddAsync(purchaseOrder);

            return id;
        }

        public async Task<PurchaseOrderDTO> FindByIdAsync(int id)
        {
            var purchaseOrder = await _repository.FindByIdAsync(id);
            var purchaseOrderDTO = _mapper.Map<PurchaseOrderDTO>(purchaseOrder);

            return purchaseOrderDTO;
        }

        public async Task<ICollection<PurchaseOrderDTO>> ListAsync()
        {
            var purchaseOrders = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PurchaseOrderDTO>>(purchaseOrders);

            return collection;
        }

        public async Task UpdateAsync(PurchaseOrderDTO purchaseOrderDto)
        {
            var purchaseOrder = _mapper.Map<PurchaseOrder>(purchaseOrderDto);
            purchaseOrder.Id = purchaseOrderDto.Id;
            await _repository.UpdateAsync(purchaseOrder);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

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
        private readonly IServiceAuction _serviceAuction;
        private readonly IRepositoryCard _repositoryCard;
        private readonly IMapper _mapper;

        public ServicePurchaseOrder(IRepositoryPurchaseOrder repository, IServiceAuction serviceAuction, IRepositoryCard repositoryCard, IMapper mapper)
        {
            _repository = repository;
            _serviceAuction = serviceAuction;
            _repositoryCard = repositoryCard;
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
            if (purchaseOrder == null)
                return null!;

            var purchaseOrderDTO = _mapper.Map<PurchaseOrderDTO>(purchaseOrder);
            purchaseOrderDTO.Status = ResolveStatus(purchaseOrderDTO);

            return purchaseOrderDTO;
        }

        public async Task<PurchaseOrderDTO?> FindByAuctionIdAsync(int auctionId)
        {
            var purchaseOrder = await _repository.FindByAuctionIdAsync(auctionId);
            if (purchaseOrder == null)
                return null;

            var purchaseOrderDTO = _mapper.Map<PurchaseOrderDTO>(purchaseOrder);
            purchaseOrderDTO.Status = ResolveStatus(purchaseOrderDTO);

            return purchaseOrderDTO;
        }

        public async Task<ICollection<PurchaseOrderDTO>> ListAsync()
        {
            var purchaseOrders = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PurchaseOrderDTO>>(purchaseOrders);

            foreach (var po in collection)
            {
                po.Status = ResolveStatus(po);
            }

            return collection;
        }

        public async Task<ICollection<PurchaseOrderDTO>> ListByUserIdAsync(int userId)
        {
            var purchaseOrders = await _repository.ListByUserIdAsync(userId);
            var collection = _mapper.Map<ICollection<PurchaseOrderDTO>>(purchaseOrders);

            foreach (var po in collection)
            {
                po.Status = ResolveStatus(po);
            }

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

        public async Task<int> RegisterPaymentForAuctionAsync(int auctionId)
        {
            var auction = await _serviceAuction.FindByIdAsync(auctionId);

            if (auction == null)
                throw new Exception("Auction not found.");

            if (auction.Status != "Finished")
                throw new Exception("Auction has not finished yet.");

            if (auction.AuctionBid == null || auction.AuctionBid.Count == 0)
                throw new Exception("Auction has no bids.");

            var existingPO = await _repository.FindByAuctionIdAsync(auctionId);
            if (existingPO != null)
                throw new Exception("A payment already exists for this auction.");

            var winner = auction.AuctionBid.OrderByDescending(b => b.BidAmount).First();

            var purchaseOrderDto = new PurchaseOrderDTO
            {
                AuctionId = auctionId,
                UserId = winner.UserId,
                PurchaseAmount = winner.BidAmount,
                PaymentDate = DateTime.Now,
                IsPaid = false,
                IsActive = true
            };

            try
            {
                return await AddAsync(purchaseOrderDto);
            }
            catch (Exception)
            {
                var created = await _repository.FindByAuctionIdAsync(auctionId);
                if (created != null)
                    throw new Exception("A payment already exists for this auction.");
                throw;
            }
        }

        public async Task ConfirmPaymentAsync(int id)
        {
            var purchaseOrder = await _repository.FindByIdAsync(id);
            if (purchaseOrder == null)
                throw new Exception($"PurchaseOrder with ID {id} not found.");

            var purchaseOrderDto = _mapper.Map<PurchaseOrderDTO>(purchaseOrder);
            purchaseOrderDto.IsPaid = true;

            await UpdateAsync(purchaseOrderDto);

            var card = await _repositoryCard.FindByIdAsync(purchaseOrder.Auction.CardId);
            if (card != null)
            {
                card.UserId = purchaseOrder.UserId;
                await _repositoryCard.UpdateAsync(card);
            }
        }

        private string ResolveStatus(PurchaseOrderDTO purchaseOrder)
        {
            return purchaseOrder.IsPaid ? "Confirmed" : "Pending";
        }
    }
}

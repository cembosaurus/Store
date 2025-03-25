using System;
using System.Threading.Tasks;
using AutoMapper;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Ordering.DTOs;
using Business.Payment.Http.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Ordering.Data.Repositories.Interfaces;
using Ordering.Tools.Interfaces;
using Services.Ordering.Models;
using Ordering.Services;


namespace Store.Test.Unit.Tests;

[TestFixture]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _mockOrderRepo;
    private Mock<ICartRepository> _mockCartRepo;
    private Mock<ICartItemsRepository> _mockCartItemsRepo;
    private Mock<IServiceResultFactory> _mockResultFact;
    private Mock<IMapper> _mockMapper;
    private Mock<IHttpAddressService> _mockHttpIdentityService;
    private Mock<IOrder> _mockOrderTools;
    private Mock<IHttpPaymentService> _mockHttpPaymentService;
    private Mock<IHttpContextAccessor> _mockAccessor;
    private OrderService _orderService;
    private IServiceResultFactory _serviceResultFactory;

    [SetUp]
    public void SetUp()
    {
        _mockOrderRepo = new Mock<IOrderRepository>();
        _mockCartRepo = new Mock<ICartRepository>();
        _mockCartItemsRepo = new Mock<ICartItemsRepository>();
        _mockResultFact = new Mock<IServiceResultFactory>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpIdentityService = new Mock<IHttpAddressService>();
        _mockOrderTools = new Mock<IOrder>();
        _mockHttpPaymentService = new Mock<IHttpPaymentService>();
        _mockAccessor = new Mock<IHttpContextAccessor>();
        _serviceResultFactory = new ServiceResultFactory();

        _orderService = new OrderService(
            _mockOrderRepo.Object,
            _mockCartRepo.Object,
            _mockCartItemsRepo.Object,
            _mockResultFact.Object,
            _mockMapper.Object,
            _mockHttpIdentityService.Object,
            _mockOrderTools.Object,
            _mockHttpPaymentService.Object,
            _mockAccessor.Object
        );
    }

    [Test]
    public async Task GetOrderByUserId_CartDoesNotExist_ReturnsError()
    {
        // Arrange
        int userId = 1;
        _mockCartRepo.Setup(repo => repo.GetCartIdByUserId(userId)).ReturnsAsync(Guid.Empty);

        _mockResultFact.Setup(factory => factory.Result<OrderReadDTO>(null, true, It.IsAny<string>()))
            .Returns(_serviceResultFactory.Result<OrderReadDTO>( null, true, $"Cart for Order with user Id '{userId}' does NOT exist !" ));

        // Act
        var result = await _orderService.GetOrderByUserId(userId);

        // Assert
        Assert.IsNull(result.Data);
        Assert.IsTrue(result.Status);
        Assert.AreEqual($"Cart for Order with user Id '{userId}' does NOT exist !", result.Message);
    }

    [Test]
    public async Task GetOrderByUserId_OrderNotFound_ReturnsMessage()
    {
        // Arrange
        int userId = 1;
        var cartId = Guid.NewGuid();
        _mockCartRepo.Setup(repo => repo.GetCartIdByUserId(userId)).ReturnsAsync(cartId);
        _mockOrderRepo.Setup(repo => repo.GetOrderByCartId(cartId)).ReturnsAsync((Order)null);

        _mockResultFact.Setup(factory => factory.Result<OrderReadDTO>(null, true, It.IsAny<string>()))
            .Returns(_serviceResultFactory.Result < OrderReadDTO >( null, true, $"Order '{userId}' was NOT found !" ));

        // Act
        var result = await _orderService.GetOrderByUserId(userId);

        // Assert
        Assert.IsNull(result.Data);
        Assert.IsTrue(result.Status);
        Assert.AreEqual($"Order '{userId}' was NOT found !", result.Message);
    }

    [Test]
    public async Task GetOrderByUserId_OrderExists_ReturnsOrder()
    {
        // Arrange
        int userId = 1;
        var cartId = Guid.NewGuid();
        var order = new Order();
        var orderDto = new OrderReadDTO();

        _mockCartRepo.Setup(repo => repo.GetCartIdByUserId(userId)).ReturnsAsync(cartId);
        _mockOrderRepo.Setup(repo => repo.GetOrderByCartId(cartId)).ReturnsAsync(order);
        _mockMapper.Setup(mapper => mapper.Map<OrderReadDTO>(order)).Returns(orderDto);

        _mockResultFact.Setup(factory => factory.Result(orderDto, true, ""))
            .Returns(_serviceResultFactory.Result(orderDto, true, "" ));

        // Act
        var result = await _orderService.GetOrderByUserId(userId);

        // Assert
        Assert.IsNotNull(result.Data);
        Assert.IsTrue(result.Status);
        Assert.IsEmpty(result.Message);
    }
}
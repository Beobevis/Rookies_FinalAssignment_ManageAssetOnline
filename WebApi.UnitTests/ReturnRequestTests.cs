using Xunit;
using Moq;
using System.Collections.Generic;
using WebApi.Entities;
using System;
using WebApi.Repositories;
using System.Threading.Tasks;
using WebApi.Services;
using System.Linq;
using AutoMapper;
using WebApi.Models.Assignments;
using WebApi.Helpers;
using WebApi.Models.ReturnRequest;

namespace WebApi.UnitTests;

public class ReturnServiceTests
{
    private readonly Mock<IReturnRepository> _returnRepositoryMock = new Mock<IReturnRepository>();
    private readonly IMapper _mapper;

    public ReturnServiceTests()
    {
        _returnRepositoryMock = new Mock<IReturnRepository>();
        if(_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(mc => 
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }

    [Fact]
    public Task GetAllReturnRequests_GetListOfReturnRequests_ReturnListOfReturnRequests()
    {
        // Arrange
        var returnRequests = GetSampleReturnRequests();
        _returnRepositoryMock.Setup(x => x.GetAllReturnRequests()).ReturnsAsync(returnRequests);
        var service = new ReturnService(_returnRepositoryMock.Object, _mapper);

        // Act
        var actionResult = service.GetAllReturnRequests();
        var actual = actionResult.Result as IEnumerable<ReturnRequestModel>;

        // Assert
        Assert.IsType<List<ReturnRequestModel>>(actual);
        Assert.IsNotType<ReturnRequestModel>(actual);
        Assert.Equal(3, actual.Count());
        Assert.Equal(GetSampleReturnRequests().Count(), actual.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetAllReturnRequests_EmptyList_ReturnListOfNone()
    {
        // Arrange
        var returnRequests = new List<ReturnRequest>();
        _returnRepositoryMock.Setup(x => x.GetAllReturnRequests()).ReturnsAsync(returnRequests);
        var service = new ReturnService(_returnRepositoryMock.Object, _mapper);

        // Act
        var actionResult = service.GetAllReturnRequests();
        var actual = actionResult.Result as IEnumerable<ReturnRequestModel>;

        // Assert
        Assert.IsType<List<ReturnRequestModel>>(actual);
        Assert.IsNotType<ReturnRequestModel>(actual);
        Assert.Equal(0, actual.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetReturnRequest_GetReturnRequestById_ReturnReturnRequest()
    {
        // Arrange
        var returnRequest = GetSampleReturnRequests().First();
        _returnRepositoryMock.Setup(x => x.GetReturnRequest(returnRequest.Id)).ReturnsAsync(returnRequest);
        var service = new ReturnService(_returnRepositoryMock.Object, _mapper);

        // Act
        var actionResult = service.GetReturnRequest(returnRequest.Id);
        var actual = actionResult.Result as ReturnRequestModel;

        // Assert
        Assert.IsType<ReturnRequestModel>(actual);
        Assert.Equal(returnRequest.Id, actual.Id);
        Assert.NotEqual(2, actual.Id);
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    public IEnumerable<ReturnRequest> GetSampleReturnRequests()
    {
        return new List<ReturnRequest>{
            new ReturnRequest{
                Id = 1,
                AssetId = 1,
                RequestedById = 2,
                AcceptedById = 1,
                AssignedDate = new DateTime(2022,01,01),
                ReturnedDate = new DateTime(2022,01,05),
                State = ReturnState.Completed,
                Location = "Test1",
            },
            new ReturnRequest{
                Id = 2,
                AssetId = 2,
                RequestedById = 3,
                AcceptedById = 1,
                AssignedDate = new DateTime(2022,01,01),
                ReturnedDate = new DateTime(2022,01,05),
                State = ReturnState.Completed,
                Location = "Test1",
            },
            new ReturnRequest{
                Id = 3,
                AssetId = 3,
                RequestedById = 1,
                AcceptedById = 1,
                AssignedDate = new DateTime(2022,01,01),
                ReturnedDate = new DateTime(2022,01,05),
                State = ReturnState.WaitingForReturn,
                Location = "Test1",
            },
        };
    }
}
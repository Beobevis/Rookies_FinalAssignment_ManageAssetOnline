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

namespace WebApi.UnitTests;

public class AssignmentServiceTests
{
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
    private readonly IMapper _mapper;
    public AssignmentServiceTests()
    {
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
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
    public Task GetAll_GetListOfAssignments_ReturnListOfAssignments()
    {
        // Arrange
        var assignments = GetSampleAssignments();
        _assignmentRepositoryMock.Setup(x => x.GetAllAssignments()).ReturnsAsync(assignments);
        var service = new AssignmentService(_assignmentRepositoryMock.Object, _mapper);
        
        // Act
        var actionResult = service.GetAllAssignments();
        var actual = actionResult.Result as IEnumerable<AssignmentModel>;

        // Assert
        Assert.IsType<List<AssignmentModel>>(actual);
        Assert.Equal(GetSampleAssignments().Count(), actual.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetAll_EmptyList_ReturnListOfNone()
    {
        // Arrange
        var assignments = new List<Assignment>();
        _assignmentRepositoryMock.Setup(x => x.GetAllAssignments()).ReturnsAsync(assignments);
        var service = new AssignmentService(_assignmentRepositoryMock.Object, _mapper);
        
        // Act
        var actionResult = service.GetAllAssignments();
        var actual = actionResult.Result as IEnumerable<AssignmentModel>;

        // Assert
        Assert.IsType<List<AssignmentModel>>(actual);
        Assert.Equal(0, actual.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }
/*
    [Fact]
    public Task GetAssignmentById_GetAssignmentByIdWrong_ReturnNoAssignment()
    {
        // Arrange
        var assignments = GetSampleAssignments();
        _assignmentRepositoryMock.Setup(x => x.GetAssignment(5)).ReturnsAsync(assignments.First(x => x.Id == 5));
        var service = new AssignmentService(_assignmentRepositoryMock.Object, _mapper);
        
        // Act
        var actionResult = service.GetAssignment(5);
        var actual = actionResult.Result as AssignmentModel;

        // Assert
        ArgumentException ex = Assert.Throws<ArgumentException>();
        Assert.IsNotType<AssignmentModel>(actual);
        Assert.IsNotType<List<AssignmentModel>>(actual);
        Assert.Equal("Assignment not found", AppException.Message);
        Assert.Null(actual);
        

        return Task.CompletedTask;
    }
*/
    [Fact]
    public Task GetAssignmentById_GetAssignmentById_ReturnAssignment()
    {
        // Arrange
        var assignment = GetSampleAssignments().First();
        _assignmentRepositoryMock.Setup(x => x.GetAssignment(assignment.Id)).ReturnsAsync(assignment);
        var service = new AssignmentService(_assignmentRepositoryMock.Object, _mapper);
        
        // Act
        var actionResult = service.GetAssignment(assignment.Id);
        var actual = actionResult.Result as AssignmentModel;

        // Assert
        Assert.IsType<AssignmentModel>(actual);
        Assert.Equal(assignment.Id, actual.Id);
        Assert.NotEqual(2, actual.Id);
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetAssignmentByUser_GetAssignmentByUser_ReturnAssignment()
    {
        // Arrange
        var assignments = GetSampleAssignments().Where(x => x.AssignToId == 1);
        _assignmentRepositoryMock.Setup(x => x.GetAssignmentsByUser(1)).ReturnsAsync(assignments);
        var service = new AssignmentService(_assignmentRepositoryMock.Object, _mapper);
        
        // Act
        var actionResult = service.GetAssignmentsByUser(1);
        var actual = actionResult.Result as List<AssignmentModel>;

        // Assert
        Assert.IsType<List<AssignmentModel>>(actual);
        Assert.Equal(assignments.Count(), actual?.Count());
        Assert.Equal(2, actual?.Count());
        Assert.NotEqual(3, actual?.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetAssignmentByUser_EmptyList_ReturnListOfNone()
    {
        // Arrange
        var assignments = new List<Assignment>();
        _assignmentRepositoryMock.Setup(x => x.GetAssignmentsByUser(1)).ReturnsAsync(assignments);
        var service = new AssignmentService(_assignmentRepositoryMock.Object, _mapper);
        
        // Act
        var actionResult = service.GetAssignmentsByUser(1);
        var actual = actionResult.Result as List<AssignmentModel>;

        // Assert
        Assert.IsType<List<AssignmentModel>>(actual);
        Assert.IsNotType<AssignmentModel>(actual);
        Assert.IsNotType<List<Assignment>>(actual);
        Assert.Equal(0, actual?.Count());
        Assert.Empty(actual);
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    public IEnumerable<Assignment> GetSampleAssignments()
    {
        return new List<Assignment>{
            new Assignment{
                Id = 1,
                AssetId = 1,
                AssignToId = 1,
                AssignedById = 1,
                AssignedDate = new DateTime(2022,01,01),
                Note = "Test",
                IsInProgress = true,
            },new Assignment{
                Id = 2,
                AssetId = 2,
                AssignToId = 1,
                AssignedById = 1,
                AssignedDate = new DateTime(2022,02,02),
                Note = "Test",
                IsInProgress = false,
            },
            new Assignment{
                Id = 3,
                AssetId = 2,
                AssignToId = 3,
                AssignedById = 1,
                AssignedDate = new DateTime(2022,03,03),
                Note = "Test",
                IsInProgress = true,
            },
        };
    }
}
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using new_backend.Controllers;
using new_backend.Models;
using new_backend.Services;

namespace NewBackendTest;

[TestClass]
public class FoodControllerTests
{
    private IFoodService foodService;
    private FoodController SUT;

    [TestInitialize]
    public void Setup()
    {
        foodService = A.Fake<IFoodService>();
        SUT = new FoodController(foodService);
    }

    [TestMethod]
    public async Task GetFoods_ReturnsOkWithFoodsFromFoodService()
    {
        // Arrange
        var expectedResult = A.CollectionOfDummy<Food>(2);
        A.CallTo(() => foodService.GetFoods(A<string>._)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetFoods();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetFoodById_ReturnsOkWithFoodFromFoodService()
    {
        // Arrange
        long foodId = 1;

        var expectedResult = A.Dummy<Food>();
        A.CallTo(() => foodService.GetFoodById(foodId)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetFoodById(foodId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task SaveFood_ReturnsOkWithFoodIdFromFoodService()
    {
        // Arrange
        var fakeFood = A.Dummy<Food>();
        fakeFood.Id = -1;

        long expectedResult = 1;
        A.CallTo(() => foodService.SaveFood(fakeFood)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.SaveFood(fakeFood);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().Be(expectedResult);
    }

    [TestMethod]
    public async Task DeleteFood_CallsDeleteFoodOnFoodServiceAndReturnsOk()
    {
        // Arrange
        long foodId = 1;

        // Act
        var result = await SUT.DeleteFood(foodId);

        // Assert
        A.CallTo(() => foodService.DeleteFood(foodId)).MustHaveHappenedOnceExactly();
        result.Should().BeOfType<OkResult>();
    }
}
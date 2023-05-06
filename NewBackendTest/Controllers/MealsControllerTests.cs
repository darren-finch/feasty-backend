using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using new_backend.Controllers;
using new_backend.Models;
using new_backend.Services;

namespace NewBackendTest;

[TestClass]
public class MealsControllerTests
{
    public IMealsService mealsService;
    public MealsController SUT;

    [TestInitialize]
    public void Setup()
    {
        mealsService = A.Fake<IMealsService>();
        SUT = new MealsController(mealsService);
    }

    [TestMethod]
    public async Task GetMeals_ReturnsOkWithMealsFromMealService()
    {
        // Arrange
        var titleQuery = "test";

        var expectedResult = A.CollectionOfDummy<Meal>(2);
        A.CallTo(() => mealsService.GetMeals(titleQuery)).Returns(expectedResult);

        // Act
        var result = await SUT.GetMeals(titleQuery);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetMealById_ReturnsOkWithMealFromMealService()
    {
        // Arrange
        var mealId = 1;

        var expectedResult = A.Dummy<Meal>();
        A.CallTo(() => mealsService.GetMealById(mealId)).Returns(expectedResult);

        // Act
        var result = await SUT.GetMealById(mealId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task SaveMeal_ReturnsOkWithMealIdFromMealService()
    {
        // Arrange
        var meal = A.Dummy<Meal>();

        var expectedResult = 1;
        A.CallTo(() => mealsService.SaveMeal(meal)).Returns(expectedResult);

        // Act
        var result = await SUT.SaveMeal(meal);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().Be(expectedResult);
    }

    [TestMethod]
    public async Task DeleteMeal_CallsDeleteMealOnMealServiceAndReturnsOk()
    {
        // Arrange
        var mealId = 1;

        // Act
        var result = await SUT.DeleteMeal(mealId);

        // Assert
        A.CallTo(() => mealsService.DeleteMeal(mealId)).MustHaveHappenedOnceExactly();
        result.Should().BeOfType<OkResult>();
    }
}
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using new_backend.Controllers;
using new_backend.Models;
using new_backend.Services;

namespace NewBackendTest;

[TestClass]
public class MealPlanMealsControllerTests
{
    private IMealPlanMealsService mealPlanMealService;
    private MealPlanMealsController SUT;

    [TestInitialize]
    public void Setup()
    {
        mealPlanMealService = A.Fake<IMealPlanMealsService>();
        SUT = new MealPlanMealsController(mealPlanMealService);
    }

    [TestMethod]
    public async Task SaveMealPlanMeal_CallsMealPlanMealServiceAndReturnsOk()
    {
        // Arrange
        var fakeMealPlanMeal = A.Dummy<MealPlanMeal>();

        // Act
        var result = await SUT.SaveMealPlanMeal(fakeMealPlanMeal);

        // Assert
        A.CallTo(() => mealPlanMealService.SaveMealPlanMeal(fakeMealPlanMeal)).MustHaveHappenedOnceExactly();
        result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public async Task DeleteMealPlanMeal_CallsMealPlanMealServiceAndReturnsOk()
    {
        // Arrange
        var mealId = 1;
        var mealPlanId = 2;

        // Act
        var result = await SUT.DeleteMealPlanMeal(mealId, mealPlanId);

        // Assert
        A.CallTo(() => mealPlanMealService.DeleteMealPlanMeal(mealId, mealPlanId)).MustHaveHappenedOnceExactly();
        result.Should().BeOfType<OkResult>();
    }
}
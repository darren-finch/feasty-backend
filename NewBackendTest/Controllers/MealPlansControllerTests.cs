using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using new_backend.Controllers;
using new_backend.Models;
using new_backend.Services;

namespace NewBackendTest;

[TestClass]
public class MealPlansControllerTests
{
    private IMealPlansService mealPlansService;

    private MealPlansController SUT;

    [TestInitialize]
    public void Setup()
    {
        mealPlansService = A.Fake<IMealPlansService>();
        SUT = new MealPlansController(mealPlansService);
    }

    // METHODS
    // Get Meal Plans (does not get meals for any meal plans)
    // Get Meal Plan By Id
    // Save Meal Plan (does not update meals)
    // Delete Meal Plan

    // TESTS
    // GetMealPlans_ReturnsOkWithMealPlansFromMealPlanService
    [TestMethod]
    public async Task GetMealPlans_ReturnsOkWithMealPlansFromMealPlanService()
    {
        // Arrange
        var expectedResult = A.CollectionOfDummy<MealPlan>(2);
        A.CallTo(() => mealPlansService.GetMealPlans()).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetMealPlans();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
    }

    // GetMealPlanById_ReturnsOkWithMealPlanFromMealPlanService
    [TestMethod]
    public async Task GetMealPlanById_ReturnsOkWithMealPlanFromMealPlanService()
    {
        // Arrange
        long mealPlanId = 1;

        var expectedResult = A.Dummy<MealPlan>();
        A.CallTo(() => mealPlansService.GetMealPlanById(mealPlanId)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetMealPlanById(mealPlanId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
    }

    // SaveMealPlan_ReturnsOkWithMealPlanIdFromMealPlanService
    [TestMethod]
    public async Task SaveMealPlan_ReturnsOkWithMealPlanIdFromMealPlanService()
    {
        // Arrange
        var mealPlan = A.Dummy<MealPlan>();

        var expectedResult = 1L;
        A.CallTo(() => mealPlansService.SaveMealPlan(mealPlan)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.SaveMealPlan(mealPlan);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().Be(expectedResult);
    }

    // DeleteMealPlan_CallsDeleteMealPlanOnMealPlanServiceAndReturnsOk
    [TestMethod]
    public async Task DeleteMealPlan_CallsDeleteMealPlanOnMealPlanServiceAndReturnsOk()
    {
        // Arrange
        long mealPlanId = 1;

        // Act
        var result = await SUT.DeleteMealPlan(mealPlanId);

        // Assert
        A.CallTo(() => mealPlansService.DeleteMealPlan(mealPlanId)).MustHaveHappenedOnceExactly();
        result.Should().BeOfType<OkResult>();
    }
}
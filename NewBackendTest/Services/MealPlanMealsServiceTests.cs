using FakeItEasy;
using FluentAssertions;
using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Repositories;
using new_backend.Services;

namespace NewBackendTest;

[TestClass]
public class MealPlanMealsServiceTests
{
    private IAuthManager authManager;
    private IMealsRepository mealsRepository;
    private IMealPlansRepository mealPlansRepository;
    private IMealPlanMealsRepository mealPlanMealsRepository;
    private MealPlanMealsService SUT;

    [TestInitialize]
    public void Setup()
    {
        authManager = A.Fake<IAuthManager>();
        A.CallTo(() => authManager.GetUserId()).Returns(GlobalTestData.USER_ID);

        mealsRepository = A.Fake<IMealsRepository>();
        mealPlansRepository = A.Fake<IMealPlansRepository>();

        mealPlanMealsRepository = A.Fake<IMealPlanMealsRepository>();
        A.CallTo(() => mealPlanMealsRepository.GetMealPlanMeal(A<long>._, A<long>._)).Returns((MealPlanMeal?)null);

        SUT = new MealPlanMealsService(mealsRepository, mealPlansRepository, mealPlanMealsRepository, authManager);
    }

    [TestMethod]
    public async Task SaveMealPlanMeal_GivenACombinationOfAMealIdAndAMealPlanIdThatAlreadyExists_DoesNothing()
    {
        // Arrange
        A.CallTo(() => mealPlanMealsRepository.GetMealPlanMeal(A<long>._, A<long>._)).Returns(A.Fake<MealPlanMeal>());

        // Act
        await SUT.SaveMealPlanMeal(A.Fake<MealPlanMeal>());

        // Assert
        A.CallTo(() => mealPlanMealsRepository.SaveMealPlanMeal(A<MealPlanMeal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMealPlanMeal_GivenAReferenceToAMealThatDoesNotExist_ThrowsANotFoundException()
    {
        // Arrange
        var mealId = 1;
        var mealPlanId = 2;

        var mealPlanMeal = A.Fake<MealPlanMeal>();
        A.CallTo(() => mealPlanMeal.MealId).Returns(mealId);
        A.CallTo(() => mealPlanMeal.MealPlanId).Returns(mealPlanId);

        A.CallTo(() => mealsRepository.GetMealById(mealId)).Returns((Meal?)null);
        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(A.Fake<MealPlan>());

        // Act
        Func<Task> actAsync = async () => await SUT.SaveMealPlanMeal(mealPlanMeal);

        // Assert
        await actAsync.Should().ThrowAsync<NotFoundException>();
        A.CallTo(() => mealPlanMealsRepository.SaveMealPlanMeal(A<MealPlanMeal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMealPlanMeal_GivenAReferenceToAMealPlanThatDoesNotExist_ThrowsANotFoundException()
    {
        // Arrange
        var mealId = 1;
        var mealPlanId = 2;

        var mealPlanMeal = A.Fake<MealPlanMeal>();
        A.CallTo(() => mealPlanMeal.MealId).Returns(mealId);
        A.CallTo(() => mealPlanMeal.MealPlanId).Returns(mealPlanId);

        A.CallTo(() => mealsRepository.GetMealById(mealId)).Returns(A.Fake<Meal>());
        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns((MealPlan?)null);

        // Act
        Func<Task> actAsync = async () => await SUT.SaveMealPlanMeal(mealPlanMeal);

        // Assert
        await actAsync.Should().ThrowAsync<NotFoundException>();
        A.CallTo(() => mealPlanMealsRepository.SaveMealPlanMeal(A<MealPlanMeal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMealPlanMeal_GivenAReferenceToAMealThatDoesNotBelongToTheUser_ThrowsAnUnauthorizedException()
    {
        // Arrange
        var mealId = 1;
        var meal = A.Fake<Meal>();
        A.CallTo(() => meal.Id).Returns(mealId);
        A.CallTo(() => meal.UserId).Returns(GlobalTestData.WRONG_USER_ID);
        A.CallTo(() => mealsRepository.GetMealById(mealId)).Returns(meal);

        var mealPlanId = 2;
        var mealPlan = A.Fake<MealPlan>();
        A.CallTo(() => mealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => mealPlan.UserId).Returns(GlobalTestData.USER_ID);
        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(mealPlan);

        var mealPlanMeal = A.Fake<MealPlanMeal>();
        A.CallTo(() => mealPlanMeal.MealId).Returns(mealId);
        A.CallTo(() => mealPlanMeal.MealPlanId).Returns(mealPlanId);

        // Act
        Func<Task> actAsync = async () => await SUT.SaveMealPlanMeal(mealPlanMeal);

        // Assert
        await actAsync.Should().ThrowAsync<UnauthorizedException>();
        A.CallTo(() => mealPlanMealsRepository.SaveMealPlanMeal(A<MealPlanMeal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMealPlanMeal_GivenAReferenceToAMealPlanThatDoesNotBelongToTheUser_ThrowsAnUnauthorizedException()
    {
        // Arrange
        var mealId = 1;
        var meal = A.Fake<Meal>();
        A.CallTo(() => meal.Id).Returns(mealId);
        A.CallTo(() => meal.UserId).Returns(GlobalTestData.USER_ID);
        A.CallTo(() => mealsRepository.GetMealById(mealId)).Returns(meal);

        var mealPlanId = 2;
        var mealPlan = A.Fake<MealPlan>();
        A.CallTo(() => mealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => mealPlan.UserId).Returns(GlobalTestData.WRONG_USER_ID);
        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(mealPlan);

        var mealPlanMeal = A.Fake<MealPlanMeal>();
        A.CallTo(() => mealPlanMeal.MealId).Returns(mealId);
        A.CallTo(() => mealPlanMeal.MealPlanId).Returns(mealPlanId);

        // Act
        Func<Task> actAsync = async () => await SUT.SaveMealPlanMeal(mealPlanMeal);

        // Assert
        await actAsync.Should().ThrowAsync<UnauthorizedException>();
        A.CallTo(() => mealPlanMealsRepository.SaveMealPlanMeal(A<MealPlanMeal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMealPlanMeal_GivenValidMealAndMealPlanReferences_CallsSaveMealPlanMealOnMealPlanMealRepository()
    {
        // Arrange
        var mealId = 1;
        var meal = A.Fake<Meal>();
        A.CallTo(() => meal.Id).Returns(mealId);
        A.CallTo(() => meal.UserId).Returns(GlobalTestData.USER_ID);
        A.CallTo(() => mealsRepository.GetMealById(mealId)).Returns(meal);

        var mealPlanId = 2;
        var mealPlan = A.Fake<MealPlan>();
        A.CallTo(() => mealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => mealPlan.UserId).Returns(GlobalTestData.USER_ID);
        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(mealPlan);

        var mealPlanMeal = A.Fake<MealPlanMeal>();
        A.CallTo(() => mealPlanMeal.MealId).Returns(mealId);
        A.CallTo(() => mealPlanMeal.MealPlanId).Returns(mealPlanId);

        // Act
        await SUT.SaveMealPlanMeal(mealPlanMeal);

        // Assert
        A.CallTo(() => mealPlanMealsRepository.SaveMealPlanMeal(mealPlanMeal)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task DeleteMealPlanMeal_GivenACombinationOfAMealIdAndAMealPlanIdThatDoesNotExist_ThrowsANotFoundException()
    {
        var mealId = 1;
        var mealPlanId = 2;

        A.CallTo(() => mealPlanMealsRepository.GetMealPlanMeal(mealId, mealPlanId)).Returns((MealPlanMeal?)null);

        // Act
        Func<Task> actAsync = async () => await SUT.DeleteMealPlanMeal(mealId, mealPlanId);

        // Assert
        await actAsync.Should().ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task DeleteMealPlanMeal_GivenAnIdOfAMealThatDoesNotBelongToTheUser_ThrowsAnUnauthorizedException()
    {
        var mealId = 1;
        var mealPlanId = 2;

        var existingMealPlanMeal = A.Fake<MealPlanMeal>();

        var existingMeal = A.Fake<Meal>();
        A.CallTo(() => existingMeal.UserId).Returns(GlobalTestData.WRONG_USER_ID);

        var existingMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => existingMealPlan.UserId).Returns(GlobalTestData.USER_ID);

        A.CallTo(() => existingMealPlanMeal.Meal).Returns(existingMeal);
        A.CallTo(() => existingMealPlanMeal.MealPlan).Returns(existingMealPlan);

        A.CallTo(() => mealPlanMealsRepository.GetMealPlanMeal(mealId, mealPlanId)).Returns(existingMealPlanMeal);

        // Act
        Func<Task> actAsync = async () => await SUT.DeleteMealPlanMeal(mealId, mealPlanId);

        // Assert
        await actAsync.Should().ThrowAsync<UnauthorizedException>();
    }

    [TestMethod]
    public async Task DeleteMealPlanMeal_GivenAnIdOfAMealPlanThatDoesNotBelongToTheUser_ThrowsAnUnauthorizedException()
    {
        var mealId = 1;
        var mealPlanId = 2;

        var existingMealPlanMeal = A.Fake<MealPlanMeal>();

        var existingMeal = A.Fake<Meal>();
        A.CallTo(() => existingMeal.UserId).Returns(GlobalTestData.USER_ID);

        var existingMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => existingMealPlan.UserId).Returns(GlobalTestData.WRONG_USER_ID);

        A.CallTo(() => existingMealPlanMeal.Meal).Returns(existingMeal);
        A.CallTo(() => existingMealPlanMeal.MealPlan).Returns(existingMealPlan);

        A.CallTo(() => mealPlanMealsRepository.GetMealPlanMeal(mealId, mealPlanId)).Returns(existingMealPlanMeal);

        // Act
        Func<Task> actAsync = async () => await SUT.DeleteMealPlanMeal(mealId, mealPlanId);

        // Assert
        await actAsync.Should().ThrowAsync<UnauthorizedException>();
    }

    [TestMethod]
    public async Task DeleteMealPlanMeal_GivenValidMealAndMealPlanReferences_CallsDeleteMealPlanMealOnMealPlanMealRepository()
    {
        var mealId = 1;
        var mealPlanId = 2;

        var existingMealPlanMeal = A.Fake<MealPlanMeal>();

        var existingMeal = A.Fake<Meal>();
        A.CallTo(() => existingMeal.UserId).Returns(GlobalTestData.USER_ID);

        var existingMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => existingMealPlan.UserId).Returns(GlobalTestData.USER_ID);

        A.CallTo(() => existingMealPlanMeal.Meal).Returns(existingMeal);
        A.CallTo(() => existingMealPlanMeal.MealPlan).Returns(existingMealPlan);

        A.CallTo(() => mealPlanMealsRepository.GetMealPlanMeal(mealId, mealPlanId)).Returns(existingMealPlanMeal);

        // Act
        await SUT.DeleteMealPlanMeal(mealId, mealPlanId);

        // Assert
        A.CallTo(() => mealPlanMealsRepository.DeleteMealPlanMeal(existingMealPlanMeal)).MustHaveHappenedOnceExactly();
    }
}
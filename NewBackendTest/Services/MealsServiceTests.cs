using FakeItEasy;
using FluentAssertions;
using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Repositories;
using new_backend.Services;

namespace NewBackendTest;

[TestClass]
public class MealsServiceTests
{
    private IAuthManager authManager;
    private IMealsRepository mealRepository;
    private IFoodRepository foodRepository;
    private MealsService SUT;

    [TestInitialize]
    public void Setup()
    {
        authManager = A.Fake<IAuthManager>();
        A.CallTo(() => authManager.GetUserId()).Returns(GlobalTestData.USER_ID);

        foodRepository = A.Fake<IFoodRepository>();
        mealRepository = A.Fake<IMealsRepository>();
        SUT = new MealsService(foodRepository, mealRepository, authManager);
    }

    [TestMethod]
    public async Task GetMeals_GivenNullTitleQuery_ReturnsAllMealsBelongingToUser()
    {
        // Arrange
        var expectedResult = A.CollectionOfDummy<Meal>(2);
        A.CallTo(() => mealRepository.GetAllMealsForUser(GlobalTestData.USER_ID)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetMeals(null);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetMeals_GivenEmptyTitleQuery_ReturnsAllMealsBelongingToUser()
    {
        // Arrange
        var expectedResult = A.CollectionOfDummy<Meal>(2);
        A.CallTo(() => mealRepository.GetAllMealsForUser(GlobalTestData.USER_ID)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetMeals("");

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetMeals_GivenANonEmptyTitleQuery_ReturnsMatchingMealsBelongingToUser()
    {
        // Arrange
        var titleQuery = "test";
        var expectedResult = A.CollectionOfDummy<Meal>(2);
        A.CallTo(() => mealRepository.GetMealsByTitleForUser(titleQuery, GlobalTestData.USER_ID)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetMeals(titleQuery);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetMealById_GivenAnIdOfAMealThatBelongsToTheUser_ReturnsTheMealWithThatId()
    {
        // Arrange
        var mealId = 1;
        var expectedResult = A.Dummy<Meal>();
        A.CallTo(() => expectedResult.UserId).Returns(GlobalTestData.USER_ID);
        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(expectedResult));

        // Act
        var result = await SUT.GetMealById(mealId);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public async Task GetMealById_GivenAnIdOfAMealThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var mealId = 1;
        var meal = A.Dummy<Meal>();
        A.CallTo(() => meal.UserId).Returns(GlobalTestData.WRONG_USER_ID);
        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(meal));

        // Act
        Func<Task> act = async () => await SUT.GetMealById(mealId);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [TestMethod]
    public async Task GetMealById_GivenAnIdOfAMealThatDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var mealId = 1;
        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(null));

        // Act
        Func<Task> act = async () => await SUT.GetMealById(mealId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task SaveMeal_GivenMealWithMealFoodThatReferencesAFoodThatDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var foodId = 1;

        var meal = A.Dummy<Meal>();
        var mealFood1 = A.Dummy<MealFood>();
        A.CallTo(() => mealFood1.FoodId).Returns(foodId);

        var listOfMealFoods = new List<MealFood> { mealFood1 };
        A.CallTo(() => meal.MealFoods).Returns(listOfMealFoods);

        var emptyListOfReferencedFoods = new List<Food>();
        A.CallTo(() => foodRepository.GetFoodsByIds(A<List<long>>._)).Returns(emptyListOfReferencedFoods);

        // Act
        Func<Task> act = async () => await SUT.SaveMeal(meal);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        A.CallTo(() => mealRepository.AddMeal(A<Meal>._)).MustNotHaveHappened();
        A.CallTo(() => mealRepository.UpdateMeal(A<Meal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMeal_GivenMealWithMealFoodThatReferencesAFoodThatBelongsToAnotherUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var foodId = 1;

        var meal = A.Dummy<Meal>();
        var mealFood1 = A.Dummy<MealFood>();
        A.CallTo(() => mealFood1.FoodId).Returns(foodId);

        var listOfMealFoods = new List<MealFood> { mealFood1 };
        A.CallTo(() => meal.MealFoods).Returns(listOfMealFoods);

        var food1 = A.Dummy<Food>();
        A.CallTo(() => food1.Id).Returns(foodId);
        A.CallTo(() => food1.UserId).Returns(GlobalTestData.WRONG_USER_ID);

        var listOfFoodsWithAFoodThatBelongsToAnotherUser = new List<Food> { food1 };
        A.CallTo(() => foodRepository.GetFoodsByIds(A<List<long>>._)).Returns(listOfFoodsWithAFoodThatBelongsToAnotherUser);

        // Act
        Func<Task> act = async () => await SUT.SaveMeal(meal);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
        A.CallTo(() => mealRepository.AddMeal(A<Meal>._)).MustNotHaveHappened();
        A.CallTo(() => mealRepository.UpdateMeal(A<Meal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMeal_GivenAMealWithValidMealFoodsAndANegativeId_AddsTheMeal()
    {
        // Arrange
        var meal = A.Dummy<Meal>();
        A.CallTo(() => meal.Id).Returns(-1);

        AddValidFakeMealFoodDataToFakeMeal(meal);

        // Act
        await SUT.SaveMeal(meal);

        // Assert
        A.CallTo(() => mealRepository.AddMeal(meal)).MustHaveHappenedOnceExactly();
        A.CallTo(() => mealRepository.UpdateMeal(A<Meal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMeal_GivenAMealWithValidMealFoodsAndAPositiveIdThatBelongsToTheUser_UpdatesTheMeal()
    {
        // Arrange
        var mealId = 1;

        var existingMeal = A.Fake<Meal>();
        A.CallTo(() => existingMeal.Id).Returns(mealId);
        A.CallTo(() => existingMeal.UserId).Returns(GlobalTestData.USER_ID);

        var newMeal = A.Dummy<Meal>();
        A.CallTo(() => newMeal.Id).Returns(mealId);
        A.CallTo(() => newMeal.Title).Returns("test");

        AddValidFakeMealFoodDataToFakeMeal(newMeal);

        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(existingMeal));

        // Act
        await SUT.SaveMeal(newMeal);

        // Assert
        A.CallToSet(() => existingMeal.Title).To(newMeal.Title).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingMeal.MealFoods).To(newMeal.MealFoods).MustHaveHappenedOnceExactly();
        A.CallTo(() => mealRepository.UpdateMeal(existingMeal)).MustHaveHappenedOnceExactly();
        A.CallTo(() => mealRepository.AddMeal(A<Meal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMeal_GivenAMealWithValidMealFoodsAndAPositiveIdThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var mealId = 1;

        var existingMeal = A.Fake<Meal>();
        A.CallTo(() => existingMeal.Id).Returns(mealId);
        A.CallTo(() => existingMeal.UserId).Returns(GlobalTestData.WRONG_USER_ID);

        var newMeal = A.Dummy<Meal>();
        A.CallTo(() => newMeal.Id).Returns(mealId);

        AddValidFakeMealFoodDataToFakeMeal(newMeal);

        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(existingMeal));

        // Act
        Func<Task> act = async () => await SUT.SaveMeal(newMeal);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
        A.CallTo(() => mealRepository.UpdateMeal(A<Meal>._)).MustNotHaveHappened();
        A.CallTo(() => mealRepository.AddMeal(A<Meal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveMeal_GivenAMealWithValidMealFoodsAndAPositiveIdThatDoesNotExist_ThrowsANotFoundException()
    {
        // Arrange
        var mealId = 1;

        var newMeal = A.Dummy<Meal>();
        A.CallTo(() => newMeal.Id).Returns(mealId);

        AddValidFakeMealFoodDataToFakeMeal(newMeal);

        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(null));

        // Act
        Func<Task> act = async () => await SUT.SaveMeal(newMeal);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        A.CallTo(() => mealRepository.UpdateMeal(A<Meal>._)).MustNotHaveHappened();
        A.CallTo(() => mealRepository.AddMeal(A<Meal>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task DeleteMeal_GivenAnIdOfAMealThatBelongsToTheUser_DeletesTheMeal()
    {
        // Arrange
        var mealId = 1;
        var meal = A.Dummy<Meal>();
        A.CallTo(() => meal.Id).Returns(mealId);
        A.CallTo(() => meal.UserId).Returns(GlobalTestData.USER_ID);
        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(meal));

        // Act
        await SUT.DeleteMeal(mealId);

        // Assert
        A.CallTo(() => mealRepository.DeleteMeal(meal)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task DeleteMeal_GivenAnIdOfAMealThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var mealId = 1;
        var meal = A.Dummy<Meal>();
        A.CallTo(() => meal.Id).Returns(mealId);
        A.CallTo(() => meal.UserId).Returns(GlobalTestData.WRONG_USER_ID);
        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(meal));

        // Act
        Func<Task> act = async () => await SUT.DeleteMeal(mealId);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
        A.CallTo(() => mealRepository.DeleteMeal(meal)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task DeleteMeal_GivenAnIdOfAMealThatDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var mealId = 1;

        A.CallTo(() => mealRepository.GetMealById(mealId)).Returns(Task.FromResult<Meal?>(null));

        // Act
        Func<Task> act = async () => await SUT.DeleteMeal(mealId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        A.CallTo(() => mealRepository.DeleteMeal(A<Meal>._)).MustNotHaveHappened();
    }

    // Adds valid meal food data to the meal passed in.
    private void AddValidFakeMealFoodDataToFakeMeal(Meal meal)
    {
        var foodId = 1;

        var mealFood1 = A.Dummy<MealFood>();
        A.CallTo(() => mealFood1.FoodId).Returns(foodId);

        var listOfValidMealFoods = new List<MealFood> { mealFood1 };
        A.CallTo(() => meal.MealFoods).Returns(listOfValidMealFoods);

        var food1 = A.Dummy<Food>();
        A.CallTo(() => food1.Id).Returns(foodId);
        A.CallTo(() => food1.UserId).Returns(GlobalTestData.USER_ID);

        var listOfFoods = new List<Food> { food1 };
        A.CallTo(() => foodRepository.GetFoodsByIds(A<List<long>>._)).Returns(listOfFoods);
    }
}
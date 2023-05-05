using FluentAssertions;
using FakeItEasy;
using new_backend.Models;
using new_backend.Repositories;
using new_backend.Services;
using new_backend.Exceptions;

namespace NewBackendTest;

[TestClass]
public class IFoodServiceTests
{
    private IAuthManager authManager;
    private IFoodRepository foodRepository;
    private IFoodService SUT;

    private const long USER_ID = 1;
    private const long WRONG_USER_ID = 2;

    [TestInitialize]
    public void Setup()
    {
        authManager = A.Fake<IAuthManager>();
        A.CallTo(() => authManager.GetUserId()).Returns(USER_ID);

        foodRepository = A.Fake<IFoodRepository>();
        SUT = new FoodService(foodRepository, authManager);
    }

    [TestMethod]
    public async Task GetFoods_GivenNoTitleQuery_ReturnsAllFoodsBelongingToUser()
    {
        // Arrange
        var expectedResult = A.CollectionOfDummy<Food>(2);
        A.CallTo(() => foodRepository.GetAllFoodsForUser(USER_ID)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetFoods();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetFoods_GivenATitleQuery_ReturnsMatchingFoodsBelongingToUser()
    {
        // Arrange
        var titleQuery = "test";
        var expectedResult = A.CollectionOfDummy<Food>(2);
        A.CallTo(() => foodRepository.GetFoodsByTitleForUser(titleQuery, USER_ID)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetFoods(titleQuery);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetFoodById_GivenAnIdOfAFoodThatBelongsToTheUser_ReturnsTheFoodWithThatId()
    {
        // Arrange
        var foodId = 1;

        var fakeFood = A.Fake<Food>();
        fakeFood.Id = foodId;
        fakeFood.UserId = USER_ID;

        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(fakeFood));

        // Act
        var result = await SUT.GetFoodById(foodId);

        // Assert
        result.Should().Be(fakeFood);
    }

    [TestMethod]
    public async Task GetFoodById_GivenAnIdOfAFoodThatDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var foodId = 1;

        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(null));

        // Act
        Func<Task> actAsync = async () => await SUT.GetFoodById(foodId);

        // Assert
        await actAsync.Should().ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task GetFoodById_GivenAnIdOfAFoodThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var foodId = 1;

        var fakeFood = A.Fake<Food>();
        fakeFood.Id = foodId;
        fakeFood.UserId = 2;

        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(fakeFood));

        // Act
        Func<Task> actAsync = async () => await SUT.GetFoodById(foodId);

        // Assert
        await actAsync.Should().ThrowAsync<UnauthorizedException>();
    }

    [TestMethod]
    public async Task SaveFood_GivenAFoodWithANegativeId_AddsTheFood()
    {
        // Arrange
        var fakeFood = A.Fake<Food>();
        fakeFood.Id = -1;

        // Act
        await SUT.SaveFood(fakeFood);

        // Assert
        A.CallTo(() => foodRepository.AddFood(fakeFood)).MustHaveHappenedOnceExactly();
        A.CallTo(() => foodRepository.UpdateFood(fakeFood)).MustNotHaveHappened();
    }

    // TODO: Find a way to clean up the attribute settings for the fake foods.
    [TestMethod]
    public async Task SaveFood_GivenAFoodWithAPositiveIdThatBelongsToTheUser_UpdatesTheFood()
    {
        // Arrange
        var foodId = 1;

        var existingFood = A.Fake<Food>();
        A.CallTo(() => existingFood.Id).Returns(foodId);
        A.CallTo(() => existingFood.UserId).Returns(USER_ID);

        var newFood = A.Fake<Food>();
        A.CallTo(() => newFood.Id).Returns(foodId);

        // Setting the user id to -1 so that it doesn't match the user id of the existing food.
        A.CallTo(() => newFood.UserId).Returns(-1);
        A.CallTo(() => newFood.Title).Returns("test");
        A.CallTo(() => newFood.Quantity).Returns(1);
        A.CallTo(() => newFood.Unit).Returns("test");
        A.CallTo(() => newFood.Calories).Returns(2);
        A.CallTo(() => newFood.Fats).Returns(3);
        A.CallTo(() => newFood.Carbs).Returns(4);
        A.CallTo(() => newFood.Proteins).Returns(5);

        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(existingFood));

        // Act
        await SUT.SaveFood(newFood);

        // Assert
        var calls = Fake.GetCalls(existingFood);
        A.CallToSet(() => existingFood.Title).To(newFood.Title).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingFood.Quantity).To(newFood.Quantity).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingFood.Unit).To(newFood.Unit).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingFood.Calories).To(newFood.Calories).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingFood.Fats).To(newFood.Fats).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingFood.Carbs).To(newFood.Carbs).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingFood.Proteins).To(newFood.Proteins).MustHaveHappenedOnceExactly();

        A.CallTo(() => foodRepository.UpdateFood(existingFood)).MustHaveHappenedOnceExactly();
        A.CallTo(() => foodRepository.AddFood(A<Food>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SaveFood_GivenAFoodWithAPositiveIdThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var foodId = 1;

        var existingFood = A.Fake<Food>();
        A.CallTo(() => existingFood.Id).Returns(foodId);
        A.CallTo(() => existingFood.UserId).Returns(WRONG_USER_ID);

        var newFood = A.Fake<Food>();
        A.CallTo(() => newFood.Id).Returns(foodId);

        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(existingFood));

        // Act
        Func<Task> actAsync = async () => await SUT.SaveFood(newFood);

        // Assert
        await actAsync.Should().ThrowAsync<UnauthorizedException>();
        A.CallTo(() => foodRepository.UpdateFood(A<Food>._)).MustNotHaveHappened();
        A.CallTo(() => foodRepository.AddFood(A<Food>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task DeleteFood_GivenAFoodIdForAFoodThatBelongsToTheUser_DeletesTheFood()
    {
        // Arrange
        var foodId = 1;
        var fakeFood = A.Fake<Food>();
        A.CallTo(() => fakeFood.UserId).Returns(USER_ID);
        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(fakeFood));

        // Act
        await SUT.DeleteFood(foodId);

        // Assert
        A.CallTo(() => foodRepository.DeleteFood(fakeFood)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task DeleteFood_GivenAFoodIdForAFoodThatDoesNotExist_ThrowsANotFoundException()
    {
        // Arrange
        var foodId = 1;
        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(null));

        Func<Task> actAsync = async () => await SUT.DeleteFood(foodId);

        // Act/Assert
        await actAsync.Should().ThrowAsync<NotFoundException>();

        A.CallTo(() => foodRepository.DeleteFood(A<Food>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task DeleteFood_GivenAFoodIdForAFoodThatDoesNotBelongToTheUser_ThrowsANotFoundException()
    {
        // Arrange
        var foodId = 1;

        var fakeFood = A.Fake<Food>();
        A.CallTo(() => fakeFood.UserId).Returns(WRONG_USER_ID);
        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(fakeFood));

        Func<Task> actAsync = async () => await SUT.DeleteFood(foodId);

        // Act/Assert
        await actAsync.Should().ThrowAsync<UnauthorizedException>();

        A.CallTo(() => foodRepository.DeleteFood(A<Food>._)).MustNotHaveHappened();
    }
}
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
    private IFoodRepository foodRepository;
    private IFoodService SUT;

    [TestInitialize]
    public void Setup()
    {
        foodRepository = A.Fake<IFoodRepository>();
        SUT = new FoodService(foodRepository);
    }

    [TestMethod]
    public async Task GetFoods_GivenNoTitleQuery_ReturnsAllFoods()
    {
        // Arrange
        var expectedResult = A.CollectionOfDummy<Food>(2);
        A.CallTo(() => foodRepository.GetAllFoodsForCurrentUser()).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetFoods();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetFoods_GivenATitleQuery_ReturnsFoodsWithTitlesThatMatchTheQuery()
    {
        // Arrange
        var fakeFood1 = A.Fake<Food>();
        var fakeFood2 = A.Fake<Food>();

        fakeFood1.Title = "Apple";
        fakeFood2.Title = "apple";

        var titleQuery = "apple";

        IList<Food> expectedResult = new List<Food> { fakeFood1, fakeFood2 };
        A.CallTo(() => foodRepository.FindFoodsForCurrentUser(titleQuery)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetFoods(titleQuery);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetFoodById_GivenAFoodId_ReturnsTheFoodWithThatId()
    {
        // Arrange
        Food? fakeFood = A.Fake<Food>();
        var foodId = 1;

        A.CallTo(() => foodRepository.GetFoodById(foodId)).Returns(Task.FromResult<Food?>(fakeFood));

        // Act
        var result = await SUT.GetFoodById(foodId);

        // Assert
        result.Should().Be(fakeFood);
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

    [TestMethod]
    public async Task SaveFood_GivenAFoodWithAPositiveId_UpdatesTheFood()
    {
        // Arrange
        var fakeFood = A.Fake<Food>();
        fakeFood.Id = 1;

        // Act
        await SUT.SaveFood(fakeFood);

        // Assert
        A.CallTo(() => foodRepository.UpdateFood(fakeFood)).MustHaveHappenedOnceExactly();
        A.CallTo(() => foodRepository.AddFood(fakeFood)).MustNotHaveHappened();
    }

    [TestMethod]
    public async Task DeleteFood_GivenAFoodIdForAFoodThatExists_DeletesTheFood()
    {
        // Arrange
        var foodId = 1;
        var fakeFood = A.Fake<Food>();
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
}
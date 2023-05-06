using FakeItEasy;
using FluentAssertions;
using new_backend.Exceptions;
using new_backend.Models;
using new_backend.Services;

namespace NewBackendTest;

[TestClass]
public class MealPlansServiceTests
{
    private IAuthManager authManager;
    private IMealPlansRepository mealPlansRepository;

    private MealPlansService SUT;

    [TestInitialize]
    public void Setup()
    {
        authManager = A.Fake<IAuthManager>();
        A.CallTo(() => authManager.GetUserId()).Returns(GlobalTestData.USER_ID);

        mealPlansRepository = A.Fake<IMealPlansRepository>();
        SUT = new MealPlansService(mealPlansRepository, authManager);
    }

    [TestMethod]
    public async Task GetMealPlans_ReturnsMealPlansFromMealPlanRepository()
    {
        // Arrange
        var expectedResult = A.CollectionOfDummy<MealPlan>(2);
        A.CallTo(() => mealPlansRepository.GetMealPlansForUser(GlobalTestData.USER_ID)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.GetMealPlans();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetMealPlanById_GivenAnIdOfAMealPlanThatBelongsToTheUser_ReturnsTheMealPlanWithThatId()
    {
        // Arrange
        long mealPlanId = 1;

        var expectedResult = A.Dummy<MealPlan>();
        A.CallTo(() => expectedResult.UserId).Returns(GlobalTestData.USER_ID);

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(expectedResult));

        // Act
        var result = await SUT.GetMealPlanById(mealPlanId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetMealPlanById_GivenAnIdOfAMealPlanThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        long mealPlanId = 1;

        var expectedResult = A.Dummy<MealPlan>();
        A.CallTo(() => expectedResult.UserId).Returns(GlobalTestData.WRONG_USER_ID);

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(expectedResult));

        // Act
        Func<Task> action = async () => await SUT.GetMealPlanById(mealPlanId);

        // Assert
        await action.Should().ThrowAsync<UnauthorizedException>();
    }

    [TestMethod]
    public async Task GetMealPlanById_GivenAnIdOfAMealPlanThatDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        long mealPlanId = 1;

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(null));

        // Act
        Func<Task> action = async () => await SUT.GetMealPlanById(mealPlanId);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task SaveMealPlan_GivenAMealPlanWithANegativeId_AddsMealPlan()
    {
        // Arrange
        var mealPlan = A.Fake<MealPlan>();
        A.CallTo(() => mealPlan.Id).Returns(-1);

        var expectedResult = 1L;
        A.CallTo(() => mealPlansRepository.AddMealPlan(mealPlan)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.SaveMealPlan(mealPlan);

        // Assert
        A.CallToSet(() => mealPlan.UserId).To(GlobalTestData.USER_ID).MustHaveHappenedOnceExactly();

        A.CallTo(() => mealPlansRepository.AddMealPlan(mealPlan)).MustHaveHappenedOnceExactly();
        A.CallTo(() => mealPlansRepository.UpdateMealPlan(mealPlan)).MustNotHaveHappened();

        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public async Task SaveMealPlan_GivenAMealPlanWithAPositiveIdThatBelongsToTheUser_UpdatesMealPlan()
    {
        // Arrange
        var mealPlanId = 1L;

        var existingMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => existingMealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => existingMealPlan.UserId).Returns(GlobalTestData.USER_ID);

        var newMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => newMealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => newMealPlan.Title).Returns("test");
        A.CallTo(() => newMealPlan.RequiredCalories).Returns(1);
        A.CallTo(() => newMealPlan.RequiredFats).Returns(2);
        A.CallTo(() => newMealPlan.RequiredCarbs).Returns(3);
        A.CallTo(() => newMealPlan.RequiredProteins).Returns(4);

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(existingMealPlan));

        var expectedResult = mealPlanId;
        A.CallTo(() => mealPlansRepository.UpdateMealPlan(newMealPlan)).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await SUT.SaveMealPlan(newMealPlan);

        // Assert
        A.CallToSet(() => existingMealPlan.Title).To(newMealPlan.Title).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingMealPlan.RequiredCalories).To(newMealPlan.RequiredCalories).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingMealPlan.RequiredFats).To(newMealPlan.RequiredFats).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingMealPlan.RequiredCarbs).To(newMealPlan.RequiredCarbs).MustHaveHappenedOnceExactly();
        A.CallToSet(() => existingMealPlan.RequiredProteins).To(newMealPlan.RequiredProteins).MustHaveHappenedOnceExactly();

        A.CallTo(() => mealPlansRepository.AddMealPlan(newMealPlan)).MustNotHaveHappened();
        A.CallTo(() => mealPlansRepository.UpdateMealPlan(newMealPlan)).MustHaveHappenedOnceExactly();

        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public async Task SaveMealPlan_GivenAMealPlanWithAPositiveIdThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var mealPlanId = 1L;

        var existingMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => existingMealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => existingMealPlan.UserId).Returns(GlobalTestData.WRONG_USER_ID);

        var newMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => newMealPlan.Id).Returns(mealPlanId);

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(existingMealPlan));

        // Act
        Func<Task> action = async () => await SUT.SaveMealPlan(newMealPlan);

        // Assert
        await action.Should().ThrowAsync<UnauthorizedException>();
    }

    [TestMethod]
    public async Task SaveMealPlan_GivenAMealPlanWithAPositiveIdThatDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var mealPlanId = 1L;

        var newMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => newMealPlan.Id).Returns(mealPlanId);

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(null));

        // Act
        Func<Task> action = async () => await SUT.SaveMealPlan(newMealPlan);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [TestMethod]
    public async Task DeleteMealPlan_GivenAnIdOfAMealPlanThatBelongsToTheUser_DeletesMealPlan()
    {
        // Arrange
        long mealPlanId = 1;

        var existingMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => existingMealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => existingMealPlan.UserId).Returns(GlobalTestData.USER_ID);

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(existingMealPlan));

        // Act
        await SUT.DeleteMealPlan(mealPlanId);

        // Assert
        A.CallTo(() => mealPlansRepository.DeleteMealPlan(existingMealPlan)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task DeleteMealPlan_GivenAnIdOfAMealPlanThatDoesNotBelongToTheUser_ThrowsUnauthorizedException()
    {
        // Arrange
        long mealPlanId = 1;

        var existingMealPlan = A.Fake<MealPlan>();
        A.CallTo(() => existingMealPlan.Id).Returns(mealPlanId);
        A.CallTo(() => existingMealPlan.UserId).Returns(GlobalTestData.WRONG_USER_ID);

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(existingMealPlan));

        // Act
        Func<Task> action = async () => await SUT.DeleteMealPlan(mealPlanId);

        // Assert
        await action.Should().ThrowAsync<UnauthorizedException>();
    }

    [TestMethod]
    public async Task DeleteMealPlan_GivenAnIdOfAMealPlanThatDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        long mealPlanId = 1;

        A.CallTo(() => mealPlansRepository.GetMealPlanById(mealPlanId)).Returns(Task.FromResult<MealPlan?>(null));

        // Act
        Func<Task> action = async () => await SUT.DeleteMealPlan(mealPlanId);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
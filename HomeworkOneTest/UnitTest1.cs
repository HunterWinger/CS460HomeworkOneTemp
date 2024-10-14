using HomeworkOneProject;
using HomeworkOneProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using HomeworkOneProject.Models;

namespace HomeworkOneTest;

// This code was assisted by ChatGPT, an AI language model by OpenAI, 
// which provided guidance on testing in NUnit and installing Moq.
// ChatGPT helped explain testing using Moq to help test controllers and other features. The above lines are what it recommended to for citing 

[TestFixture]
public class TeamCreationTests
{

    // Create the controller and mock logger for our tests
    private HomeController _controller;
    private Mock<ILogger<HomeController>> _mockLogger;

    [SetUp]
    public void Setup()
    {
        // Assign the controller and mock logger
        _mockLogger = new Mock<ILogger<HomeController>>();
        _controller = new HomeController(_mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose of the controller when done for safety, rather this than pragma ignore warning
        _controller?.Dispose();
    }

    [Test]
    public void ReturnsTeamBuilderViewSuccess()
    {
        // Arrange -> provide info that would pass 
        var model = new TeamInfoModel
        {
            initialList = "Hunter \n Christian \n Bryce \n Stevie",
            teamSize = 2
        };

        // Act - > we want to go to our TeamBuilder page from the controller, and pass the model as constructed above
        var result = _controller.TeamBuilder(model) as ViewResult;

        // Assert
        // We should have a ViewResult
        Assert.That(result, Is.Not.Null, "Expected a ViewResult.");
        // It should be the correct view. i.e. TeamBuilder
        Assert.That(result.ViewName, Is.EqualTo("TeamBuilder"));
        // Our model type should match
        Assert.That(result.Model, Is.InstanceOf<TeamInfoModel>());
    }

    [Test]
    public void TeamBuilderIdentifiesMembersSuccess()
    {
        // Arrange - > create a mock TeamInfoModel
        var model = new TeamInfoModel
        {
            initialList = "Hunter \n Christian \n Bryce \n Stevie \n Alice \n Gabriel \n Rowan",
            teamSize = 3
        };

        // Act - > Go to TeamBuilder page from controller and pass model to simulate a real user
        var result = _controller.TeamBuilder(model) as ViewResult;

        // Assert - > We should have a memberList count of 7, taken in from our initial string from the text area
        Assert.That(((TeamInfoModel)result.Model).memberList.Count, Is.EqualTo(7));
    }

    [Test]
    public void BadNameIsValidated()
    {
        // Arrange - > create a mock TeamInfoModel, names have invalid characters
        var model = new TeamInfoModel
        {
            initialList = "Hunter[ \n Christian8 \n Bryce \n Alice \n Stevie05",
            teamSize = 2
        };

        // Act - > Try to go to TeamBuilder, but in reality we should be validated and refreshed on Index
        var result = _controller.TeamBuilder(model) as ViewResult;

        // Assert - > We should still be on Index, despite trying to submit the team since we had invalid names
        Assert.That(result.ViewName, Is.EqualTo("Index"));
    }

    [Test]
    public void BadTeamSizeIsValidated()
    {
        // Arrange - > Create a mock TeamInfoModel, invalid team size
        var model = new TeamInfoModel
        {
            initialList = "Hunter \n Christian \n Stevie \n Bryce",
            teamSize = 1
        };

        // Act - > Try to go to TeamBuilder, but in reality we should be validated and refreshed on Index
        var result = _controller.TeamBuilder(model) as ViewResult;

        // Assert - > We should still be on Index, despite trying to submit the team since we had invalid team size
        Assert.That(result.ViewName, Is.EqualTo("Index"));
    }

    [Test]
    public void TeamSizeWithOddNumber()
    {
        // Arrange - > Create a mock TeamInfoModel, should be valid
        var model = new TeamInfoModel
        {
            initialList = "Hunter \n Christian \n Bryce \n Stevie \n Alice \n Gabriel \n Rowan",
            teamSize = 2
        };

        // Act - > Go to TeamBuilder, we should have 4 teams, three teams of 2 and one team of one
        var result = _controller.TeamBuilder(model) as ViewResult;

        // Assert - > Check that there is 4 teams
        Assert.That(model.teamList.Count, Is.EqualTo(4));
    }

}
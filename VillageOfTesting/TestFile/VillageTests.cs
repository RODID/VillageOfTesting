using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageOfTesting;
using VillageOfTesting.Objects;
using Xunit;
using VillageOfTesting.OccupationActions;

namespace VillageOfTesting.Tests
{
    public class VillageTests
    {
        private Village village;

        public VillageTests() 
        {
            village = new Village();
        }


        [Fact]
        public void Village_AddWorker_ShouldAddWorkerToTheList()
        {
            // Arrange
            string workerName1 = "Worker1";
            string workerName2 = "Worker2";
            string workerName3 = "Worker3";
            string occupation = "farmer";

            // Act
            village.AddWorker(workerName1, occupation);
            village.AddWorker(workerName2, occupation);
            village.AddWorker(workerName3, occupation);

            // Assert
            Assert.Equal(3, village.Workers.Count);
            Assert.Equal(workerName1, village.Workers[0].Name);
            Assert.Equal(workerName2, village.Workers[1].Name);
            Assert.Equal(workerName3, village.Workers[2].Name);
            Assert.All(village.Workers, worker => Assert.Equal(occupation, worker.Occupation));
        }

        [Fact]
        public void AdvanceToNextDay_WithoutWorkers()
        {
            // Arrange
            var village = new Village();

            // Act
            village.Day();

            // Assert
            Assert.Equal(1, village.DaysGone);
            Assert.Empty(village.Workers);
            Assert.False(village.GameOver);
        }

        [Fact]
        public void AdvanceToNextDay_WithWorkersAndSufficientFood()
        {
            // Arrange
            village.Food = 10;
            village.AddWorker("Worker1", "farmer");
            village.AddWorker("Worker2", "lumberjack");

            // Act
            village.Day();

            // Assert
            Assert.Equal(1, village.DaysGone);
            Assert.All(village.Workers, worker => Assert.False(worker.Hungry));
            Assert.Equal(8, village.Food); // 2 arbetare åt för 2 mat 
        }

        [Fact]
        public void AdvanceToNextDay_WithWorkersAndInsufficientFood()
        {
            // Arrange
            village.Food = 1;
            village.AddWorker("Worker1", "farmer");
            village.AddWorker("Worker2", "lumberjack");

            // Act
            village.Day();

            // Assert
            Assert.Equal(1, village.DaysGone);
            Assert.True(village.Workers[0].Hungry || village.Workers[1].Hungry);
            Assert.Equal(0, village.Food); // All mat borde bli uppäten
        }

        [Fact]
        public void AdvanceToNextDay_WithHungryWorkers()
        {
            // Arrange
            village.Food = 0;
            village.AddWorker("Worker1", "farmer");
            village.AddWorker("Worker2", "lumberjack");

            // Act
            village.Day();
            village.Day();

            // Assert
            Assert.Equal(2, village.DaysGone);
            Assert.All(village.Workers, worker => Assert.True(worker.Hungry));
            Assert.All(village.Workers, worker => Assert.Equal(2, worker.DaysHungry));
        }

        [Fact]
        public void AdvanceToNextDay_WorkersDieOfHunger()
        {
            // Arrange
            village.Food = 0;
            village.AddWorker("Worker1", "farmer");
            village.AddWorker("Worker2", "lumberjack");

            // Act
            for (int i = 0; i < Worker.daysUntilStarvation + 1; i++)
            {
                village.Day();
            }

            // Assert
            Assert.All(village.Workers, worker => Assert.False(worker.Alive));
            Assert.True(village.GameOver);
        }

        [Fact]
        public void AddProject_SuccessfullyAddsProject()
        {
            // Arrange
            int initialWood = village.Wood;
            int initialMetal = village.Metal;
            string projectName = "House";

            // Act
            village.AddProject(projectName);

            // Assert
            Assert.True(village.Projects.Count > 0);
            Assert.True(village.Wood < initialWood);
            Assert.True(village.Metal < initialMetal);
        }
        [Fact]
        public void AddProject_InsufficientResources()
        {
            // Arrange
            int initialWood = village.Wood;
            int initialMetal = village.Metal;
            string projectName = "Castle";

            // Act
            village.AddProject(projectName);

            // Assert
            Assert.True(village.Projects.Count == 0);
            Assert.Equal(initialWood,   village.Wood);
            Assert.Equal(initialMetal, village.Metal);
        }

        [Fact]
        public void AddProject_CompleteProjectAffectsResourceProduction()
        {
            // Arrange
            string projectName = "Woodmill";
            int initialWoodPerDay = village.WoodPerDay;

            // Act
            village.AddProject(projectName);
            
            village.Projects[0].Complete();

            // Assert
            Assert.Equal(initialWoodPerDay + 1, village.WoodPerDay);
        }

    }
}

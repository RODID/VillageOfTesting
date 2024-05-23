using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageOfTesting;
using VillageOfTesting.Objects;
using VillageOfTesting.OccupationActions;

namespace VillageOfTesting.TestFile
{
    public class VillageTests
    {
        [TestFixture]
        public class VillageTEsts
        {
            private Village village;

            [SetUp]
            public void SetUp()
            {
                village = new Village();
            }

            [Test]
            public void Day_NoWorkers()
            {
                village.Food = 10;

                village.Day();
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace QuizinatorCore.Services
{
    [TestClass]
    public class RandomizerTests
    {
        [TestMethod]
        public void Get_1_random_number_from_a_list_with_only_number_7_should_return_7_in_a_list()
        {
            List<int> veryShortList = new List<int>() { 7 };
            Randomizer randomizer = new Randomizer();

            List<int> result = randomizer.GetRandom<int>(veryShortList, 1);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(7, result[0]);
        }

        [TestMethod]
        public void Get_5_random_strings_from_a_list_of_4_should_throw_error()
        {
            List<string> list = new List<string>() { "one", "two", "three", "four" };
            Randomizer randomizer = new Randomizer();

            List<string> result = randomizer.GetRandom<string>(list, 5);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(7, result[0]);
        }

    }
}

using BowlingGame;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace BowlingGameTest {
    [TestClass]
    public class GameTests {

        Game g;

        [TestInitialize]
        public void Initialize() {
            g = new Game();
        }

        [TestMethod]
        public void TestGutterGame() {
            //gutter game means 0 pins every time
            rollMany(20, 0);
            Assert.AreEqual(0, g.getScore());
        }

        [TestMethod]
        public void TestSinglePinGame() {
            //every ball roll hits exactly 1 pin
            rollMany(20, 1);
            Assert.AreEqual(20, g.getScore());
        }

        [TestMethod]
        public void TestOneSpare() {
            g.roll(5);
            g.roll(5); //spare
            g.roll(3);
            rollMany(17, 0);
            Assert.AreEqual(16, g.getScore());
        }

        [TestMethod]
        public void TestAllStrikes() {
            // a perfect game has 12 strikes
            rollMany(12, 10);
            Assert.AreEqual(300, g.getScore());
        }

        [TestMethod]
        public void TestAllSpares() {
            int numFrames = 10;
            //the first roll in each frame plus the last roll completely determines an all-spare game
            int[] firstRolls = { 1, 3, 8, 7, 6, 2, 0, 5, 9, 4 };
            int lastRoll = 2;
            //roll and add bonuses to score
            for (int i = 0; i < numFrames; i++) {
                g.roll(firstRolls[i]);
                g.roll(10 - firstRolls[i]); //spare (yay!)
                //every first roll of every frame but the first frame is counted twice for bonus.
                //Instead of if statement for first frame, we will subtract out first frame later
            }
            g.roll(lastRoll);
            //correct for first frame first bowl no bonus and add in last roll with bonus
            //(1 + 9) + (3 + 7) + (8 + 2) + (7 + 3) + (6 + 4) + (2 + 8) + (0 + 10) + (5 + 5) + (9 + 1) + (4 + 6 + 2)
            //        +  3      +  8      +  7      +  6      +  2      +  0       +  5      +  9      +  4
            //  = 102 + 44 = 146
            Assert.AreEqual(146, g.getScore());
        }

        [TestMethod]
        public void TestGutterThenTurkey() {
            rollMany(18, 0);
            g.roll(10);
            g.roll(10);
            g.roll(10);
            Assert.AreEqual(30, g.getScore());
        }

        [TestMethod]
        public void TestThreeStrikes() {
            g.roll(10);
            g.roll(10);
            g.roll(10);
            rollMany(14, 0);
            Assert.AreEqual(30 + 20 + 10, g.getScore());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestNegativeRoll() {
            g.roll(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestLargeRoll() {
            g.roll(11);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestLargeRolls() {
            g.roll(6);
            g.roll(5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestLargeRollsInFirstTwoOfLastFrame() {
            rollMany(18, 0);
            g.roll(6);
            g.roll(5);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestTooManyBowlsInLastFrame() {
            rollMany(18, 0);
            g.roll(1);
            g.roll(1);
            g.roll(1);
        }

        [TestMethod]
        public void TestThreeBowlsInLastFrameWithStrike() {
            rollMany(18, 0);
            g.roll(10);
            g.roll(5);
            g.roll(5);
            Assert.AreEqual(20, g.getScore());
        }

        [TestMethod]
        public void TestThreeBowlsInLastFrameWithSpare() {
            rollMany(18, 0);
            g.roll(4);
            g.roll(6);
            g.roll(5);
            Assert.AreEqual(15, g.getScore());
        }

        private void rollMany(int numRolls, int const_pins) {
            //knock over the same number of pins with every ball
            for (int i = 0; i < numRolls; i++) {
                g.roll(const_pins);
            }
        }
    }
}

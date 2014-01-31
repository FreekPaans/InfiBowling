using System;
using BestBowlingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingGame.UnitTests
{
    [TestClass]
    public class AlgemeneTests
    {
        [TestMethod]
        public void NewGameShouldSetCurrentScore0()
        {
            var game = new Game();

            Assert.AreEqual(0, game.CurrentScore);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowLessThanZeroShouldGiveException()
        {
            var game = new Game();

            game.Gooi(-1);
        }

        [TestMethod]
        public void Throw1PinsShouldSetScore1()
        {
            var game = new Game();

            game.Gooi(1);

            Assert.AreEqual(1, game.CurrentScore);
        }

        [TestMethod]
        public void Throw2PinsShouldSetScore2()
        {
            var game = new Game();

            game.Gooi(2);

            Assert.AreEqual(2, game.CurrentScore);
        }

        [TestMethod]
        public void Throw2ConsecutiveRegularScoresShouldSum()
        {
            var game = new Game();

            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(2, game.CurrentScore);
        }

        [TestMethod]
        public void NoThrowsGivesScoreForFrame1Is0()
        {
            var game = new Game();

            Assert.AreEqual(0, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void Throw1PinsGivesScoreForFrame1Is1()
        {
            var game = new Game();

            game.Gooi(1);

            Assert.AreEqual(1, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowTwoConsecutive1PinsGivesScoreForFrame1Is2()
        {
            var game = new Game();

            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(2, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowThreeConsecutive1PinsGivesScoreForFrame2Is3()
        {
            var game = new Game();

            game.Gooi(1);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(3, game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void ThrowThreeConsecutive1PinsGivesScoreForFrame1Is2()
        {
            var game = new Game();

            game.Gooi(1);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(2, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void FrameWithOneRegularThrowIsNotDone()
        {
            var frame = new Frame();

            frame.AddThrow(1);

            Assert.AreEqual(false, frame.IsDone());
        }

        [TestMethod]
        public void FrameWithTwoRegularThrowIsDone()
        {
            var frame = new Frame();

            frame.AddThrow(1);
            frame.AddThrow(1);

            Assert.AreEqual(true, frame.IsDone());
        }

        [TestMethod]
        public void FrameWithSpareThrowIsDone()
        {
            var frame = new Frame();

            frame.AddThrow(4);
            frame.AddThrow(6);

            Assert.AreEqual(true, frame.IsDone());
        }

        [TestMethod]
        public void FrameWithStrikeThrowIsDone()
        {
            var frame = new Frame();

            frame.AddThrow(10);

            Assert.AreEqual(true, frame.IsDone());
        }

        [TestMethod]
        public void ThrowOnlySpareShouldSetScoreTo10()
        {
            var game = new Game();

            game.Gooi(4);
            game.Gooi(6);

            Assert.AreEqual(10, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowSpareAnd1ShouldSetScoreForFrame1To11()
        {
            var game = new Game();

            game.Gooi(4);
            game.Gooi(6);
            game.Gooi(1);

            Assert.AreEqual(11, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void FrameWithStrikeIsStrike()
        {
            var frame = new Frame();

            frame.AddThrow(10);

            Assert.IsTrue(frame.IsStrike());
        }

        [TestMethod]
        public void FrameWithNoThrowsIsNotStrike()
        {
            var frame = new Frame();

            Assert.IsFalse(frame.IsStrike());
        }

        [TestMethod]
        public void FrameWithSpareIsSpare()
        {
            var frame = new Frame();

            frame.AddThrow(4);
            frame.AddThrow(6);

            Assert.IsTrue(frame.IsSpare());
        }

        [TestMethod]
        public void ThrowSpareAnd1And1ShouldSetScoreForFrame1To11()
        {
            var game = new Game();

            game.Gooi(4);
            game.Gooi(6);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(11, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowStrikeAnd1And1ShouldSetScoreForFrame1To12()
        {
            var game = new Game();

            game.Gooi(10);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(12, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void Throw2ConsecutiveStrikesAnd1ShouldSetScoreForFrame1To21()
        {
            var game = new Game();

            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(1);

            Assert.AreEqual(21, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowPerfectGameShouldSetScoreTo300()
        {
            var game = new Game();

            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);

//            Assert.AreEqual(300, game.ScoreVoorFrame(10));
            Assert.AreEqual(300, game.CurrentScore);
        }
    }
}

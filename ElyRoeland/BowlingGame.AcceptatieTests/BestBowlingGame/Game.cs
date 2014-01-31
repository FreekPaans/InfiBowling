using System;
using System.Collections.Generic;
using System.Linq;

namespace BestBowlingGame
{
    public class Game
    {
        private readonly List<Frame> _frames = new List<Frame>();

        private int CurrentFrameNumber
        {
            get
            {
                return _frames.Count - 1;
            }
        }

        public int CurrentScore
        {
            get
            {
                return Math.Min(ScoreVoorFrame(CurrentFrameNumber), 300);
            }
        }

        public Game()
        {
            _frames.Add(new Frame());
        }

        public void Gooi(int aantalPins)
        {
            if (aantalPins < 0)
            {
                throw new Exception("Minder dan 0 pins");
            }

            var currentFrame = GetCurrentFrame();

            if (currentFrame.IsDone())
            {
                currentFrame = AddNewFrame();
            }

            currentFrame.AddThrow(aantalPins);
        }

        public int ScoreVoorFrame(int frameNumber)
        {
            int currentFrameNumber = 1;
            int score = 0;

            foreach (var currentFrame in _frames)
            {
                score += CalculateScore(currentFrame, currentFrameNumber);

                if (currentFrameNumber == frameNumber)
                    break;

                currentFrameNumber++;
            }

            return score;
        }

        private int CalculateScore(Frame frame, int currentFrameNumber)
        {
            int score = frame.ScoreInFrame();

            if (frame.IsSpare())
            {
                score += CalculateSpareScore(currentFrameNumber);
            }
            else if (frame.IsStrike())
            {
                score += CalculateStrikeScore(currentFrameNumber);
            }

            return score;
        }

        private int CalculateSpareScore(int frameNumber)
        {
            int result = 0;
            var nextFrame = GetFrameForFrameNumber(frameNumber + 1);

            if (nextFrame != null)
            {
                result = nextFrame.FirstThrowScore();
            }

            return result;
        }

        private int CalculateStrikeScore(int frameNumber)
        {
            int result = 0;
            var nextFrame = GetFrameForFrameNumber(frameNumber + 1);

            if (nextFrame != null)
            {
                result = nextFrame.ScoreInFrame();

                // FIXME: leluk!
                if (nextFrame.IsStrike())
                {
                    var nextNextFrame = GetFrameForFrameNumber(frameNumber + 2);
                    if (nextNextFrame != null)
                    {
                        result += nextNextFrame.FirstThrowScore();
                    }
                }
            }

            return result;
        }

        private Frame AddNewFrame()
        {
            var frame = new Frame();

            _frames.Add(frame);

            return frame;
        }

        private Frame GetCurrentFrame()
        {
            return _frames[CurrentFrameNumber];
        }

        private Frame GetFrameForFrameNumber(int frameNumber)
        {
            Frame frame = null;

            if (frameNumber <= _frames.Count)
                frame = _frames[frameNumber - 1];

            return frame;
        }
    }
}

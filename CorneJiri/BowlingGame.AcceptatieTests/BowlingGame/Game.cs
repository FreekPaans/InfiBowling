using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingGame {

    public class Game {

        private bool CurrentThrowIsFirstInFrame {
            get {
                return CurrentFrame.Scores.Count == 1;
            }
        }

        private Frame CurrentFrame = new Frame { Nummer = 1 };

        private bool IsLastFrame {
            get {
                return CurrentFrame.Nummer == 10;
            }
        }

        private bool IsGameFinished {
            get {
                return IsLastFrame && 
                    (CurrentFrame.Scores.Count == 3 ||
                     (CurrentFrame.Scores.Count == 2 && CurrentFrame.Scores.Sum() < 10));
            }
        }

        private List<int> Throws = new List<int>();

        private int CurrentThrow {
            get {
                return this.Throws.Count;
            }
        }

        public void Gooi(int pins) {
            if (IsGameFinished) {
                throw new InvalidOperationException();
            }

            if (pins > 10 || pins < 0) {
                throw new InvalidPinCountException();
            }

            CurrentFrame.Scores.Add(pins);

            if (!IsLastFrame) {
                if (CurrentThrowIsFirstInFrame) {
                    if (pins == 10 && !IsLastFrame) {
                        StartNextFrame();
                    }
                }
                else {
                    if (!IsLastFrame && (pins + this.Throws.LastOrDefault() > 10)) {
                        throw new InvalidPinCountWithinFrameException();
                    }
                    else if (!IsLastFrame || this.Throws.Last() != 10) {
                        StartNextFrame();
                    }
                }
            }

            this.Throws.Add(pins);
        }

        private void StartNextFrame() {
            CurrentFrame.Nummer++;
            CurrentFrame.Scores = new List<int>();
        }

        public int CurrentScore {
            get {
                return ScoreVoorFrame(CurrentFrame.Nummer);
            }
        }
        
        private int GetThrow(int throwNummer) {
            if (this.Throws.Count <= throwNummer) {
                return 0;
            }
            return this.Throws[throwNummer];
        }

        public int ScoreVoorFrame(int frameNummer) {
            var currentFrame = 1;
            int totalScore = 0;
            var currentThrow = 0;

            while (currentFrame <= frameNummer) {
                var throwScore = this.GetThrow(currentThrow);
                if (throwScore == 10) { // strike
                    totalScore += this.GetTotalScoreStartingAtThrow(currentThrow, 2);
                    currentThrow++;
                }
                else if ((throwScore + this.GetThrow(currentThrow + 1)) == 10) { // spare
                    totalScore += this.GetTotalScoreStartingAtThrow(currentThrow, 2);
                    currentThrow += 2;
                }
                else { // normale worp
                    totalScore += this.GetTotalScoreStartingAtThrow(currentThrow, 1); 
                    currentThrow += 2;
                }
                currentFrame++;
            }            

            return totalScore;
        }

        private int GetTotalScoreStartingAtThrow(int startThrowNummer, int numberOfThrows) {
            var totalScore = 0;
            for (var i = 0; i <= numberOfThrows; i++) {
                totalScore += this.GetThrow(i + startThrowNummer);
            }
            return totalScore;
        }
       
    }

}

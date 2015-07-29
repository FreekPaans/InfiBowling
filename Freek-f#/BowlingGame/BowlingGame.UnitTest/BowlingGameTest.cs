using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingGame;

namespace BowlingGame.UnitTest {

    [TestClass]
    public class BowlingGameTest {
        
        [TestMethod()]
        public void GameTest1() {
            var game = GetGame();
            Assert.AreEqual(0, game.CurrentScore);
            game.Gooi(10);
            Assert.AreEqual(10, game.CurrentScore);
            Assert.AreEqual(10, game.ScoreVoorFrame(1));
            game.Gooi(10);
            Assert.AreEqual(30, game.CurrentScore);
            Assert.AreEqual(20, game.ScoreVoorFrame(1));
            Assert.AreEqual(30, game.ScoreVoorFrame(2));
            game.Gooi(3);
            Assert.AreEqual(39, game.CurrentScore);
            Assert.AreEqual(23, game.ScoreVoorFrame(1));
            Assert.AreEqual(36, game.ScoreVoorFrame(2));
            Assert.AreEqual(39, game.ScoreVoorFrame(3));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GameTestOngeldigeScoreNaLaatsteStrike() {
            var game = GetGame();
            for (int i = 0; i < 9; i++) {
                game.Gooi(0);
                game.Gooi(0);
            }
            game.Gooi(10);
            game.Gooi(6);
            game.Gooi(5);
            Assert.Fail();
        }

        // Infi acceptatie tests

        [TestMethod]
        public void gegeven_een_perfect_game_dan_is_de_score_300() {
            var game = GooiPerfectGame();

            Assert.AreEqual(300, game.CurrentScore);
        }

        [TestMethod]
        public void gegeven_een_perfect_game_dan_is_de_score_per_frame_30_60_etc() {
            var game = GooiPerfectGame();

            for (var i = 1; i <= 10; i++) {
                Assert.AreEqual(i * 30, game.ScoreVoorFrame(i));
            }
        }

        [TestMethod]
        public void gegeven_het_voorbeeld_spel_dan_is_de_score_133() {
            var game = GooiVoorbeeldSpel();

            Assert.AreEqual(133, game.CurrentScore);
        }

        [TestMethod]
        public void gegeven_het_voorbeeld_spel_dan_klopt_de_score_per_frame() {
            var game = GooiVoorbeeldSpel();

            for (var i = 1; i <= 10; i++) {
                Assert.AreEqual(ScorePerFrame[i], game.ScoreVoorFrame(i));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void gegeven_het_spel_is_afgelopen_dan_kunnen_we_niet_meer_gooien() {
            var game = GetAfgelopenSpel();
            game.Gooi(10);
        }

        private BowlingGame.Game GetAfgelopenSpel() {
            return GooiPerfectGame();
        }

        readonly static int[] VoorbeeldSpelScores = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
        readonly static Dictionary<int, int> ScorePerFrame = new Dictionary<int, int> {
			{1,5},
			{2,14},
			{3,29},
			{4,49},
			{5,60},
			{6,61},
			{7,77},
			{8,97},
			{9,117},
			{10,133}
		};

        private BowlingGame.Game GooiVoorbeeldSpel() {
            var game = GetGame();

            foreach (var score in VoorbeeldSpelScores) {
                game.Gooi(score);
            }

            return game;
        }

        private Game GooiPerfectGame() {
            var game = new BowlingGame.Game();
            for (var i = 0; i < 12; i++) {
                game.Gooi(10);
            }

            return game;
        }

        // Infi andere tests Freek

        [TestMethod]
        public void als_we_een_nieuwe_game_hebben_en_we_vragende_score_op_sdan_is_de_score_0() {
            var game = GetGame();

            Assert.AreEqual(0, game.CurrentScore);
        }

        private BowlingGame.Game GetGame() {
            return new BowlingGame.Game();
        }

        [TestMethod]
        public void als_we_een_worp_gegooid_met_5_pins_en_we_vragen_de_score_op_dan_is_de_current_score_5() {
            var game = GetGame();

            game.Gooi(5);

            Assert.AreEqual(5, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_worp_gegooid_met_7_pins_en_we_vragen_de_score_op_dan_is_de_current_score_7() {
            var game = GetGame();

            game.Gooi(7);

            Assert.AreEqual(7, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_2_ballen_gooien_dan_is_de_score_de_som_van_die_scores() {
            var game = GetGame();

            game.Gooi(6);
            game.Gooi(2);

            Assert.AreEqual(8, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_4_5_4_gooien_dan_is_de_score_van_frame_1_9() {
            var game = GetGame();
            game.Gooi(4);
            game.Gooi(5);
            game.Gooi(4);
            Assert.AreEqual(9, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_we_3_2_4_gooien_dan_is_de_score_van_frame_1_5() {
            var game = GetGame();
            game.Gooi(3);
            game.Gooi(2);
            game.Gooi(4);
            Assert.AreEqual(5, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_we_3_2_4_gooien_dan_is_de_score_van_frame_2_9() {
            var game = GetGame();
            game.Gooi(3);
            game.Gooi(2);
            game.Gooi(4);
            Assert.AreEqual(9, game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void als_we_een_spare_gooien_en_in_de_frame_daarna_3_dan_is_de_score_voor_frame_1_13() {
            var game = GetGame();
            game.Gooi(3);
            game.Gooi(7);
            game.Gooi(3);
            Assert.AreEqual(13, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_we_een_spare_gooien_en_in_de_frame_daarna_3_dan_is_de_current_score_16() {
            var game = GetGame();
            game.Gooi(3);
            game.Gooi(7);
            game.Gooi(3);
            Assert.AreEqual(16, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_3_7_7_3_5_gooien_dan_is_de_score_voor_frame_2_32() {
            var game = GetGame();
            game.Gooi(3);
            game.Gooi(7);
            game.Gooi(7);
            game.Gooi(3);
            game.Gooi(5);
            Assert.AreEqual(32, game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void als_we_3_7_7_3_5_gooien_dan_is_de_current_score_37() {
            var game = GetGame();
            game.Gooi(3);
            game.Gooi(7);
            game.Gooi(7);
            game.Gooi(3);
            game.Gooi(5);
            Assert.AreEqual(37, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_strike_gooien_en_dan_3_dan_is_de_score_voor_frame_1_13() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(3);
            Assert.AreEqual(13, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_we_een_strike_gooien_en_dan_0_en_dan_5_dan_is_de_score_voor_frame_2_5() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(0);
            game.Gooi(5);
            game.Gooi(5);
            Assert.AreEqual(20,game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void als_we_een_strike_gooien_en_dan_3_dan_is_de_current_score_16() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(3);
            Assert.AreEqual(16,game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_strike_gooien_en_dan_3_4_dan_is_de_score_voor_frame_1_17() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(3);
            game.Gooi(4);
            Assert.AreEqual(17,game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_we_een_strike_gooien_en_dan_3_4_dan_is_de_current_score_24() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(3);
            game.Gooi(4);
            Assert.AreEqual(24,game.CurrentScore);
        }

        [TestMethod]
        public void als_we_2_strikes_gooien_dan_is_de_score_voor_frame_1_20() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(10);
            Assert.AreEqual(20,game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_we_3_strikes_gooien_dan_is_de_score_voor_frame_1_30() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            Assert.AreEqual(30,game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_we_2_strikes_gooien_dan_is_de_current_score_30() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(10);
            Assert.AreEqual(30,game.CurrentScore);
        }

        [TestMethod]
        public void als_we_2_strikes_gooien_dan_is_de_score_voor_frame_2_30() {
            var game = GetGame();
            game.Gooi(10);
            game.Gooi(10);
            Assert.AreEqual(30,game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void als_we_een_strike_gooien_in_de_laatste_beurt_en_dan_gooien_we_nog_2_strikes_dan_is_de_current_score_30() {
            var game = GetGame();
            Gooi9Frames0(game);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            Assert.AreEqual(30,game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_spare_in_de_laatste_frame_gooien_en_dan_3_dan_is_de_current_score_13() {
            var game = GetGame();
            Gooi9Frames0(game);
            game.Gooi(3);
            game.Gooi(7);
            game.Gooi(3);
            Assert.AreEqual(13,game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_spare_in_de_laatste_frame_gooien_en_dan_3_dan_is_de_score_voor_frame_10_13() {
            var game = GetGame();
            Gooi9Frames0(game);
            game.Gooi(3);
            game.Gooi(7);
            game.Gooi(3);
            Assert.AreEqual(13,game.ScoreVoorFrame(10));
        }

        [TestMethod]
        public void als_we_een_spare_in_de_laatste_frame_gooien_en_dan_een_strike_dan_is_de_current_score_20() {
            var game = GetGame();
            Gooi9Frames0(game);
            game.Gooi(3);
            game.Gooi(7);
            game.Gooi(10);
            Assert.AreEqual(20,game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_8_frames_nul_gooien_dan_10_10_10_10_dan_is_de_current_score_60() {
            var game = GetGame();
            GooiXFrames0(game,8);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            Assert.AreEqual(60,game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_8_frames_nul_gooien_dan_10_10_10_10_dan_is_de_score_voor_frame_9_30_en_voor_frame_10_60() {
            var game = GetGame();
            GooiXFrames0(game,8);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);
            Assert.AreEqual(30,game.ScoreVoorFrame(9));
            Assert.AreEqual(60,game.ScoreVoorFrame(10));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void als_we_in_een_afgesloten_game_gooien_dan_krijgen_we_een_invalid_operation_exception() {
            var game = GetGame();
            GooiXFrames0(game,10);
            game.Gooi(5);
        }

        private void Gooi9Frames0(BowlingGame.Game game) {
            GooiXFrames0(game, 9);
        }

        private void GooiXFrames0(BowlingGame.Game game, int x) {
            for (var i = 0; i < (x * 2); i++) {
                game.Gooi(0);
            }
        }

        // Infi andere tests duo 2

        [TestMethod]
        public void NewGameShouldSetCurrentScore0() {
            var game = GetGame();

            Assert.AreEqual(0, game.CurrentScore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowLessThanZeroShouldGiveException() {
            var game = new BowlingGame.Game();

            game.Gooi(-1);
        }

        [TestMethod]
        public void Throw1PinsShouldSetScore1() {
            var game = new BowlingGame.Game();

            game.Gooi(1);

            Assert.AreEqual(1, game.CurrentScore);
        }

        [TestMethod]
        public void Throw2PinsShouldSetScore2() {
            var game = GetGame();

            game.Gooi(2);

            Assert.AreEqual(2, game.CurrentScore);
        }

        [TestMethod]
        public void Throw2ConsecutiveRegularScoresShouldSum() {
            var game = GetGame();

            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(2, game.CurrentScore);
        }

        [TestMethod]
        public void Throw1PinsGivesScoreForFrame1Is1() {
            var game = GetGame();

            game.Gooi(1);

            Assert.AreEqual(1, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowTwoConsecutive1PinsGivesScoreForFrame1Is2() {
            var game = GetGame();

            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(2, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowThreeConsecutive1PinsGivesScoreForFrame2Is3() {
            var game = GetGame();

            game.Gooi(1);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(3, game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void ThrowThreeConsecutive1PinsGivesScoreForFrame1Is2() {
            var game = GetGame();

            game.Gooi(1);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(2, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowOnlySpareShouldSetScoreTo10() {
            var game = GetGame();

            game.Gooi(4);
            game.Gooi(6);

            Assert.AreEqual(10, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowSpareAnd1ShouldSetScoreForFrame1To11() {
            var game = GetGame();

            game.Gooi(4);
            game.Gooi(6);
            game.Gooi(1);

            Assert.AreEqual(11, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowSpareAnd1And1ShouldSetScoreForFrame1To11() {
            var game = GetGame();

            game.Gooi(4);
            game.Gooi(6);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(11, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowStrikeAnd1And1ShouldSetScoreForFrame1To12() {
            var game = GetGame();

            game.Gooi(10);
            game.Gooi(1);
            game.Gooi(1);

            Assert.AreEqual(12, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void Throw2ConsecutiveStrikesAnd1ShouldSetScoreForFrame1To21() {
            var game = GetGame();

            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(1);

            Assert.AreEqual(21, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void ThrowPerfectGameShouldSetScoreTo300() {
            var game = GetGame();

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

        // Infi tests duo 3

        [TestMethod]
        public void als_we_een_nieuwe_game_maken_en_we_vragen_de_score_op_dan_is_de_score_0() {
            var game = GetGame();

            Assert.AreEqual(0, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_worp_gegooid_hebben_met_4_pins_en_we_vragen_de_score_op_dan_is_de_score_4() {
            var game = GetGame();

            game.Gooi(4);

            Assert.AreEqual(4, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_worp_gegooid_hebben_met_6_pins_en_we_vragen_de_score_op_dan_is_de_score_6() {
            var game = GetGame();

            game.Gooi(6);

            Assert.AreEqual(6, game.CurrentScore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void als_we_een_worp_gegooid_hebben_met_11_pins_dan_error() {
            var game = GetGame();

            game.Gooi(11);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void als_we_een_worp_gegooid_hebben_met_110_pins_dan_error() {
            var game = GetGame();

            game.Gooi(110);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void als_we_een_worp_gegooid_hebben_met_een_negatief_aantal_pins_dan_error() {
            var game = GetGame();

            game.Gooi(-1);
        }

        [TestMethod]
        public void als_we_twee_worpen_gooien_met_6_en_3_dan_is_de_score_9() {
            var game = GetGame();

            game.Gooi(6);

            // ? Hier nog assert dat score 6 is?

            game.Gooi(3);

            Assert.AreEqual(9, game.CurrentScore);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void als_de_eerste_twee_worpen_meer_dan_10_zijn_dan_error() {
            var game = GetGame();

            game.Gooi(6);

            // ? Hier nog assert dat score 6 is?

            game.Gooi(5);
        }

        [TestMethod]
        public void als_de_eerste_twee_worpen_totaal_9_pins_zijn_en_de_derde_worp_is_5_dan_is_de_score_14() {
            var game = GetGame();

            game.Gooi(6);

            // ? Hier nog assert dat score 6 is?

            game.Gooi(3);

            game.Gooi(5);

            Assert.AreEqual(14, game.CurrentScore);
        }

        [TestMethod]
        public void als_de_eerste_frame_een_spare_dan_is_de_frame_score_10() {
            var game = GetGame();

            game.Gooi(6);
            game.Gooi(4);

            Assert.AreEqual(10, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_de_eerste_frame_een_spare_en_de_3e_gooi_5_dan_is_de_frame_score_van_het_1e_frame_15() {
            var game = GetGame();

            game.Gooi(6);
            game.Gooi(4);

            game.Gooi(5);

            Assert.AreEqual(15, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_de_eerste_frame_een_spare_en_de_3e_gooi_5_dan_is_de_frame_score_van_het_2e_frame_20() {
            var game = GetGame();

            game.Gooi(6);
            game.Gooi(4);

            game.Gooi(5);

            Assert.AreEqual(20, game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void als_de_eerste_3_strikes_dan_is_de_frame_score_van_het_3e_frame_60() {
            var game = GetGame();

            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);

            Assert.AreEqual(60, game.CurrentScore);
        }

        [TestMethod]
        public void als_eerste_0_en_dan_10_en_dan_5_dan_hebben_we_20_punten() {
            var game = GetGame();

            game.Gooi(0);
            game.Gooi(10);

            game.Gooi(5);

            Assert.AreEqual(20, game.CurrentScore);
        }


        [TestMethod]
        public void als_de_eerste_3_spares_dan_is_de_frame_score_van_het_3e_frame_60() {
            var game = GetGame();

            game.Gooi(5);
            game.Gooi(5);

            game.Gooi(5);
            game.Gooi(5);

            game.Gooi(5);
            game.Gooi(5);

            Assert.AreEqual(40, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_perfect_game_gooien_is_de_score_300() {

            var game = GetGame();

            for (var i = 0; i < 12; i++) {
                game.Gooi(10);
            }

            Assert.AreEqual(300, game.CurrentScore);
        }


    }

}

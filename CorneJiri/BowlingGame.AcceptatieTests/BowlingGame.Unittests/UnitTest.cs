using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingGame.Unittests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void als_we_een_nieuwe_game_maken_en_we_vragen_de_score_op_dan_is_de_score_0()
        {
            var game = new Game();

            Assert.AreEqual(0, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_worp_gegooid_hebben_met_4_pins_en_we_vragen_de_score_op_dan_is_de_score_4()
        {
            var game = new Game();

            game.Gooi(4);

            Assert.AreEqual(4, game.CurrentScore);
        }

        [TestMethod]
        public void als_we_een_worp_gegooid_hebben_met_6_pins_en_we_vragen_de_score_op_dan_is_de_score_6()
        {
            var game = new Game();

            game.Gooi(6);

            Assert.AreEqual(6, game.CurrentScore);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPinCountException))]
        public void als_we_een_worp_gegooid_hebben_met_11_pins_dan_error()
        {
            var game = new Game();

            game.Gooi(11);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPinCountException))]
        public void als_we_een_worp_gegooid_hebben_met_110_pins_dan_error()
        {
            var game = new Game();

            game.Gooi(110);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPinCountException))]
        public void als_we_een_worp_gegooid_hebben_met_een_negatief_aantal_pins_dan_error()
        {
            var game = new Game();

            game.Gooi(-1);
        }

        [TestMethod]
        public void als_we_twee_worpen_gooien_met_6_en_3_dan_is_de_score_9()
        {
            var game = new Game();

            game.Gooi(6);

            // ? Hier nog assert dat score 6 is?

            game.Gooi(3);

            Assert.AreEqual(9, game.CurrentScore);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPinCountWithinFrameException))]
        public void als_de_eerste_twee_worpen_meer_dan_10_zijn_dan_error()
        {
            var game = new Game();

            game.Gooi(6);

            // ? Hier nog assert dat score 6 is?

            game.Gooi(5);
        }

        [TestMethod]
        public void als_de_eerste_twee_worpen_totaal_9_pins_zijn_en_de_derde_worp_is_5_dan_is_de_score_14()
        {
            var game = new Game();

            game.Gooi(6);

            // ? Hier nog assert dat score 6 is?

            game.Gooi(3);

            game.Gooi(5);

            Assert.AreEqual(14, game.CurrentScore);
        }

        [TestMethod]
        public void als_de_eerste_frame_een_spare_dan_is_de_frame_score_10()
        {
            var game = new Game();

            game.Gooi(6);
            game.Gooi(4);

            Assert.AreEqual(10, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_de_eerste_frame_een_spare_en_de_3e_gooi_5_dan_is_de_frame_score_van_het_1e_frame_15()
        {
            var game = new Game();

            game.Gooi(6);
            game.Gooi(4);

            game.Gooi(5);

            Assert.AreEqual(15, game.ScoreVoorFrame(1));
        }

        [TestMethod]
        public void als_de_eerste_frame_een_spare_en_de_3e_gooi_5_dan_is_de_frame_score_van_het_2e_frame_20()
        {
            var game = new Game();

            game.Gooi(6);
            game.Gooi(4);

            game.Gooi(5);

            Assert.AreEqual(20, game.ScoreVoorFrame(2));
        }

        [TestMethod]
        public void als_de_eerste_3_strikes_dan_is_de_frame_score_van_het_3e_frame_60()
        {
            var game = new Game();

            game.Gooi(10);
            game.Gooi(10);
            game.Gooi(10);

            Assert.AreEqual(60, game.CurrentScore);
        }

        [TestMethod]
        public void als_eerste_0_en_dan_10_en_dan_5_dan_hebben_we_20_punten()
        {
            var game = new Game();

            game.Gooi(0);
            game.Gooi(10);

            game.Gooi(5);
                        
            Assert.AreEqual(20, game.CurrentScore);
        }
        

        [TestMethod]
        public void als_de_eerste_3_spares_dan_is_de_frame_score_van_het_3e_frame_60()
        {
            var game = new Game();

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

            var game = new Game();

            for (var i = 0; i < 12; i++) {
                game.Gooi(10);
            }

            Assert.AreEqual(300, game.CurrentScore);
        }

    }

}

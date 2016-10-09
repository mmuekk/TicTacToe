using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Tests
{
    [TestClass()]
    public class GameTests
    {
        [TestMethod()]
        public void DoesPlayerWinsTest()
        {
            var board = new bool?[]
            {
                false, false, true,
                true, true, false,
                false, true, true
            };

            Assert.IsFalse( Game.DoesPlayerWins(board, false));
            Assert.IsFalse( Game.DoesPlayerWins(board, true));
        }
    }
}
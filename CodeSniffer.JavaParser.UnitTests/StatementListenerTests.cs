using NUnit.Framework;
using CodeSniffer.Listeners;
using CodeSniffer.Models;
using System.Threading;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;

namespace CodeSniffer.Parser.Java.UnitTests
{
    [TestFixture]
    public class StatementListenerTests
    {
        private StatementListener _statementListener;

        [SetUp]
        public void SetUp()
        {
            _statementListener = new StatementListener(); 
        }

        [Test]
        public void GivenAnActiveMethod_WhenAStatmentIsFound_ThenTheStatmentIsAddedToTheClassModel()
        {
            Method method = new Method(null, "", "");

            _statementListener.setCurrentMethod(method);

            Mocks.StatementContextMock statementContext = new Mocks.StatementContextMock(null, 1);
            _statementListener.EnterStatement(statementContext);
            method.ExtractInnerAndOuterMethodInvocations();

            _statementListener.resetCurrentMethod();

            Assert.AreEqual(1, method.OuterMethodInvocations.Count);
            Assert.AreEqual("testInvoke", method.OuterMethodInvocations[0].Content);
        }

        [Test]
        public void GivenNoActiveMethod_WhenAStatmentIsFound_ThenTheStatmentIsIgnored()
        {
            Mocks.StatementContextMock statementContext = new Mocks.StatementContextMock(null, 1);

            Assert.DoesNotThrow(() => _statementListener.EnterStatement(statementContext) );
        }
    }
    
}

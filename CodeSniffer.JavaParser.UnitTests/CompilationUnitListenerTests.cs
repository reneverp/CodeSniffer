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
    public class CompilationUnitListenerTests
    {
        private CompilationUnitListener _compilationUnitListener;
        private Project _project;

        [SetUp]
        public void SetUp()
        {
            _project = new Project();

            _compilationUnitListener = new CompilationUnitListener(
                                            _project,
                                                new ClassListener(
                                                    new MethodListener(
                                                        new StatementListener())));
        }

        [Test]
        public void WhenACompilationUnitIsFound_ThenTheCompilationUnitIsAddedToTheProjectModel()
        {
            Mocks.CompilationUnitContextMock compilationUnitContext = new Mocks.CompilationUnitContextMock(null, 1);
            _compilationUnitListener.EnterCompilationUnit(compilationUnitContext);

            Assert.AreEqual(_project.CompilationUnits.Count, 1);
        }
    }
    
}

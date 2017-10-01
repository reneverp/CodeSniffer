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
    public class ClassListenerTests
    {
        private ClassListener _classListener;

        [SetUp]
        public void SetUp()
        {
            _classListener = new ClassListener(
                                    new MethodListener(
                                            new StatementListener())); 
        }

        [Test]
        public void GivenAnActiveCompilationUnit_WhenAClassIsFound_ThenTheClassIsAddedToTheClassModel()
        {
            CompilationUnit compilationUnit = new CompilationUnit("");

            _classListener.setCurrentCompilationUnit(compilationUnit);

            Mocks.ClassContextMock classContext = new Mocks.ClassContextMock(null, 1);
            _classListener.EnterClassDeclaration(classContext);

            _classListener.resetCurrentComilationUnit();

            Assert.AreEqual(compilationUnit.Classes.Count, 1);
            Assert.AreEqual("test", compilationUnit.Classes[0].Text); 
        }

        [Test]
        public void GivenNoActiveCompilationUnit_WhenAClassIsFound_ThenTheClassIsIgnored()
        {
            Method method = new Method("", 0, "");

            Mocks.ClassContextMock classContext = new Mocks.ClassContextMock(null, 1);

            Assert.DoesNotThrow(() => _classListener.EnterClassDeclaration(classContext) );
        }
    }
    
}

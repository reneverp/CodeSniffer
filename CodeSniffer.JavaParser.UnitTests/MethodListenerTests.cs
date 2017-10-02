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
    public class MethodListenerTests
    {
        private MethodListener _methodListener;

        [SetUp]
        public void SetUp()
        {
            _methodListener = new MethodListener(new StatementListener()); 
        }

        [Test]
        public void GivenAnActiveClass_WhenAMethodIsFound_ThenTheMethodIsAddedToTheClassModel()
        {
            Class classmodel = new Class("", "");

            _methodListener.setCurrentClass(classmodel);

            Mocks.MethodContextMock methodcontext = new Mocks.MethodContextMock(null, 1);
            _methodListener.EnterMethodDeclaration(methodcontext);

            _methodListener.resetCurrentClass();

            Assert.AreEqual(classmodel.NumberOfMethods, 1);
            Assert.AreEqual("test", classmodel.Methods[0].Content); 
        }

        [Test]
        public void GivenNoActiveClass_WhenAMethodIsFound_ThenTheMethodIsIgnored()
        {
            Method method = new Method("", 0, "");

            Mocks.MethodContextMock methodcontext = new Mocks.MethodContextMock(null, 1);

            Assert.DoesNotThrow(() => _methodListener.EnterMethodDeclaration(methodcontext) );
        }
    }
    
}

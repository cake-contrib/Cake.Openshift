﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;
using Cake.Core;
using Moq;

namespace Cake.Openshift.Tests.Login
{
    public sealed class OpenshiftLoginnerTests
    {
        [TestClass]
        public sealed class TheRunMethod
        {
            [TestMethod]
            public void Should_Throw_If_Settings_Are_Null() => ToolTests.The_Run_Method_Should_Throw_If_Settings_Are_Null(new OpenshiftLoginnerFixture());

            [TestMethod]
            public void Should_Add_Username_And_Password()
            {
                // Given
                var fixture = new OpenshiftLoginnerFixture();
                fixture.Settings.Username = "Admin";
                fixture.Settings.Password = "Password1";

                // When
                var result = fixture.Run();

                // Then
                result.Args.Should().Be($"login --username={fixture.Settings.Username.Quote()} --password={fixture.Settings.Password.Quote()}");
            }

            [TestMethod]
            public void Should_Add_Token()
            {
                // Given
                var fixture = new OpenshiftLoginnerFixture();
                fixture.Settings.Token = "fake-token";

                // When
                var result = fixture.Run();

                // Then
                result.Args.Should().Be($"login --token={fixture.Settings.Token.Quote()}");
            }
        }

        [TestClass]
        public sealed class TheAliases
        {
            [TestMethod]
            public void Should_Throw_If_Context_Is_Null_For_Token()
            {
                // Given
                ICakeContext context = null;

                // When
                Action action = () => OpenshiftAliases.OpenshiftLogin(context, "fake");

                // Then
                action.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be("context");
            }

            [TestMethod]
            public void Should_Throw_If_Context_Is_Null_For_Username_And_Password()
            {
                // Given
                ICakeContext context = null;

                // When
                Action action = () => OpenshiftAliases.OpenshiftLogin(context, "fake-username", "fake-password");

                // Then
                action.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be("context");
            }

            [DataTestMethod]
            [DataRow(null)]
            [DataRow("")]
            public void Should_Throw_If_Username_Is_Null_Or_Empty(string username)
            {
                // Given
                var context = Mock.Of<ICakeContext>();
                var password = "Password1";

                // When
                Action action = () => OpenshiftAliases.OpenshiftLogin(context, username, password);

                // Then
                action.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be("username");
            }

            [DataTestMethod]
            [DataRow(null)]
            [DataRow("")]
            public void Should_Throw_If_Password_Is_Null_Or_Empty(string password)
            {
                // Given
                var context = Mock.Of<ICakeContext>();
                var username = "Admin";

                // When
                Action action = () => OpenshiftAliases.OpenshiftLogin(context, username, password);

                // Then
                action.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be("password");
            }

            [DataTestMethod]
            [DataRow(null)]
            [DataRow("")]
            public void Should_Throw_If_Token_Is_Null_Or_Empty(string token)
            {
                // Given
                var context = Mock.Of<ICakeContext>();

                // When
                Action action = () => OpenshiftAliases.OpenshiftLogin(context, token);

                // Then
                action.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be("token");
            }
        }
    }
}

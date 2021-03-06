// Copyright (c) Microsoft. All rights reserved.
namespace DevOpsLibTest
{
    using System;
    using DevOpsLib;
    using DevOpsLib.VstsModels;
    using NUnit.Framework;

    [TestFixture]
    public class IoTEdgeReleaseEnvironmentTest
    {
        [Test]
        public void TestConstructorWithInvalidId([Values(-1)] int id)
        {
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                    new IoTEdgeReleaseEnvironment(
                        id,
                        343406,
                        "ARM32",
                        VstsEnvironmentStatus.Succeeded,
                        TestUtil.GetDeployments(1)));
            Assert.True(ex.Message.StartsWith("Cannot be negative."));
            Assert.AreEqual("id", ex.ParamName);
        }

        [Test]
        public void TestConstructorWithInvalidDefinitionId([Values(-1, 0)] int definitionId)
        {
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                    new IoTEdgeReleaseEnvironment(
                        3782954,
                        definitionId,
                        "AMD64",
                        VstsEnvironmentStatus.Scheduled,
                        TestUtil.GetDeployments(1)));
            Assert.True(ex.Message.StartsWith("Cannot be less than or equal to zero."));
            Assert.AreEqual("definitionId", ex.ParamName);
        }

        [Test]
        public void TestProperties()
        {
            var releaseEnv = new IoTEdgeReleaseEnvironment(3242, 343406, "AMD64", VstsEnvironmentStatus.Queued, TestUtil.GetDeployments(1));

            Assert.AreEqual(3242, releaseEnv.Id);
            Assert.AreEqual(343406, releaseEnv.DefinitionId);
            Assert.AreEqual(VstsEnvironmentStatus.Queued, releaseEnv.Status);
            Assert.AreEqual(1, releaseEnv.Deployments.Count);
        }

        [Test]
        public void TestEquals()
        {
            DateTime deploymentStartTime = DateTime.UtcNow;
            var releaseEnv1 = new IoTEdgeReleaseEnvironment(3242, 343406, "Linux AMD64", VstsEnvironmentStatus.Queued, TestUtil.GetDeployments(1, deploymentStartTime));
            var releaseEnv2 = new IoTEdgeReleaseEnvironment(3242, 343406, "Linux AMD64", VstsEnvironmentStatus.Queued, TestUtil.GetDeployments(1, deploymentStartTime));
            var releaseEnv3 = new IoTEdgeReleaseEnvironment(9708, 343406, "Linux ARM64", VstsEnvironmentStatus.Queued, TestUtil.GetDeployments(1, deploymentStartTime));
            var releaseEnv4 = new IoTEdgeReleaseEnvironment(3242, 84893, "Windows X64", VstsEnvironmentStatus.Queued, TestUtil.GetDeployments(1, deploymentStartTime));
            var releaseEnv5 = new IoTEdgeReleaseEnvironment(3242, 343406, "Windows Server Core", VstsEnvironmentStatus.Succeeded, TestUtil.GetDeployments(1, deploymentStartTime));
            var releaseEnv6 = new IoTEdgeReleaseEnvironment(3242, 343406, "Windows Server Core", VstsEnvironmentStatus.Succeeded, TestUtil.GetDeployments(2, deploymentStartTime));

            Assert.False(releaseEnv1.Equals(null));
            Assert.True(releaseEnv1.Equals(releaseEnv1));
            Assert.True(releaseEnv1.Equals(releaseEnv2));

            Assert.False(releaseEnv1.Equals((object)null));
            Assert.True(releaseEnv1.Equals((object)releaseEnv1));
            Assert.True(releaseEnv1.Equals((object)releaseEnv2));
            Assert.False(releaseEnv1.Equals(new object()));

            Assert.False(releaseEnv1.Equals(releaseEnv3));
            Assert.False(releaseEnv1.Equals(releaseEnv4));
            Assert.False(releaseEnv1.Equals(releaseEnv5));
            Assert.False(releaseEnv5.Equals(releaseEnv6));
        }

        [Test]
        public void TestGetHashCode()
        {
            var releaseEnv = new IoTEdgeReleaseEnvironment(3242, 343406, "Any Name", VstsEnvironmentStatus.Queued, TestUtil.GetDeployments(1));

            Assert.AreEqual(3242, releaseEnv.GetHashCode());
        }

        [Test]
        public void TestCreate()
        {
            var vstsReleaseEnv = new VstsReleaseEnvironment { Id = 83429, DefinitionId = 2349080, DefinitionName = "Old E2E tests", Status = VstsEnvironmentStatus.Rejected };

            IoTEdgeReleaseEnvironment releaseEnv = IoTEdgeReleaseEnvironment.Create(vstsReleaseEnv);
            Assert.AreEqual(83429, releaseEnv.Id);
            Assert.AreEqual(2349080, releaseEnv.DefinitionId);
            Assert.AreEqual("Old E2E tests", releaseEnv.DefinitionName);
            Assert.AreEqual(VstsEnvironmentStatus.Rejected, releaseEnv.Status);
        }

        [Test]
        public void TestCreateEnvironmentWithNoResult()
        {
            IoTEdgeReleaseEnvironment releaseEnv = IoTEdgeReleaseEnvironment.CreateEnvironmentWithNoResult(38942);

            Assert.AreEqual(0, releaseEnv.Id);
            Assert.AreEqual(38942, releaseEnv.DefinitionId);
            Assert.AreEqual(VstsEnvironmentStatus.Undefined, releaseEnv.Status);
        }
    }
}

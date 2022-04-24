using Domain.Common;
using System;
using Xunit;

namespace DomainTests.Entities.Common
{
    public class AuditTest
    {
        [Fact]
        public void When_UpdateLastModified_Expect_ModifyUserAndDate()
        {
            //Arrange
            Audit audit = new Audit("Julio Oliveira");

            //Act
            audit.UpdateLastModified("Bruce Wayne");

            //Assert
            Assert.Equal("Bruce Wayne", audit.LastModifiedBy);
        }
    }
}

using IOLab;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTestProject1 {



    class MockUserService : IUserService {


        public List<int> GetUserPostsCalls = new List<int>();
        public int GetUsersCallCount = 0;
        public Post[] GetUserPosts(int userId) {
            GetUserPostsCalls.Add(userId);
            return new Post[] {
                new Post{ Id=1, UserId=1, Title="abc1" },
                new Post{ Id=2, UserId=2, Title="abc2" },
                new Post{ Id=3, UserId=1, Title="abc3" }
            }.Where(x => x.UserId == userId).ToArray();
        }

        public UserFromApi[] GetUsers() {
            GetUsersCallCount++;
            return new UserFromApi[] {
                new UserFromApi{ Id=1, Name="Stefan" },
                new UserFromApi{ Id=2, Name="Zbigniew" },
            };
        }
    }

    public class LazyApiTest {
        [Fact]
        public void GetUsersShouldNotCallUnderlyingApiIfNotUsed() {
            var mockService = new MockUserService();
            var api = new AthApi(mockService);
            api.GetUsers();
            Assert.Empty(mockService.GetUserPostsCalls);
            Assert.Equal(0, mockService.GetUsersCallCount);
        }

        [Fact]
        public void ShouldNotCallGetUsersPostIfPostsAreNotAccessed() {
            var mockService = new MockUserService();
            var api = new AthApi(mockService);
            foreach (var item in api.GetUsers()) { }
            Assert.Empty(mockService.GetUserPostsCalls);
            Assert.Equal(1, mockService.GetUsersCallCount);
        }

        [Fact]
        public void ShouldLazyLoadOnlyAccesedPosts() {
            var mockService = new MockUserService();
            var api = new AthApi(mockService);
            foreach (var item in api.GetUsers().First(x=>x.Id == 1).Posts) { }
            Assert.Equal(1, mockService.GetUserPostsCalls[0]);
            Assert.Equal(1, mockService.GetUsersCallCount);
        }
    }
}

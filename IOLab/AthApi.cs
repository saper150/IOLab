using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOLab {
    public class UserFromApi {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Post {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
    }

    public class User {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }

    public interface IUserService {
        UserFromApi[] GetUsers();
        Post[] GetUserPosts(int userId);
    }

    public class AthApi {
        IUserService _service;
        public AthApi(IUserService service) {
            _service = service;
        }

        private IEnumerable<Post> GetPosts(int userId) {
            foreach (var item in _service.GetUserPosts(userId)) {
                yield return item;
            }
        }

        public IEnumerable<User> GetUsers() {
            var users = _service.GetUsers()
                .Select(x=> new User {
                    Name = x.Name,
                    Id = x.Id,
                    Posts = GetPosts(x.Id)
                });

            foreach (var item in users) {
                yield return item;
            }
        }
    }
}

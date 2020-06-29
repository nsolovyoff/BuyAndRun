using AutoMapper.Configuration.Annotations;

namespace BLL.Resources.Identity.User
{
    public class CreateUserResource
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [Ignore]
        public string Password { get; set; }
        public string ImageUrl { get; set; }
    }
}


namespace RhonAyro.Common.Data.ScoreKeeping
{
    public class Person : Entity
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public RoleType Role { get; set; }

        public Person()
        {
            FirstName = null;
            LastName = null;
            Role = RoleType.Unspecified;
        }
    }
}

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents a person involved in a wheel gymnastics competition.
    /// </summary>
    public sealed class ClubMember : Entity
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// Gets the full name of this member.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Gets or sets the roles this person have.
        /// </summary>
        public RoleType Roles { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ClubMember"/>.
        /// </summary>
        public ClubMember()
        {
            Roles = RoleType.Unspecified;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, FirstName='{FirstName}', LastName='{LastName}', Roles='{Roles}'";
        }
    }
}

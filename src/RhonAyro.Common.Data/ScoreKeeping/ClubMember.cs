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
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the roles this person have.
        /// </summary>
        public RoleType Roles { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ClubMember"/>.
        /// </summary>
        public ClubMember()
        {
            FirstName = null;
            LastName = null;
            Roles = RoleType.Unspecified;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, FirstName='{FirstName}', LastName='{LastName}', Roles='{Roles}'";
        }
    }
}

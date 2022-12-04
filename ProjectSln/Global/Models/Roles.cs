namespace Main.Global.Models
{
    public record Role
    {
        public Role(int roleId)
        {
            RoleId = roleId;
            Value = RoleId switch
            {
                1 => "Security",
                2 => "Club",
                3 => "Admin",
                _ => string.Empty
            };
        }
        public int RoleId { get; init; }

        public string Value { get; private set; }
    }
}
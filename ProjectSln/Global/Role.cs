namespace Main.Global
{
    public static class Role
    {
        public static string Switch(int roleId)
        {
            return roleId switch
            {
                0 => "Security",
                1 => "Club",
                2 => "Admin",
                _ => string.Empty
            };
        }
    }
}

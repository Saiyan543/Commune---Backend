namespace Main.Global
{
    public static class Enums
    {
        public static string Switch(int roleId)
        {
            return roleId switch
            {
                0 => "Security",
                1 => "Club",
                2 => "Administrator",
                _ => string.Empty
            };
        }

        public static string ContractResponse(int responseId)
            => responseId switch
            {
                1 => "Pending",
                2 => "Accepted",
                3 => "Rejected",
                4 => "Blocked",
                _ => string.Empty
            };
    }
}
namespace Main.Global
{
    public static class Enums
    {
        public static string Switch(int roleId)
        {
            return roleId switch
            {
                1 => "Security",
                2 => "Club",
                3 => "Administrator",
                _ => throw new InvalidOperationException()
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
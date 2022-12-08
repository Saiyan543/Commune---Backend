namespace Main.Slices.Rota.Models.Contracts
{
    public record ContractRequestResponseDto
    {
        public ContractRequestResponseDto(int responseId, string Message)
        {
            ResponseId = responseId;
            this.Message = Message;
            Response = ResponseId switch
            {
                1 => "Pending",
                2 => "Accepted",
                3 => "Rejected",
                4 => "Blocked",
                _ => string.Empty
            };
        }
        public int ResponseId { get; init; }
        public string Message { get; init; }
        public string Response { get; private set; }
    }
}
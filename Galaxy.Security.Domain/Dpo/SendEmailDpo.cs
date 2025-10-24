namespace Galaxy.Security.Domain.Dpo
{
    public class SendEmailDpo
    {
        public string Recipients { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
    }
}

namespace A.L
{
    public class LSP
    {
        // clase base
        public class EmailSender
        {
            public virtual void SendEmail(string to, string subject, string body)
            {
                Console.WriteLine($"Email enviado a: {to} con asunto: {subject}");
            }
        }

        // clase derivada
        public class SecureEmailSender : EmailSender
        {
            public override void SendEmail(string to, string subject, string body)
            {
                Console.WriteLine($"Email seguro enviado a: {to} con asunto: {subject}");
            }
        }

        public class EmailService
        {
            private readonly EmailSender _emailSender;

            public EmailService(EmailSender emailSender)
            {
                _emailSender = emailSender;
            }

            public void Send(string to, string subject, string body)
            {
                _emailSender.SendEmail(to, subject, body);
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                // Servicio instanciado con la clase derivada
                EmailSender secureEmailSender = new SecureEmailSender();
                EmailService secureEmailService = new EmailService(secureEmailSender);
                secureEmailService.Send("user@email.com", "Secure Hello!", "This is a secure email.");
            }
        }
    }
}

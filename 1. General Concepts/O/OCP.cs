namespace A.O
{
    public class OCP
    {
        // Interfaz que define la funcionalidad 
        public interface IEmailSender
        {
            void SendEmail(string to, string subject, string body);
        }

        // Implementación base
        public class RegularEmailSender : IEmailSender
        {
            public void SendEmail(string to, string subject, string body)
            {
                Console.WriteLine($"Email enviado a {to} con asunto: {subject}");
            }
        }

        // Funcionalidad extendida
        public class PromotionalEmailSender : IEmailSender
        {
            public void SendEmail(string to, string subject, string body)
            {
                Console.WriteLine($"Emailpromocional enviado a {to} con asunto: {subject}");
                Console.WriteLine($"Oferta especial: {body}");
            }
        }

        // Servicio que depende de la interfaz
        public class EmailService
        {
            private readonly IEmailSender _emailSender;

            public EmailService(IEmailSender emailSender)
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
                // Uso de la funcionalidad Base
                IEmailSender regularSender = new RegularEmailSender();
                EmailService emailService = new EmailService(regularSender);
                emailService.Send("user@email.com", "Welcome", "Welcome");

                // Uso de la funcionalidad extendida
                IEmailSender promoSender = new PromotionalEmailSender();
                EmailService promoEmailService = new EmailService(promoSender);
                promoEmailService.Send("user@email.com", "Special Offer", "Offer");
            }
        }
    }
}

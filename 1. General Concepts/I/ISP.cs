namespace A.I
{
    public class ISP
    {
        public interface IEmailSender
        {
            void SendEmail(string to, string subject, string body);
        }

        public interface IReportGenerator
        {
            void GenerateReport();
        }

        // Sólo implementa la interfaz de envío de emails
        public class EmailSender : IEmailSender
        {
            public void SendEmail(string to, string subject, string body)
            {
                Console.WriteLine($"Email enviado a {to} con asunto: {subject}");
            }
        }

        // Sólo implementa la interfaz de Generación de reporte
        public class EmailReportGenerator : IReportGenerator
        {
            public void GenerateReport()
            {
                Console.WriteLine("Email de reporte generado");
            }
        }

    }
}

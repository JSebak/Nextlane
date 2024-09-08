namespace A.D
{
    public interface IPdfGenerator
    {
        string GeneratePdf(string content);
    }

    public interface IEmailSender
    {
        void SendEmail(string to, string subject, string body, string attachment);
    }

    public class PdfGenerator : IPdfGenerator
    {
        public string GeneratePdf(string content)
        {
            // Logic for generating a PDF
            return "PDF content generated";
        }
    }

    public class EmailSender : IEmailSender
    {
        public void SendEmail(string to, string subject, string body, string attachment)
        {
            // Logic for sending an email
            Console.WriteLine($"Email sent to {to} with subject: {subject}");
        }
    }

    // DIP: La clase de alto nivel depende de las interfaces, no de las clases.
    public class ReportService
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IEmailSender _emailSender;

        public ReportService(IPdfGenerator pdfGenerator, IEmailSender emailSender)
        {
            _pdfGenerator = pdfGenerator;
            _emailSender = emailSender;
        }

        public void GenerateAndSendReport(string to, string reportContent)
        {
            var pdf = _pdfGenerator.GeneratePdf(reportContent);
            _emailSender.SendEmail(to, "Report", "Please find attached your report.", pdf);
        }
    }

}

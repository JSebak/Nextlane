public class Program
{
    private static void Main(string[] args)
    {
        bool shouldContinue = true;
        while (shouldContinue)
        {
            Console.WriteLine("Enter a string to invert or type 'ESC' to exit");
            string userInput = Console.ReadLine();
            if (userInput == "ESC")
            {
                Console.WriteLine("Exiting");
                shouldContinue = false;

            }
            else
            {
                string invertedString = InvertString(userInput);

                Console.WriteLine("Inverted string: " + invertedString);
            }

        }

        string InvertString(string originalString)
        {
            string invertedString = "";
            for (int i = originalString.Length - 1; i >= 0; i--)
            {
                invertedString += originalString[i];
            }
            return invertedString;
        }
    }
}
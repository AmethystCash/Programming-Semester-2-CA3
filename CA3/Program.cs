using System.Globalization;
using System.Numerics;

namespace CA3
{
    public class Vars ///Used to store 'global' variables that are referenced throughout the program
    {
        public static bool criticalError = false;
        public static bool stop = false;
        public static Passanger[] passangers;
        public static string[,] ships = new string[0,4]; //x by 4. 0: name, 1: DeparturePort, 2: ArrivalDate, 3: passangers 
        public static string[,] occupations = new string[0,2]; //x by 2. 0:Occ name, 1: count
        public static int[] ageSections = {1, 12, 19, 29, 50, 1000}; //sections of ages used for age report function
        public static int[] ages = new int[7];
        
    }

    public class Passanger  ///Each line of the imported CSV should be parsed into an instance of this class
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        private string _sex;
        private string _occupation;
        private string _nativeCountry;
        private string _destination;
        private string _passangerPortCode;
        private string _manifestID;
        private string _arrivalDate;

        public string firstName
        {
            get { return _firstName; }
            set
            {
                try
                {
                    _firstName = value;
                }
                catch
                {
                    _firstName = "unknown";
                }

            }
        }
        public string lastName
        {
            get { return _lastName; }
            set { try
                {
                    _lastName = value;
                }
                catch
                {
                    _lastName = "unknown";
                }

            }
        }
        public string age
        {
            get { return _age.ToString(); }
            set
            {

                if (value.ToString().StartsWith("Infant"))
                {
                    _age = 0;
                    Vars.ages[0]++;
                }
                else
                {
                    try
                    {
                        for (int i = 1; i < 6; i++)
                        {
                            if (int.Parse(value.Substring(4)) <= Vars.ageSections[i] && Vars.ageSections[i-1] < int.Parse(value.Substring(4)))
                                Vars.ages[i]++;
                        }
                        _age = int.Parse(value.Substring(4));
                    }
                    catch
                    { 
                        _age = -1;
                        Vars.ages[6]++;
                    }
                }
            }
        }
        public string sex 
        { 
            get { return _sex; }
            set { _sex = value; }
        }

        public string occupation
        {
            get { return _occupation; }
            set { 
                _occupation = value;  
                bool ocFound = false;
                for (int i = 0; i < Vars.occupations.GetLength(0); i++)
                {
                    if (Vars.occupations[i, 0] == value)
                    {
                        ocFound = true;
                        Vars.occupations[i, 1] = (int.Parse(Vars.occupations[i, 1]) + 1).ToString();
                    }
                }

                if (!ocFound)
                {
                    string[,] temp = new string[Vars.occupations.GetLength(0) + 1, 2];
                    for(int i = 0; i < Vars.occupations.GetLength(0); i++)
                    {
                        temp[i, 0] = Vars.occupations[i, 0];
                        temp[i, 1] = Vars.occupations[i, 1];
                    }

                    temp[temp.GetLength(0) - 1, 0] = value;
                    temp[temp.GetLength(0) - 1, 1] = "1";

                    Vars.occupations = temp;
                }

            }
        }

        public string nativeCountry
        {
            get { return _nativeCountry; }
            set { _nativeCountry = value; }
        }

        public string destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        public string passangerPortCode
        {
            get { return _passangerPortCode; }
            set { _passangerPortCode = value; }
        }
        public string arrivalDate
        {
            get { return _arrivalDate; }
            set {
                try
                {
                    DateOnly date;
                    DateOnly.TryParse(value, CultureInfo.GetCultureInfo("us-EN"), DateTimeStyles.None, out date);

                    _arrivalDate = date.ToString();
                }
                catch
                {
                    _arrivalDate = "Unknown";
                }
            }
        }

        private bool found = false; //has the ship been found in Vars.ships
        public string manifestID
        {

            get { return _manifestID; }
            set
            {
                if (Vars.ships.GetLength(0) != 0) 
                {
                    for (int i = 0; i < Vars.ships.GetLength(0); i++) //going through the Vars.ships array to find the current ship & adding 1 passanger
                    {
                        if (value == Vars.ships[i, 0])
                        {
                            _manifestID = value;
                            found = true;
                            Vars.ships[i, 3] = (int.Parse(Vars.ships[i, 3]) + 1).ToString();
                        }
                    }
                }

                if (!found) //if ship isn't in Vars.ships duplicate Vars.ships to temp array, add new ship & overwrite Vars.ships with the new ship.
                {
                    string[,] temp = new string[Vars.ships.GetLength(0) + 1, 4];
                    for (int i = 0; i < Vars.ships.GetLength(0); i++)
                    {
                        temp[i, 0] = Vars.ships[i, 0];
                        temp[i, 1] = Vars.ships[i, 1];
                        temp[i, 2] = Vars.ships[i, 2];
                        temp[i, 3] = Vars.ships[i, 3];
                    }

                    temp[Vars.ships.GetLength(0), 0] = value;
                    temp[Vars.ships.GetLength(0), 1] = _passangerPortCode;
                    temp[Vars.ships.GetLength(0), 2] = _arrivalDate;
                    temp[Vars.ships.GetLength(0), 3] = "1";
                    _manifestID = value;

                    Vars.ships = temp;
                }
            }
        }

        public static void prnt(Passanger pas, int i = 0) //print statement for the passanger class. 
        {
            Console.WriteLine($"{i}: {pas.firstName}, {pas.lastName}, {pas.age}, {pas.passangerPortCode}, {pas.manifestID}, {pas.arrivalDate}, {pas.sex}, {pas.destination}, {pas.nativeCountry}, {pas.occupation}");
        }

    }


    internal class Program
    {
        static int selectMenuItem(int min, int max)
        {
            string input = Console.ReadLine();
            int output;
            try
            {
                output = int.Parse(input);
                if (output < min || output > max)
                {
                    Console.WriteLine($"Invalid selection. Please use an interger between {min} and {max}");
                    return selectMenuItem(min, max);
                }
                else
                    return output;
            }
            catch
            {
                Console.WriteLine($"Invalid selection. Please use an interger between {min} and {max}");
                return selectMenuItem(min, max);
            }
        }

        static void mainMenu()
        {
            try
            {

                Console.WriteLine("\nMain Menu\n\n 1. Ship Reports\n 2. Occupation Reports\n 3. Age Report\n 4. Exit\n\nEnter Choice:");
                int choice = selectMenuItem(1, 4);
                if (choice == 1)
                {
                    //Console.WriteLine(Vars.ships.GetLength(0));
                    shipReprt();
                }
                else if (choice == 2)
                {
                    occuReport();
                }
                else if (choice == 3)
                {
                    ageReport();
                }
                else if (choice == 4)
                {
                    Vars.stop = true;
                }
                else
                {
                    Console.WriteLine("An unknown error occoured.");
                    Vars.criticalError = true;
                }
            }
            catch
            {
                Vars.criticalError = true;
                Console.WriteLine("An unknown error occoured.");
            }

        }
        static void shipReprt()
        {
            Console.WriteLine("Ship Report: \n");
            int k = 0;
            for (int i = 0; i < Vars.ships.GetLength(0); i++)
            {
                k++;
                Console.WriteLine($"{k}. {Vars.ships[i, 0]}");
            }

            int selection = selectMenuItem(1, k) - 1;

            Console.WriteLine($"DISPATCH: {Vars.ships[selection, 0]}.  Leaving From: {Vars.ships[selection, 1]}.  Arrived: {Vars.ships[selection, 2]} with {Vars.ships[selection, 3]} passangers.");

            for (int j = 0; j < Vars.passangers.Length; j++)
            {
                if (Vars.passangers[j].manifestID == Vars.ships[selection, 0])
                {
                    Console.WriteLine($"{Vars.passangers[j].firstName} {Vars.passangers[j].lastName}");
                }
            }

        }
        static void occuReport()
        {
            for (int i = 0; i < Vars.occupations.GetLength(0); i++)
            {
                Console.WriteLine($"{Vars.occupations[i, 0]}: {Vars.occupations[i, 1]}");
            }
        }
        static void ageReport()
        {
            Console.WriteLine($"Age Report:\n\nInfants: {Vars.ages[0]}");
            for (int i = 1; i < Vars.ageSections.Length - 1; i++)
            {
                Console.WriteLine($"{Vars.ageSections[i-1]} to {Vars.ageSections[i]}: {Vars.ages[i]}");
            }
            Console.WriteLine($"Over 50: {Vars.ages[5]}\nUnknown: {Vars.ages[6]}");

        }

        static long getFileLength(string filepath)
        {
            try
            {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                string headers = sr.ReadLine();

                long len = 0;
                string line = "";
                do
                {
                    line = sr.ReadLine();
                    len++;
                }
                while (line != null);

                sr.Close();
                fs.Close();

                Console.WriteLine("Importing " + len + " records...");
                return len;
            }
            catch
            {
                Console.WriteLine("There was a problem opening the file. Please try again.");
                Vars.criticalError = true;
                return 0;
            }
            
        }

        static void importCSV(string filepath)
        {
            try
            {
                long len = getFileLength(filepath);

                if (!Vars.criticalError) //if an error occours in getFileLength this will stop the same error reoccouring and the user getting 2 error messages
                {
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);
                    string headers = sr.ReadLine();

                    Vars.passangers = new Passanger[len - 1];
                    for (int i = 0; i < len - 1; i++)
                    {
                        string[] csvLine = sr.ReadLine().Split(",");
                        if (csvLine.Length == 10)
                        {
                            Vars.passangers[i] = new Passanger();
                            Vars.passangers[i].firstName = csvLine[1];
                            Vars.passangers[i].lastName = csvLine[0];
                            Vars.passangers[i].age = csvLine[2];
                            Vars.passangers[i].sex = csvLine[3];
                            Vars.passangers[i].occupation = csvLine[4];
                            Vars.passangers[i].nativeCountry = csvLine[5];
                            Vars.passangers[i].destination = csvLine[6];
                            Vars.passangers[i].passangerPortCode = csvLine[7];
                            Vars.passangers[i].manifestID = csvLine[8];
                            Vars.passangers[i].arrivalDate = csvLine[9];
                        }
                        else
                        {
                            Console.WriteLine($"Line {i} contains invalid data and has not been imported.");
                        }
                    }

                    sr.Close();
                    fs.Close();
                }

            }
            catch
            {
                Console.WriteLine("File Parsing failed. Please double check the filepath or contact tech support.");
                Vars.criticalError = true;
            }

        }


        static void Main(string[] args)
        {
            importCSV("C:\\Users\\samki\\Downloads\\faminefile.csv");       //please adjust csv's filepath accordingly
            
            if (!Vars.criticalError)
            {
                while (!Vars.stop && !Vars.criticalError)
                {
                    mainMenu();
                }
            }    
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;

//TODO: Provide functionality for numbers below 8

class Program
{
    static void Main(string[] args)
    {
        Dictionary<long, long> prevPrime = new Dictionary<long, long>();
        Console.Out.WriteLine("Enter a number: ");
        long NUMPRIMES = int.Parse(Console.In.ReadLine());  //Number which the primes are counted to
        long primeCount = 1;    //This is how many primes I have at the start, is increased as the program continues
        long count = 3;     //Starts at 3 because I manually add 2 and 3 is the prime after 2

        if (NUMPRIMES <= 2) //If someone tries to be clever..
            Environment.Exit(0);

        Console.Out.WriteLine("Enter progress output interval (less is much slower), enter 0 for no progress information");
        int INTERVAL = int.Parse(Console.In.ReadLine());

        var watch = System.Diagnostics.Stopwatch.StartNew(); //STOPWATCH

        bool factorFound = false;   //Only one factor needs to be found to invalidate a potential prime

        //Initialise the prevPrime array with the first prime
        //Code was changed to essentially ignore 2; as the role
        //of 2 as a prime can be assumed rather than checked for.
        //This change equates to around 5% better performance
        prevPrime[0] = 2;
        if (NUMPRIMES > 3)  //Fringe case handling
            prevPrime[1] = 3;

        for (; count < NUMPRIMES; count += 2) //Primes other than 2 are all odd
        {
            //At every INTERVAL interations will update the user interface and
            //nicely display the progress of the prime generation
            //however if performance is what you'd prefer, just remove this 'if' statement
            if (INTERVAL != 0 && count % INTERVAL == 0)
            {
                Console.Clear();
                Console.Out.WriteLine("Calculating prime numbers up to " + NUMPRIMES);
                Console.Out.WriteLine("Current number: " + count);
                Console.Out.Write("[");
                for (int i = 1; i <= 50; i++) //50 because 100 / 2 = 50, I'm not bothered to make a smaller bar
                {
                    //Essentially draws the percentage of how far along 'count' is
                    if (i * 2 <= ((double)count / (double)NUMPRIMES) * 100.0)
                        Console.Out.Write("|");
                    else
                        Console.Out.Write(" ");
                }
                Console.Out.Write("]");
            }
            if (NUMPRIMES != 3) {
                for (int j = 1; j < primeCount; j++)    //Should be noted that 'j' only ever reaches primeCount when a prime is found
                {
                    if (count % prevPrime[j] == 0)  //If a factor of the current number is found
                    {
                        factorFound = true;
                        break;
                    }
                    //The 'if' statement below was an idea to improve efficiency
                    //the logic is that x can only potentially be a factor of y if x^2 <= y
                    if (count < (prevPrime[j] * prevPrime[j]))
                    {
                        break;
                    }
                }
                if (factorFound)
                {
                    factorFound = false;
                }
                else
                {
                    prevPrime[primeCount] = count;  //Add the found prime to the prime dictionary
                    primeCount++;
                }
            }
        }

        watch.Stop();

        Console.Out.WriteLine("\nTime taken to calculate: " + watch.ElapsedMilliseconds + " milliseconds");

        File.Delete("PrimeList.txt");
        StreamWriter sw = File.AppendText("PrimeList.txt");

        sw.AutoFlush = true; //Fixes text truncation near EOF

        Console.Out.WriteLine("\nCurrently writing to file.");
        //Console.Out.WriteLine("Number of primes below " + NUMPRIMES + " = " + primeCount);
        sw.WriteLine("Number of primes below " + NUMPRIMES + " = " + primeCount);

        //Output for file 'Prime List', will display a (10*n) grid
        for (int i = 0; i < primeCount; i++)
        {
            if ((i % 10 != 0 || i == 0) && i != primeCount - 1)
                sw.Write(prevPrime[i] + ", ");
            else
                sw.WriteLine(prevPrime[i]);
        }

        Console.Out.WriteLine("Finished!");

        // Output for console, will display a (10*n) grid
        /*
        for (int i = 0; i < primeCount; i++)
        {
            if((i % 10 != 0 || i == 0) && i != primeCount - 1)
                Console.Out.Write(prevPrime[i] + ", ");
            else
                Console.Out.WriteLine(prevPrime[i]);
        }
        */


        Console.In.Read();
    }
}





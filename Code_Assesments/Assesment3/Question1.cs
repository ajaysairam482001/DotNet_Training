using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment3
{
    class CricketTeam
    {
        public void PointsCalculation(int matchCount, out int count, out int average, out int totalSum)
        {
            List<int> scores = new List<int>();
            totalSum = 0;
            for (int i = 0; i < matchCount; i++)
            {
                Console.Write($"What is the score of match {i + 1} : ");
                int score = int.Parse(Console.ReadLine());
                scores.Add(score);
                totalSum += score;
            }
            count = matchCount;
            average = totalSum / count;

        }
        
    }

    class Question1
    {
        public static void Main(string[] args)
        {
            int matches;
            Console.Write("Enter the number of matches : ");
            matches = int.Parse(Console.ReadLine());
            CricketTeam cricket = new CricketTeam();
            int count, avg, total;
            cricket.PointsCalculation(matches, out count, out avg, out total);
            Console.WriteLine($"Matches: {matches}, Average: {avg}, Sum of the Scores: {total}");
            Console.ReadLine();
        }
    }
}

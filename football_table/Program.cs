﻿using Football.File;
using Football.Ranking; 

namespace Football
{
    public class Program
    {
        static FileAssistant fileAssistant = new FileAssistant();
        static List<List<string>> matchesCSV = fileAssistant.Read("../csv files/Rounds");
        static List<List<string>> teamsCSV = fileAssistant.Read("../csv files/teams");
        static List<Team> teamList = new List<Team>();
        static List<Match> matchList = new List<Match>();
        static List<Round> roundList = new List<Round>();

        public static void Main(String[] args)
        {

            foreach (List<string> t in teamsCSV)
            {
                Team team = new Team(t);
                teamList.Add(team);
            }

            foreach (List<string> m in matchesCSV)
            {
                var team1 = teamList.Find(t => t.abbr == m[0]);
                var team2 = teamList.Find(t => t.abbr == m[1]);

                Match match = new Match(m, team1, team2);
                matchList.Add(match);
            }


            for (int i = 0; i < matchList.Count; i += 6)
            {
                var currentRound = matchList.GetRange(i, 6);
                var round = new Round(currentRound);
                round.Fight();
                roundList.Add(round);
            }

            foreach (Team team in teamList)
            {
                team.winStreak(roundList.GetRange(26, 6));
            }

            Table();
            
        }

        public static void Table()
        {
            var result = teamList
                .OrderByDescending(team => team.points)
                .ThenBy(team => team.goalDifference)
                .ThenBy(team => team.goalsScored)
                .ThenBy(team => team.goalsLost)
                .ThenBy(team => team.name);

            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine("Po. Club  Name                        Rank  MP   W    D    L    GF   GA   GD   P    Streak");
            Console.WriteLine("-------------------------------------------------------------------------------------------");

            int i = 1;
            foreach (Team t in result)
            {
                switch (t.specialRanking)
                {
                    case Ranking.SpecialRanking.R:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;
                    case Ranking.SpecialRanking.C:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case Ranking.SpecialRanking.P:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;
                    case Ranking.SpecialRanking.W:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    default:
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine($"{i, -2} | {t.abbr,-3} | {t.name,-25} | {t.specialRanking,-2} | {t.numberOfGamesPlayed,-2} | {t.numberOfGamesWon,-2} |"
                + $" {t.numberOfGamesDrawn,-2} | {t.numberOfGamesLost,-2} | {t.goalsScored,-2} | {t.goalsLost,-2} | {t.goalDifference,-3} | {t.points, -2} | {t.streak}");

                i++;
                Console.ResetColor();
            }
        }
    }
}
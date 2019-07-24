using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_2360_Project_ThePlace
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Move[] moves = MoveLoader();
            Fighter[] fighters = FighterLoader(moves, rnd);
            BattleLoop(moves, fighters, rnd);
            //This allows the user to see the results
            System.Console.Write("Press any key to continue . . . ");
            System.Console.ReadKey(true);
        }
        static void BattleLoop(Move[] moves, Fighter[] fighters, Random rnd)
        {
            bool gameDone = false;
            int turnCount = 0;

            while (!gameDone)
            {
                System.Console.WriteLine("Turn {0}", turnCount + 1);
                for (int i = 0; i < fighters.Length; i++)
                {
                    if (fighters[i].statHealth > 0)
                    {
                        if (fighters[i].isAI)
                        {
                            fighters[i].nextMove = MoveSelecter(fighters[i], rnd);
                            fighters[i].nextTarget = TargetSelecter(fighters, i, rnd);
                        }
                        else
                        {
                            UserInput(fighters, moves, i);
                        }
                    }
                }

                int[] attackOrder = AttackOrder(fighters, moves);

                for (int i = 0; i < fighters.Length; i++)
                {
                    if (fighters[attackOrder[i]].statHealth > 0)
                    {
                        Attack(fighters, moves, attackOrder, i, rnd);
                    }
                    else
                    {
                        System.Console.WriteLine("{0} is dead", fighters[attackOrder[i]].name);
                    }
                }
                turnCount++;
                gameDone = TestGameDone(fighters, moves);
            }
        }
        static Move[] MoveLoader()
        {
            Move[] moves = new Move[6];

            moves[0] = new Move
            {
                name = "null",
                damage = 0,
                accuracy = 0,
                speed = 0
            };

            moves[1] = new Move
            {
                name = "Punch",
                damage = 20,
                accuracy = 80,
                speed = 10
            };

            moves[2] = new Move
            {
                name = "Kick",
                damage = 40,
                accuracy = 60,
                speed = 10
            };

            moves[3] = new Move
            {
                name = "Running Punch",
                damage = 15,
                accuracy = 50,
                speed = 20
            };

            moves[4] = new Move
            {
                name = "Focused Punch",
                damage = 15,
                accuracy = 95,
                speed = 10
            };

            moves[5] = new Move
            {
                name = "Nibble",
                damage = 5,
                accuracy = 95,
                speed = 15
            };

            return moves;
        }
        static Fighter[] FighterLoader(Move[] moves, Random rnd)
        {
            int MAX_MOVES_KNOWN = 4;
            bool inputGood = false;
            int fighterCount = 0;
            int teamCount = 0;
            string[] names = { "Joe", "Rat", "Dog", "Fish", "Log", "Tim" };
            float tempAttack = 2;
            float tempDefence = 2;
            float tempSpeed = 2;
            
            System.Console.Write("Enter the number of fighters: ");
            while (!int.TryParse(System.Console.ReadLine(), out fighterCount))
            {
                System.Console.WriteLine("Invalid");
            }
            Fighter[] fighters = new Fighter[fighterCount];
            int[] fighterMoves = new int[MAX_MOVES_KNOWN];

            while (!inputGood)
            {
                System.Console.Write("Enter the number of teams: ");
                if (int.TryParse(System.Console.ReadLine(), out teamCount))
                {
                    if (teamCount > 1)
                    {
                        inputGood = true;
                    }
                    else
                    {
                        System.Console.WriteLine("Must be greater then 1");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid input");
                }

            }

            for (int i = 0; i < fighterCount; i++)
            {
                for (int k = 0; k < fighterMoves.Length; k++)
                {
                    fighterMoves[k] = rnd.Next(1, moves.Length);
                }
                fighters[i] = new Fighter
                {
                    name = names[rnd.Next(0, names.Length)] + i,
                    statMaxHealth = 500,
                    statHealth = 500,
                    statAttack = tempAttack / rnd.Next(1, 100),
                    statDefence = tempDefence / rnd.Next(1, 100),
                    statSpeed = tempSpeed / rnd.Next(1, 100),
                    isAI = true,
                    teamNum = rnd.Next(1, teamCount+1),
                    knownMoves = fighterMoves
                };
            }

            /*int[] fighter0Moves = { 1, 2, 3, 0 };
            fighters[0] = new Fighter
            {
                name = "Joe",
                statMaxHealth = 100,
                statHealth = 100,
                statAttack = 1,
                statDefence = 2,
                statSpeed = 5,
                isAI = true,
                teamNum = 2,
                knownMoves = fighter0Moves
            };*/

            return fighters;
        }
        static void UserInput(Fighter[] fighters, Move[] moves, int currectFighter)
        {
            bool done = false;
            bool inputGood = false;
            string userInput = "";
            int convertedInput = 0;
            int stage = 1;

            System.Console.WriteLine("{0}s turn", fighters[currectFighter].name);
            while (!done)
            {
                if (stage == 1)
                {
                    for (int i = 0; i < fighters[currectFighter].knownMoves.Length;i++)
                    {
                        if(fighters[currectFighter].knownMoves[i] != 0)
                        {
                            System.Console.WriteLine("{0}: {1}", i+1, moves[fighters[currectFighter].knownMoves[i]].name);
                        }
                    }
                    System.Console.Write("Please select a move: ");
                    inputGood = false;
                    while (!inputGood)
                    {
                        userInput = System.Console.ReadLine();
                        if (int.TryParse(userInput, out convertedInput))
                        {
                            convertedInput -= 1;
                            if (convertedInput < 0 || convertedInput > fighters[currectFighter].knownMoves.Length-1 || fighters[currectFighter].knownMoves[convertedInput] == 0)
                            {
                                System.Console.WriteLine("{0} is invalid", userInput);
                                System.Console.Write("Please enter another number: ");
                            }
                            else
                            {
                                fighters[currectFighter].nextMove = fighters[currectFighter].knownMoves[convertedInput];
                                System.Console.WriteLine("{0} selected", moves[fighters[currectFighter].nextMove].name);
                                inputGood = true;
                                stage = 2;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("{0} is invalid", userInput);
                            System.Console.Write("Please enter another number: ");
                        }
                    }
                }
                if (stage == 2)
                {
                    for(int i = 0; i < fighters.Length; i++)
                    {
                        if (i != currectFighter)
                        {
                            System.Console.WriteLine("{0}: {1} {2}", i + 1, (fighters[currectFighter].teamNum == fighters[i].teamNum) ? "Ally" : "Enemy", fighters[i].name);
                        }
                    }
                    System.Console.Write("Please select a target or b to return: ");
                    inputGood = false;
                    while (!inputGood)
                    {
                        userInput = System.Console.ReadLine();
                        if (userInput == "b")
                        {
                            stage = 1;
                            inputGood = true;
                        }
                        else if (int.TryParse(userInput, out convertedInput))
                        {
                            convertedInput -= 1;
                            if (convertedInput < 0 || convertedInput > fighters.Length - 1 || convertedInput == currectFighter)
                            {
                                System.Console.WriteLine("{0} is invalid", userInput);
                                System.Console.Write("Please enter another number: ");
                            }
                            else
                            {
                                fighters[currectFighter].nextTarget = convertedInput;
                                System.Console.WriteLine("{0} targeted", fighters[fighters[currectFighter].nextTarget].name);
                                inputGood = true;
                                stage = 3;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("{0} is invalid", userInput);
                            System.Console.Write("Please enter another number: ");
                        }
                    }
                }
                if (stage == 3)
                {
                    done = true;
                }
            }
        }
        static int MoveSelecter(Fighter currectFighter, Random rnd)
        {
            int numOfMoves = 0;
            int selectedMove = 0;
            if (currectFighter.isAI)
            {
                for (int i = 0; i < currectFighter.knownMoves.Length; i++)
                {
                    if (currectFighter.knownMoves[i] != 0)
                    {
                        numOfMoves += 1;
                    }
                }
                selectedMove = currectFighter.knownMoves[rnd.Next(0, numOfMoves)];
            }
            else
            {
                System.Console.WriteLine("Im not listening");
            }
            return selectedMove;
        }
        static int TargetSelecter(Fighter[] fighters, int currectFighter, Random rnd)
        {
            bool done = false;
            int selectedTarget = 0;
            while (!done)
            {
                selectedTarget = (rnd.Next(0, fighters.Length));
                if (currectFighter != selectedTarget && fighters[selectedTarget].statHealth > 0 && fighters[selectedTarget].teamNum != fighters[currectFighter].teamNum)
                {
                    done = true;
                }
            }
            return selectedTarget;
        }
        static int[] AttackOrder(Fighter[] fighters, Move[] moves)
        {
            bool swapped = true;
            int[] order= new int[fighters.Length];

            for (int currectFighter = 0; currectFighter < fighters.Length; currectFighter++)
            {
                order[currectFighter] = currectFighter;
            }

            while (swapped)
            {
                swapped = false;
                for (int currectFighter = 1; currectFighter < fighters.Length; currectFighter++)
                {
                    if ((fighters[order[currectFighter - 1]].statSpeed* moves[fighters[order[currectFighter - 1]].nextMove].speed) < (fighters[order[currectFighter]].statSpeed*moves[fighters[order[currectFighter]].nextMove].speed))
                    {
                        int temp = order[currectFighter - 1];
                        order[currectFighter - 1] = order[currectFighter];
                        order[currectFighter] = temp;
                        swapped = true;
                    }
                }
            }
            return order;
        }
        static void Attack(Fighter[] fighters, Move[] moves, int[] attackOrder, int currectFighter, Random rnd)
        {
            System.Console.WriteLine("{0} Attacked {1} with {2}", fighters[attackOrder[currectFighter]].name,
                                                                      fighters[fighters[attackOrder[currectFighter]].nextTarget].name,
                                                                      moves[fighters[attackOrder[currectFighter]].nextMove].name);
            if (rnd.Next(1, 101) < moves[fighters[attackOrder[currectFighter]].nextMove].accuracy)
            {
                fighters[fighters[attackOrder[currectFighter]].nextTarget].statHealth -= (fighters[attackOrder[currectFighter]].statAttack * moves[fighters[attackOrder[currectFighter]].nextMove].damage) / fighters[fighters[attackOrder[currectFighter]].nextTarget].statDefence;
                System.Console.WriteLine("Attack Hits, {0} is left with {1} health", fighters[fighters[attackOrder[currectFighter]].nextTarget].name, fighters[fighters[attackOrder[currectFighter]].nextTarget].statHealth.ToString("0"));
                if (fighters[fighters[attackOrder[currectFighter]].nextTarget].statHealth <= 0)
                {
                    fighters[fighters[attackOrder[currectFighter]].nextTarget].nextMove = 0;
                    System.Console.WriteLine("{0} has been defeated!", fighters[fighters[attackOrder[currectFighter]].nextTarget].name);
                }
            }
            else
            {
                System.Console.WriteLine("Attack Misses");
            }
        }
        static bool TestGameDone(Fighter[] fighters, Move[] moves)
        {
            bool isGameDone = false;
            int firstTeam = 0;
            bool firstTeamFound = false;
            bool anotherTeamFound = false;
            for (int currentFighter = 0; currentFighter < fighters.Length; currentFighter++)
            {
                if (fighters[currentFighter].statHealth > 0 && fighters[currentFighter].teamNum != firstTeam && !firstTeamFound)
                {
                    firstTeam = fighters[currentFighter].teamNum;
                    firstTeamFound = true;
                }
            }
            for (int currentFighter = 1; currentFighter < fighters.Length; currentFighter++)
            {
                if(fighters[currentFighter].statHealth > 0 && fighters[currentFighter].teamNum != firstTeam)
                {
                    anotherTeamFound = true;
                }
            }
            if (anotherTeamFound)
            {
                isGameDone = false;
            }
            else
            {
                isGameDone = true;
                for (int currentFighter = 0; currentFighter < fighters.Length; currentFighter++)
                {
                    if (fighters[currentFighter].statHealth > 0)
                    {
                        System.Console.WriteLine("{0} wins!", fighters[currentFighter].name);
                        for (int i = 0; i < fighters[currentFighter].knownMoves.Length; i++)
                        { 
                            System.Console.WriteLine("Moves {0}", moves[fighters[currentFighter].knownMoves[i]].name);
                        }
                        System.Console.WriteLine("{0} {1} {2} {3}", fighters[currentFighter].statMaxHealth, fighters[currentFighter].statAttack, fighters[currentFighter].statDefence, fighters[currentFighter].statSpeed);
                    }
                }
            }
            return isGameDone;
        }
    }
    class Fighter
    {
        public string name = "tempName";
        public double statMaxHealth = 0;
        public double statHealth = 0;
        public double statAttack = 0;
        public double statDefence = 0;
        public double statSpeed = 0;
        public int[] knownMoves = { 0, 0, 0, 0 };
        public bool isAI = true;
        public int teamNum = 0;

        public int nextMove = 0;
        public int nextTarget = 0;
    }
    class Move
    {
        public string name = "tempName";
        public double damage = 0;
        public int accuracy = 0;
        public double speed = 0;
    }
}

using System;
using System.Collections.Generic;


namespace COP_2360_Project_ThePlace
{
    class Program // This Class builds the move array and fighter array, then creates and starts a Battle.
    {
        static void Main(string[] args) // Main thats auto Called.
        {
            Random rnd = new Random();
            Move[] moves = MoveLoader(rnd);
            Fighter[] fighters = FighterLoader(moves, rnd);
            Battle mainBattle = new Battle(fighters, moves, 10000, rnd);
            mainBattle.StartBattle();
            //This allows the user to see the results
            System.Console.Write("Press any key to continue . . . ");
            System.Console.ReadKey(true);
        }
        static Move[] MoveLoader(Random rnd) // This builds the move array.
        {
            Move[] moves = new Move[13];
            moves[0] = new Move("null", 0, 0, 0, false, false);
            moves[1] = new Move("Punch", 20, 80, 10, false, true);
            moves[2] = new Move("Kick", 40, 60, 10, false, true);
            moves[3] = new Move("Running Punch", 20, 50, 15, false, true);
            moves[4] = new Move("Focused Punch", 15, 95, 9, false, true);
            moves[4].AddBuff(new Buff(GLOBAL.ATTACK_ID, 10, 3, true, 75, 100, rnd));
            moves[5] = new Move("Foot Nibble", 5, 75, 9, false, true);
            moves[5].AddBuff(new Buff(GLOBAL.ATTACK_ID, -10, 3, false, 95, 100, rnd));
            moves[6] = new Move("Bandage", -10, 95, 8, false, false);
            moves[6].AddBuff(new Buff(GLOBAL.HEALTH_ID, 5 , 3, false, 95, 100, rnd));
            moves[7] = new Move("Stand Guard", 0, 98, 20, true, false);
            moves[7].AddBuff(new Buff(GLOBAL.DEFENCE_ID, 30, 1, true, 100, 100, rnd));
            moves[8] = new Move("Curse", 0, 95, 10, false, true);
            moves[8].AddBuff(new Buff(GLOBAL.ATTACK_ID, -20, 1, false, 100, 100, rnd));
            moves[8].AddBuff(new Buff(GLOBAL.DEFENCE_ID, -20, 1, false, 100, 100, rnd));
            moves[8].AddBuff(new Buff(GLOBAL.SPEED_ID, -20, 1, false, 100, 100, rnd));
            moves[8].AddBuff(new Buff(GLOBAL.ATTACK_ID, -10, 3, false, 100, 100, rnd));
            moves[8].AddBuff(new Buff(GLOBAL.DEFENCE_ID, -10, 3, false, 100, 100, rnd));
            moves[8].AddBuff(new Buff(GLOBAL.SPEED_ID, -10, 3, false, 100, 100, rnd));
            moves[9] = new Move("Aid Other", 0, 90, 12, false, false);
            moves[9].AddBuff(new Buff(GLOBAL.ATTACK_ID, 30, 1, false, 100, 100, rnd));
            moves[9].AddBuff(new Buff(GLOBAL.DEFENCE_ID, 30, 1, false, 100, 100, rnd));
            moves[9].AddBuff(new Buff(GLOBAL.SPEED_ID, 30, 1, false, 100, 100, rnd));
            moves[10] = new Move("Heal Self", -20, 90, 12, true, false);
            moves[10].AddBuff(new Buff(GLOBAL.HEALTH_ID, 5, 3, false, 95, 100, rnd));
            moves[11] = new Move("Scratch", 20, 80, 10, false, true);
            moves[12] = new Move("Lunge self", 60, 60, 10, false, true);
            moves[12].AddBuff(new Buff(GLOBAL.DEFENCE_ID, -30, 1, true, 100, 100, rnd));
            return moves;
        }
        static Fighter[] FighterLoader(Move[] moves, Random rnd) // This builds the fighter array randomly for testing, based on user input.
        {
            int MAX_MOVES_KNOWN = 4;
            bool inputGood;
            int fighterCount = 0;
            int teamCount = 0;
            int teamInterval = 0;
            int realPlayerCount = 0;
            string[] names = { "Joe", "Rat", "Dog", "Fish", "Log", "Tim", "Fern", "Bob" };
            System.Console.Write("Enter the number of fighters: ");
            while (!int.TryParse(System.Console.ReadLine(), out fighterCount))
            {
                System.Console.WriteLine("Invalid");
            }
            Fighter[] fighters = new Fighter[fighterCount];
            int[] fighterMoves = new int[MAX_MOVES_KNOWN];
            inputGood = false;
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
            inputGood = false;
            while (!inputGood)
            {
                System.Console.Write("Enter the Number of real players: ");
                if (int.TryParse(System.Console.ReadLine(), out realPlayerCount))
                {
                    if (teamCount >= 0)
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
            bool isAI;
            for (int i = 0; i < fighterCount; i++)
            {
                for (int k = 0; k < fighterMoves.Length; k++)
                {
                    fighterMoves[k] = rnd.Next(1, moves.Length);
                }
                if (i <= realPlayerCount - 1)
                {
                    isAI = false;
                }
                else
                {
                    isAI = true;
                }
                if (i == 0)
                    teamInterval = 1;
                else if (i == 1)
                    teamInterval = 2;
                else
                    teamInterval = rnd.Next(1, teamCount + 1);
                fighters[i] = new Fighter(names[rnd.Next(0, names.Length)] + i, 1, 0, 500, 500, rnd.Next(1, 100), rnd.Next(1, 100), rnd.Next(1, 100), isAI, teamInterval, GLOBAL.IntArrayDeepCopy(fighterMoves));
            }
            return fighters;
        }
    }
    class Battle // The objects made from this class are used to manage a battle between givin fighters.
    {
        private Fighter[] fighters;
        private Move[] moves;
        private int turnMax;
        private Random rnd;
        int turnCount;
        bool isBattleDone;
        int[] attackOrder;
        public Battle(Fighter[] fighters, Move[] moves) : this(fighters, moves, GLOBAL.INFINITY, new Random())
        {
        }
        public Battle(Fighter[] fighters, Move[] moves, int turnMax) : this(fighters, moves, turnMax, new Random())
        {
        }
        public Battle(Fighter[] fighters, Move[] moves, Random rnd) : this(fighters, moves, GLOBAL.INFINITY, rnd)
        {
        }
        public Battle(Fighter[] fighters, Move[] moves, int turnMax, Random rnd)
        {
            this.fighters = fighters;
            this.moves = moves;
            this.turnMax = turnMax;
            this.rnd = rnd;
            this.attackOrder = new int[fighters.Length];
            for (int i = 0; i < attackOrder.Length; i++)
            {
                this.attackOrder[i] = i;
            }
        }
        public void StartBattle() // Called to start a Battle objects BattleLoop.
        {
            this.turnCount = 0;
            this.isBattleDone = false;
            this.BattleLoop();
        }
        private void BattleLoop() // This contains the main loop for any given Battle.
        {
            while (!isBattleDone)
            {
                Console.WriteLine("Turn {0}", turnCount + 1);
                // This loop calls for the fighters to generate next moves and target.
                for (int i = 0; i < this.fighters.Length; i++)
                {
                    if (this.fighters[i].StatHealthCurrect > 0)
                    {
                        if (this.fighters[i].IsAI)
                        {
                            AIMoveSelecter(this.fighters[i]);
                            if (!this.moves[fighters[i].NextMove].IsSelfTarget) // If move is not active then its not worth selecting a new target.
                            {
                                AITargetSelecter(this.fighters[i]);
                            }
                        }
                        else // This calls for the users input if isAI is false.
                        {
                            UserInput(this.fighters[i]);
                        }
                    }
                }
                AttackOrderSorter(); // This sorts the fighters turn sequence based on fighters speed and move.
                FighterAttack(); // This enacts the selected move on selected target.
                // This loop calls for buff updates for all fighters.
                for (int i = 0; i < fighters.Length; i++)
                {
                    fighters[i].UpdateBuffs();
                }
                TestGameDone(); // This tests if the fight is over.
            }
        }
        private void AIMoveSelecter(Fighter currectFighter) // This is used by the AI to select their next move.
        {
            // Just selects a random move for now.
            currectFighter.NextMove = this.rnd.Next(0, currectFighter.KnownMoves.Length);
        }
        private void AITargetSelecter(Fighter currectFighter) // This is used by the AI to select their next target.
        {
            bool moveSelected = false;
            int selectedTarget = 0;
            // This selects a random fighter, then checks to see if its valid, a simple brute force method.
            while (!moveSelected)
            {
                selectedTarget = (rnd.Next(0, fighters.Length));
                if (moves[currectFighter.NextMoveID].IsAggressive)
                {
                    if (fighters[selectedTarget].StatHealthCurrect > 0 && fighters[selectedTarget].TeamID != currectFighter.TeamID)
                    {
                        moveSelected = true;
                    }
                }
                else // If the selected move is a support.
                {
                    if (fighters[selectedTarget].TeamID == currectFighter.TeamID)
                    {
                        moveSelected = true;
                    }
                }
            }
            currectFighter.NextTarget = selectedTarget;
        }
        private void UserInput(Fighter currectFighter) // This manages the user inverface for the battle.
        {
            const int MODE_SELECT_STAGE = 1;
            const int MOVE_SELECT_STAGE = 2;
            const int TARGET_SELECT_STAGE = 3;
            const int INFO_STAGE = 4;
            const int DONE_STAGE = 5;
            int currectStage = 1;
            bool stageDone;
            string userInput;
            int convertedInput;
            bool isUserInputDone = false;
            while (!isUserInputDone)
            {
                if (currectStage == MODE_SELECT_STAGE) // This stage is used to for the basic menu.
                {
                    Console.WriteLine("1: Fight\n2: Info\n3: PlaceHolder");
                    Console.Write("Please enter a number: ");
                    stageDone = false;
                    while (!stageDone)
                    {
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out convertedInput))
                        {
                            if (convertedInput == 1) // Move Stage Selected.
                            {
                                stageDone = true;
                                currectStage = MOVE_SELECT_STAGE;
                            }
                            else if (convertedInput == 2) // Info Stage Selected.
                            {
                                stageDone = true;
                                currectStage = INFO_STAGE;
                            }
                            else if (convertedInput == 3) // PlaceHolder Stage Selected.
                            {
                                Console.WriteLine("PlaceHolder");
                                Console.Write("Please enter another number: ");
                            }
                            else
                            {
                                Console.WriteLine("{0} is invalid", userInput);
                                Console.Write("Please enter another number: ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} is invalid", userInput);
                            Console.Write("Please enter another number: ");
                        }
                    }
                }
                else if (currectStage == MOVE_SELECT_STAGE) // This stage is prints all known moves and allows the user to select one.
                {
                    for (int i = 0; i < currectFighter.KnownMoves.Length; i++)
                    {
                        if (currectFighter.KnownMoves[i] != GLOBAL.NullID)
                        {
                            System.Console.WriteLine("{0}: {1}", i + 1, moves[currectFighter.KnownMoves[i]].MoveName);
                        }
                    }
                    System.Console.Write("Please select a move, or B to go back: ");
                    stageDone = false;
                    while (!stageDone)
                    {
                        userInput = System.Console.ReadLine();
                        if (userInput[0] == 'b' || userInput[0] == 'B') // Checks to see if the user wants to go back.
                        {
                            stageDone = true;
                            currectStage = MODE_SELECT_STAGE; // Mode Stage Selected.
                        }
                        else if (int.TryParse(userInput, out convertedInput))
                        {
                            convertedInput -= 1;
                            if (convertedInput >= 0 && convertedInput < currectFighter.KnownMoves.Length && currectFighter.KnownMoves[convertedInput] != 0)
                            {
                                currectFighter.NextMove = convertedInput;
                                System.Console.WriteLine("{0} selected", moves[currectFighter.KnownMoves[currectFighter.NextMove]].MoveName);
                                stageDone = true;
                                if (moves[currectFighter.NextMoveID].IsSelfTarget)
                                {
                                    currectStage = DONE_STAGE; // Done Stage Selected.
                                }
                                else
                                {
                                    currectStage = TARGET_SELECT_STAGE; // Target Stage Selected.
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("{0} is invalid", userInput);
                                System.Console.Write("Please enter another number: ");
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("{0} is invalid", userInput);
                            System.Console.Write("Please enter another number: ");
                        }
                    }
                }
                else if (currectStage == TARGET_SELECT_STAGE) // This stage prints all fighter, and allows user to select one to target.
                {
                    for (int i = 0; i < fighters.Length; i++)
                    {
                        System.Console.WriteLine("{0}: {1} {2}{3}"
                                , i + 1
                                , (currectFighter.TeamID == fighters[i].TeamID) ? "Ally" : "Enemy"
                                , fighters[i].FighterName
                                , (fighters[i].StatHealthCurrect > 0) ? "" : " Dead")
                                ;
                    }
                    System.Console.Write("Please select a target or B to go back: ");
                    stageDone = false;
                    while (!stageDone)
                    {
                        userInput = System.Console.ReadLine();
                        if (userInput[0] == 'b' || userInput[0] == 'B') // Checks to see if the user wants to go back.
                        {
                            stageDone = true;
                            currectStage = MOVE_SELECT_STAGE; // Move Stage Selected.
                        }
                        else if (int.TryParse(userInput, out convertedInput))
                        {
                            convertedInput -= 1;
                            if (convertedInput >= 0 && convertedInput < fighters.Length)
                            {
                                currectFighter.NextTarget = convertedInput;
                                System.Console.WriteLine("{0} targeted", fighters[currectFighter.NextTarget].FighterName);
                                stageDone = true;
                                currectStage = DONE_STAGE; // Done Stage Selected.
                            }
                            else
                            {
                                System.Console.WriteLine("{0} is invalid", userInput);
                                System.Console.Write("Please enter another number: ");
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("{0} is invalid", userInput);
                            System.Console.Write("Please enter another number: ");
                        }
                    }
                }
                else if (currectStage == INFO_STAGE) // This is used for debuging/user info about fighters.
                {
                    //This prints the fighters names with basic info.
                    for (int i = 0; i < fighters.Length; i++)
                    {
                        System.Console.WriteLine("{0}: {1} {2}{3}"
                            , i + 1
                            , (currectFighter.TeamID == fighters[i].TeamID) ? "Ally" : "Enemy"
                            , fighters[i].FighterName
                            , (fighters[i].StatHealthCurrect > 0) ? "" : " Dead"
                            );
                    }
                    System.Console.Write("Please select a target or B to go back: ");
                    stageDone = false;
                    while (!stageDone)
                    {
                        userInput = System.Console.ReadLine();
                        if (userInput[0] == 'b' || userInput[0] == 'B') // Checks to see if the user wants to go back.
                        {
                            stageDone = true;
                            currectStage = MODE_SELECT_STAGE; // Mode Stage Selected.
                        }
                        else if (int.TryParse(userInput, out convertedInput))
                        {
                            convertedInput -= 1;
                            if (convertedInput >= 0 && convertedInput < fighters.Length)
                            {
                                Console.WriteLine("Name: {0}\nMax Health: {1}, Currecnt Health: {2}\nBase Attack: {3}, Currect Attack: {4}" +
                                    "\nBase Defence: {5}, Currect Defence: {6}\nBase Speed: {7}, Current Speed: {8}\nPlayer Controlled: {9}, Team Number: {10}"
                                    , fighters[convertedInput].FighterName
                                    , fighters[convertedInput].StatHealthMax, fighters[convertedInput].StatHealthCurrect
                                    , fighters[convertedInput].StatAttackBase, fighters[convertedInput].StatAttackBuffed
                                    , fighters[convertedInput].StatDefenceBase, fighters[convertedInput].StatDefenceBuffed
                                    , fighters[convertedInput].StatSpeedBase, fighters[convertedInput].StatSpeedBuffed
                                    , !fighters[convertedInput].IsAI, fighters[convertedInput].TeamID
                                    );
                                for (int i = 0; i < fighters[convertedInput].KnownMoves.Length; i++)
                                {
                                    Console.WriteLine("Move {0}: {1}", i + 1, moves[fighters[convertedInput].KnownMoves[i]].MoveName);
                                }
                                Console.WriteLine("Buff count: {0}", fighters[convertedInput].BuffList.Count);
                                for (int i = 0; i < fighters[convertedInput].BuffList.Count; i++)
                                {
                                    Console.WriteLine("Buff {0}: Stat: {1}, Magitude: %{2}, Duration: {3} turns left", i + 1, fighters[convertedInput].BuffList[i].ModdedStatID, fighters[convertedInput].BuffList[i].Magnitude, fighters[convertedInput].BuffList[i].Duration);
                                }
                                stageDone = true;
                                currectStage = MODE_SELECT_STAGE;
                            }
                            else
                            {
                                System.Console.WriteLine("{0} is invalid", userInput);
                                System.Console.Write("Please enter another number: ");
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("{0} is invalid", userInput);
                            System.Console.Write("Please enter another number: ");
                        }
                    }
                }
                else if (currectStage == DONE_STAGE) // This stage is used to finish the userInput, more of a placeholder for now.
                {
                    isUserInputDone = true;
                }
            }
        }
        private void AttackOrderSorter() // This bubble sorts the attackOrder array using the fighters speed * the selected moves speed, larger to the left.
        {
            Fighter currectFighter;
            Fighter previousFighter;
            bool swapped = true;
            while (swapped)
            {
                swapped = false;
                for (int i = 1; i < fighters.Length; i++)
                {
                    currectFighter = fighters[attackOrder[i]];
                    previousFighter = fighters[attackOrder[i - 1]];
                    // This tests if the currectFighter is faster then the previousFighter, if so, it swaps then so that the faster is to the left.
                    if ((previousFighter.StatSpeedBuffed * moves[previousFighter.NextMoveID].MoveSpeed) 
                        < (currectFighter.StatSpeedBuffed * moves[currectFighter.NextMoveID].MoveSpeed))
                    {
                        int temp = attackOrder[i - 1];
                        attackOrder[i - 1] = attackOrder[i];
                        attackOrder[i] = temp;
                        swapped = true;
                    }
                }
            }
        }
        private void FighterAttack() // This enacting the fighters attacks.
        {
            Fighter currectFighter;
            Fighter targetFighter;
            Move currentMove;
            // This loop attacks for all fighters based on AttackOrder.
            for (int i = 0; i < fighters.Length; i++)
            {
                currectFighter = fighters[attackOrder[i]];
                if (currectFighter.StatHealthCurrect > 0)
                {
                    currentMove = moves[fighters[attackOrder[i]].NextMoveID];
                    if (!currentMove.IsSelfTarget)
                    {
                        targetFighter = fighters[fighters[attackOrder[i]].NextTarget];
                        System.Console.WriteLine("{0} Attacked {1} with {2}", currectFighter.FighterName, targetFighter.FighterName, currentMove.MoveName);
                        if (currentMove.MoveAccuracy >= rnd.Next(1, 101)) // This tests to see if move was successful based on moveAccuracy and random number between 1 and 100.
                        {
                            targetFighter.ModifyStatHealth(-(currentMove.MoveDamage * (currectFighter.StatAttackBuffed / targetFighter.StatDefenceBuffed)));
                            System.Console.WriteLine("Attack Hits, {0} is left with {1} health", targetFighter.FighterName, targetFighter.StatHealthCurrect.ToString("0"));
                            // This tests if target was defeted and if so it adds exp to currectFighter.
                            // if two fighers successfully attack a target and target dies with the first hit, then the second will still get EXP, will Possably change in future.
                            if (targetFighter.StatHealthCurrect <= 0)
                            {
                                int rewardEXP = targetFighter.CalcFighterEXPValue();
                                System.Console.WriteLine("{0} has defeated {1} and received {2} EXP", currectFighter.FighterName, targetFighter.FighterName, rewardEXP);
                                currectFighter.AddFighterEXP(rewardEXP);
                            }
                            if (currentMove.BuffList.Count > 0) // If the move has buffs then this adds buffs to fighters buffList.
                            {
                                for (int j = 0; j < currentMove.BuffList.Count; j++)
                                {
                                    if (currentMove.BuffList[j].AddChance >= rnd.Next(1, 101))
                                    {
                                        if (currentMove.BuffList[j].SelfTarget)
                                        {
                                            currectFighter.AddBuff(currentMove.BuffList[j].CloneBuff());
                                            currentMove.BuffList[j].PrintBuffEffect(currectFighter.FighterName);
                                        }
                                        else
                                        {
                                            targetFighter.AddBuff(currentMove.BuffList[j].CloneBuff());
                                            currentMove.BuffList[j].PrintBuffEffect(targetFighter.FighterName);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("{0}s attack missed", currectFighter.FighterName);
                        }
                    }
                    else // This is used if the attack is Self Targeting.
                    {
                        System.Console.WriteLine("{0} used {1}", currectFighter.FighterName, currentMove.MoveName);
                        if (currentMove.MoveAccuracy >= rnd.Next(1, 101))  // This tests to see if move was successful based on moveAccuracy and random number between 1 and 100.
                        {
                            System.Console.WriteLine("{0} was Successful", currectFighter.FighterName);
                            if (currentMove.MoveDamage != 0)
                            {
                                currectFighter.ModifyStatHealth(-(currentMove.MoveDamage * currectFighter.StatAttackBuffed));
                                System.Console.WriteLine("{0} is left with {1} health", currectFighter.FighterName, currectFighter.StatHealthCurrect.ToString("0"));
                            }
                            if (currentMove.BuffList.Count > 0) // If the move has buffs then this adds buffs to fighters buffList.
                            {
                                for (int j = 0; j < currentMove.BuffList.Count; j++)
                                {
                                    if (currentMove.BuffList[j].AddChance >= rnd.Next(1, 101))
                                    {
                                        currectFighter.AddBuff(currentMove.BuffList[j].CloneBuff());
                                        currentMove.BuffList[j].PrintBuffEffect(currectFighter.FighterName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("{0}s move failed", currectFighter.FighterName);
                        }
                    }
                }
                else
                {
                    // This may be enabled if you want dead fighters to be noted per turn.
                   // System.Console.WriteLine("{0} is dead", currectFighter.FighterName);
                }
            }
        }
        private void TestGameDone() // This tests to see if any game over conditions are meet.
        {
            isBattleDone = true;
            int firstTeam = 0;
            // This finds the first living fighters teamID.
            for (int currentFighter = 0; currentFighter < fighters.Length; currentFighter++)
            {
                if (fighters[currentFighter].StatHealthCurrect > 0)
                {
                    firstTeam = fighters[currentFighter].TeamID;
                    break;
                }
            }
            // This looks to see if there are any other living fighters with a different teamID.
            for (int currentFighter = 0; currentFighter < fighters.Length; currentFighter++)
            {
                if (fighters[currentFighter].StatHealthCurrect > 0 && fighters[currentFighter].TeamID != firstTeam)
                {
                    isBattleDone = false;
                }
            }
            // This prints the winners if only 1 teamID remains.
            if (isBattleDone) 
            {
                for (int currentFighter = 0; currentFighter < fighters.Length; currentFighter++)
                {
                    if (fighters[currentFighter].StatHealthCurrect > 0)
                    {
                        System.Console.WriteLine("{0} wins!", fighters[currentFighter].FighterName);
                        for (int i = 0; i < fighters[currentFighter].KnownMoves.Length; i++)
                        {
                            System.Console.WriteLine("Moves {0}", moves[fighters[currentFighter].KnownMoves[i]].MoveName);
                        }
                        System.Console.WriteLine("{0} {1} {2} {3}", fighters[currentFighter].StatHealthMax, fighters[currentFighter].StatAttackBase, fighters[currentFighter].StatDefenceBase, fighters[currentFighter].StatSpeedBase);
                    }
                }
            }
            // This updates the turnCount. and look to see if it has hit the turnMax
            turnCount++;
            if (turnCount == turnMax && !isBattleDone) // if turnMax is set to GLOBAL.INFINITY_ID(-1), then the battle will nevery end due to the timer
            {
                Console.Write("The Clock is up\nNO WINNER\n");
                isBattleDone = true;
            }
        }
    }
    class Fighter //The objects made from this class are used to contain and manage a fighters stats, Buffs and AI.
    {
        private string fighterName;
        private int fighterLevel;
        private int fighterEXP;
        private int statHealthMax;
        private double statHealthCurrect;
        private int statAttackBase;
        private int statDefenceBase;
        private int statSpeedBase;
        private bool isAI;
        private int teamID;
        private int[] knownMoves; // This contains the IDs for the fighters known moves, Example {1,2,4,7}.
        private int nextMove;
        private int nextTarget;
        private List<Buff> buffList = new List<Buff>();
        public Fighter(string fighterName, int fighterLevel, int fighterEXP, int statHealthMax, double statHealthCurrect, int statAttackBase, int statDefenceBase, int statSpeedBase, bool isAI, int teamID, int[] knownMoves)
        {
            this.fighterName = fighterName;
            this.fighterLevel = fighterLevel;
            this.fighterEXP = fighterEXP;
            this.statHealthMax = statHealthMax;
            if (statHealthCurrect > statHealthMax) // statHealthCurrect should never be higher then statHealthMax.
            {
                this.statHealthCurrect = statHealthMax;
            }
            else
            {
                this.statHealthCurrect = statHealthCurrect;
            }
            this.statAttackBase = statAttackBase;
            this.statDefenceBase = statDefenceBase;
            this.statSpeedBase = statSpeedBase;
            this.isAI = isAI;
            this.teamID = teamID;
            this.knownMoves = knownMoves;
        }
        public string FighterName { get { return this.fighterName; } /*set { this.name = value; }*/ }
        public int FighterLevel { get { return this.fighterLevel; } /*set { this.name = value; }*/ }
        public int FighterEXP { get { return this.fighterEXP; } /*set { this.name = value; }*/ }
        public void AddFighterEXP(int EXP)
        {
            if (EXP >= 0)
            {
                this.fighterEXP += EXP;
                if (this.fighterEXP >= (this.fighterLevel * 100))
                {
                    this.fighterLevel++;
                    this.statHealthMax += (fighterLevel * 10);
                    this.ModifyStatHealth(fighterLevel * 5);
                    this.statAttackBase += (fighterLevel * 2);
                    this.statDefenceBase += (fighterLevel * 2);
                    this.statSpeedBase += (fighterLevel * 2);
                    this.fighterEXP -= (this.fighterLevel * 100);
                    Console.Write("!!!!!!!!!!!!!{0} has leveled up to level {1}\n", this.fighterName, this.fighterLevel);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Added EXP must be >= to 0");
            }
        }
        public int StatHealthMax { get { return this.statHealthMax; } /*set { this.statHealthMax = value; }*/ }
        public double StatHealthCurrect { get { return this.statHealthCurrect; } /*set { this.statHealthCurrect = value; }*/ }
        public void ModifyStatHealth(double modification) // Called to relatively change a fighters statHealthCurrect.
        {
            this.statHealthCurrect += modification;
            if (this.statHealthCurrect > this.statHealthMax)
            {
                this.statHealthCurrect = this.statHealthMax;
            }
        }
        public int StatAttackBase { get { return this.statAttackBase; } /*set { this.StatAttack = value; }*/ }
        public double StatAttackBuffed // Returns the StatAttackBase after buffs are taken into account.
        {
            get
            {
                double statModifier = 0;
                for (int i = 0; i < this.buffList.Count; i++)
                {
                    if (this.buffList[i].ModdedStatID == GLOBAL.ATTACK_ID)
                    {
                        statModifier += this.buffList[i].Magnitude;
                    }
                }
                return this.statAttackBase * ((statModifier / 100) + 1);
            }
        }
        public int StatDefenceBase { get { return this.statDefenceBase; } /*set { this.statDefenceBase = value; }*/ }
        public double StatDefenceBuffed // Returns the statDefenceBase after buffs are taken into account.
        {
            get
            {
                double statModifier = 0;
                for (int i = 0; i < this.buffList.Count; i++)
                {
                    if (this.buffList[i].ModdedStatID == GLOBAL.DEFENCE_ID)
                    {
                        statModifier += this.buffList[i].Magnitude;
                    }
                }
                return this.statDefenceBase * ((statModifier / 100) + 1);
            }
        }
        public int StatSpeedBase { get { return this.statSpeedBase; } /*set { this.statSpeedBase = value; }*/ }
        public double StatSpeedBuffed // Returns the statSpeedBase after buffs are taken into account.
        {
            get
            {
                double statModifier = 0;
                for (int i = 0; i < this.buffList.Count; i++)
                {
                    if (this.buffList[i].ModdedStatID == GLOBAL.SPEED_ID)
                    {
                        statModifier += this.buffList[i].Magnitude;
                    }
                }
                return this.statSpeedBase * ((statModifier / 100) + 1);
            }
        }
        public bool IsAI { get { return this.isAI; } /*set { this.isAI = value; }*/ }
        public int TeamID { get { return this.teamID; } /*set { this.teamID = value; }*/ }
        public int[] KnownMoves { get { return GLOBAL.IntArrayDeepCopy(this.knownMoves); } /*set { this.knownMoves = IntArrayDeepClone(value); }*/ }
        public int NextMove
        {
            get { return this.nextMove; }
            set
            {
                if (value < this.knownMoves.Length && value >= 0)
                {
                    this.nextMove = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Selected move is outside of knownMoves index range");
                }
            }
        }
        public int NextMoveID { get { return this.knownMoves[nextMove]; } }
        public int NextTarget
        {
            get { return this.nextTarget; }
            set
            {
                if (value >= 0)
                {
                    this.nextTarget = value;
                }
                else // the target could still be over the index, but that wont be checked here.
                {
                    throw new ArgumentOutOfRangeException("Selected target is outside of possable fighter index range");
                }
            }
        }
        public List<Buff> BuffList { get { return Buff.BuffListDeepCopy(this.buffList); } }
        public void AddBuff(Buff newBuff) //This adds a Buff to the fighters buffList.
        {
            this.buffList.Add(newBuff.CloneBuff());
        }
        public void UpdateBuffs() // This manages the Buffs for a Fighter, such as healing and buff timer decrements.
        {
            // This checks and applies healing buffs.
            for (int i = 0; i < this.buffList.Count; i++)
            {
                if (this.buffList[i].ModdedStatID == GLOBAL.HEALTH_ID)
                {
                    ModifyStatHealth(buffList[i].Magnitude);
                }
            }
            // This decrements all, but permanent(-1), buffList Buffs and removes any at 0.
            for (int i = 0; i < this.buffList.Count;)
            {
                if (this.buffList[i].Duration > 0)
                {
                    this.buffList[i].DecrementDuration();
                }
                if (this.buffList[i].Duration == 0)
                {
                    this.buffList.RemoveAt(i);
                }
                else
                {
                    i++;
                }

            }
        }
        public int CalcFighterEXPValue()
        {
            return (this.fighterLevel * (((this.statHealthMax / 5) + this.statAttackBase + this.statDefenceBase + this.statSpeedBase) / 5));
        }
    }
    class Move //The objects made from this class are used to contain a Moves attributes, and Buffs.
    {
        private static int nextMoveID = 0;
        private int moveID;
        private string moveName;
        private double moveDamage;
        private double moveSpeed;
        private int moveAccuracy;
        private bool isSelfTarget;
        private bool isAggressive;
        private List<Buff> buffList;
        public Move(string moveName, double moveDamage, int moveAccuracy, double moveSpeed, bool isSelfTarget, bool isAggressive) :
            this(moveName, moveDamage, moveAccuracy, moveSpeed, isSelfTarget, isAggressive, new List<Buff>()) //This Constructor is used if no buffList is given.
        {
        }
        public Move(string moveName, double moveDamage, int moveAccuracy, double moveSpeed, bool isSelfTarget, bool isAggressive, List<Buff> buffList)
        {
            this.moveID = nextMoveID++;
            this.moveName = moveName;
            this.moveDamage = moveDamage;
            this.moveAccuracy = moveAccuracy;
            this.moveSpeed = moveSpeed;
            this.isSelfTarget = isSelfTarget;
            this.isAggressive = isAggressive;
            this.buffList = Buff.BuffListDeepCopy(buffList);
        }
        public string MoveName { get { return this.moveName; } /*set { this.moveName = value; }*/ }
        public double MoveDamage { get { return this.moveDamage; } /*set { this.moveDamage = value; }*/ }
        public double MoveSpeed { get { return this.moveSpeed; } /*set { this.moveSpeed = value; }*/ }
        public int MoveAccuracy { get { return this.moveAccuracy; } /*set { this.moveAccuracy = value; }*/ }
        public bool IsSelfTarget { get { return this.isSelfTarget; } /*set { this.isSelfTarget = value; }*/ }
        public bool IsAggressive { get { return this.isAggressive; } /*set { this.isAggressive = value; }*/ }
        public List<Buff> BuffList { get { return Buff.BuffListDeepCopy(this.buffList); } }
        public void AddBuff(Buff newBuff) //This is used to add a Buff to the Moves buffList.
        {
            buffList.Add(newBuff.CloneBuff());
        }
    }
    class Buff //The objects made from this class are used to contain a Buffs attributes, duration, and Deepcopy function.
    {
        private int moddedStatID;
        private double magnitude;
        private int duration;
        private bool selfTarget;
        private int addChance; //This is the chance that the Buff will be added to fighter if the move is successful.
        private int decrementChance; //This is the chance that the Buff will be decremented per turn.
        private Random rnd;
        public Buff(int moddedStatID, double magnitude, int duration, bool selfTarget, int addChance, int decrementChance, Random rnd)
        {
            this.moddedStatID = moddedStatID;
            this.magnitude = magnitude;
            this.duration = duration;
            this.selfTarget = selfTarget;
            this.addChance = addChance;
            this.decrementChance = decrementChance;
            this.rnd = rnd;
        }
        public int ModdedStatID { get { return this.moddedStatID; } /*set { this.moddedStatID = value; }*/ }
        public double Magnitude { get { return this.magnitude; } /*set { this.magnitude = value; }*/ }
        public int Duration { get { return this.duration; } /*set { this.duration = value; }*/ }
        public void DecrementDuration(int durationMod = 1) //Called to decrement duration.
        {
            if (this.decrementChance >= this.rnd.Next(1, 101))
            {
                this.duration -= durationMod;
            }
        }
        public bool SelfTarget { get { return this.selfTarget; } /*set { this.selfTarget = value; }*/ }
        public int AddChance { get { return this.addChance; } /*set { this.addChance = value; }*/ }
        public int DecrementChance { get { return this.decrementChance; } /*set { this.decrementChance = value; }*/ }
        public Buff CloneBuff() // This is used to Deepcopy Buffs, allowing it to be passed by "value".
        {
            return new Buff(this.moddedStatID, this.magnitude, this.duration, this.selfTarget, this.addChance, this.decrementChance, this.rnd);
        }
        public void PrintBuffEffect(string fighterName) // This is called with a fighterName to indicate the effect to user.
        {
            if (this.moddedStatID == GLOBAL.HEALTH_ID)
            {
                if (this.magnitude > 0)
                {
                    Console.Write("{0} feels healthier\n", fighterName);
                }
                else
                {
                    Console.Write("{0} feels ill\n", fighterName);
                }
            }
            else if (this.moddedStatID == GLOBAL.ATTACK_ID)
            {
                if (this.magnitude > 0)
                {
                    Console.Write("{0} feels stronger\n", fighterName);
                }
                else
                {
                    Console.Write("{0} feels weaker\n", fighterName);
                }
            }
            else if (this.moddedStatID == GLOBAL.DEFENCE_ID)
            {
                if (this.magnitude > 0)
                {
                    Console.Write("{0} feels tougher\n", fighterName);
                }
                else
                {
                    Console.Write("{0} feels fragile\n", fighterName);
                }
            }
            if (this.moddedStatID == GLOBAL.SPEED_ID)
            {
                if (this.magnitude > 0)
                {
                    Console.Write("{0} feels agile\n", fighterName);
                }
                else
                {
                    Console.Write("{0} feels stiff\n", fighterName);
                }
            }
        }
        public static List<Buff> BuffListDeepCopy(List<Buff> oldList) //This is used to Deepcopy Buff lists, allowing it to be passed by "value".
        {
            List<Buff> newList = new List<Buff>();
            for (int i = 0; i < oldList.Count; i++)
            {
                newList.Add(oldList[i].CloneBuff());
            }
            return newList;
        }
    }
    static class GLOBAL //This class holds the programs GLOBAL constants.
    {
        public const int HEALTH_ID = 0;
        public const int ATTACK_ID = 1;
        public const int DEFENCE_ID = 2;
        public const int SPEED_ID = 3;
        public const int INFINITY = -1;
        public const int NullID = 0;
        public const bool DEGUG_MODE = true;
        static public int[] IntArrayDeepCopy(int[] oldArray) //This is used to Deepcopy int arrays, allowing it to be passed by "value".
        {
            int[] newArray = new int[oldArray.Length];
            for (int i = 0; i < oldArray.Length; i++)
            {
                newArray[i] = oldArray[i];
            }
            return newArray;
        }

    }
}

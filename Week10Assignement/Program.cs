using System;
using System.Collections.Generic;

namespace Week10Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
           
            //create Player and Enemy
            Player player = new Player("Hero");
            Enemy enemy = new Enemy("Bandit");
            
            //mark
            player.enemyTarget = enemy.name;
            enemy.enemyTarget = player.name;

            //set player max and current hp, potions will always heal half of the player's Max HP
            player.maximumHP = 20;
            player.currentHP = 20;

            //set the enemy's max and current HP, if both player and enemy deal 10 damage each turn, than the player will always die first, making the player want to heal to above 10 hp to take a crit 10.
            enemy.maximumHP = 25;
            enemy.currentHP = 25;

            //create a list of Fighters (the turn order), and create and add the combatants to it.
            Fighter[] combat = new Fighter[2]; // create list to hope the fighters
            combat[0] = player;
            combat[1] = enemy;

            int damageDealt = 0; // hold the damage dealt by the fighter and give it to the next fighter after them.

            Boolean firstTurn = true; //prevent the loop from hurting the fighter that goes first at the start.
            Boolean playerTurn = true; //check whoses fighting

            /*
            Console.WriteLine(player.name + " HP: " + player.currentHP); //[debug] notify the user of the player's current HP
            Console.WriteLine(enemy.name + " HP: " + enemy.currentHP); //[debug] notify the user of the enemy's current HP
            Console.WriteLine("--------------"); //[debug] seperate for better reading of the output
            //*/

            Boolean Battle = true; //keep the foreach loop going until program notices that one of the fighter's has died

            while (Battle == true)
            {
                foreach (Fighter a in combat)
                {
                    if (firstTurn == true || Battle == false)
                    {
                        //avoid anyone taking damage for being first to take action
                        firstTurn = false; //first turn is over
                    }
                    else
                    {
                        a.takeDamage(damageDealt); // first, the damage from the previous fighter will now be dealt to the current fighter on the list
                    }
                    
                    
                    if(a.alive == false)
                    {
                        Battle = false; //The game loop should continue until one fighter remains
                    }
                    else
                    {
                        if (Battle == true) //while the battle continues
                        {
                            if(playerTurn == true) //check if the it's the player's turn
                            {
                                playerTurn = false; //turn off until enemy has taken their turn

                                Console.WriteLine("\n === Player Turn ===");
                                Console.WriteLine(" " + player.name + "'s Health: " + player.currentHP);
                                Console.WriteLine(" " + enemy.name + "'s Health: " + enemy.currentHP);
                                Console.WriteLine(" What would you like to do?");
                                Console.WriteLine(" 1 - Fight");
                                Console.WriteLine(" 2 - Drink Potion" + "(" + player.potionCount + " remaining)");
                                damageDealt = a.takeAction(); //then the current fighter will deal damage.
                            }
                            else
                            {
                                playerTurn = true; //next time it's the player's turn
                                damageDealt = a.takeAction(); //then the current fighter will deal damage.
                            }
                        }
                        else
                        {
                            //don't make the fighter take a action again
                        }
                    }

                }
            }

            if(player.alive == true) //a victory/loss message should be displayed.
            {
                Console.WriteLine("\n Victory!\n The player has won!");
            }
            else
            {
                Console.WriteLine("\n Defeated!\n The player has lost!");
            }

            //Console.WriteLine(player.name + " " + player.currentHP); [Debug] check the hp of the player after the game is over
            //Console.WriteLine(enemy.name + " " + enemy.currentHP); [Debug] check the hp of the enemy after the game is over



        }

        abstract class Fighter
        {
            public string name;

            public int currentHP;       // current and maximum health
            public int maximumHP;

            public Boolean alive = true;// Whether the fighter is dead

            public string enemyTarget;  // a reference to another fighter, an opponent


            public Fighter(string n)    //set the name of the fighter
            {
                name = n;
            }

            public void setMaxHP(int H) // set max HP
            {
                maximumHP = H;
            }

            public void setCurrentHP(int H) //change current HP
            {
                currentHP = H;
            }

            public void takeDamage(int D) //a method for taking damage (decrease current health by a given amount, call death function if health reaches 0)
            {
                Console.WriteLine(" "+ name + " took " + D + " damage!");
                currentHP = currentHP - D;

                //Console.WriteLine("current hp: " + currentHP);

                if (currentHP <= 0) // call death function
                {
                    HPcheck(currentHP);
                }
            }

            void HPcheck(int H) //a method for when the fighter dies (set its "dead" variable to true)
            {
                if (currentHP > 0) // if your current HP is above 0, then the Fighter is still alive
                {
                    Console.WriteLine("i'll live!");
                    alive = true;
                }
                else // else then they died
                {
                    Console.WriteLine(" the " + name +" has died!");
                    currentHP = 0;
                    alive = false;
                }
            }
          
            public virtual int takeAction() //-a method for taking an action, which will be overridden by the Player and Enemy classes (the player will have a choice of options, while the enemy will simply attack the player)
            {
                Console.WriteLine(" an normal attack at " + enemyTarget + "!");
                return 0;
            }
        }
        //*
        class Enemy : Fighter
        {

            Random random = new Random(); //create a random chance that determines that damage of the attack

           public Enemy(string n) : base (n) //create Enemy
            {
                name = n;
            }

            public override int takeAction()
            {
                Console.WriteLine("\n " + name + " took a dirty stab at the "+ enemyTarget + "!");
                return 5+(random.Next(6)); //deal 5 damage and addition damage ranging from 1 to 5.
            }
        }

        class Player : Fighter
        {
            public int potionCount = 5; //determine how many potions the player will have
            

            Random random = new Random(); //add some classic JRPG random chance, used to determine the damage either figher will do

            public Player(string n) : base(n) //create and set the name of the player
            {
                name = n;
            }

            public override int takeAction() //allow the player to either deal damage or heal themselves withh their limited amount of potions
            {
                int choice = 3; //set this to a value that won't trigger if parameters
                do
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1)
                    {
                        Console.WriteLine(" A heroic strike against the "+ enemyTarget +"!");
                        return 5 + (random.Next(6));
                    }
                    else if (choice == 2)
                    {
                        drinkPotion();
                        return 0; // due to taking the turn to heal, the player will do no damage. if they have no potions than they'll waste a turn realzing they ran out of potion juice.
                    }
                    else
                    {
                        Console.WriteLine("Error: please input 1 or 2");
                    }
                }while (choice != 1 || choice !=2);

                choice = 3; //reset for next time

                return 0; //in case of huge error, return no damage
            }

            public void drinkPotion() // drinkPotion should restore some of the player's health
            {
                if (potionCount > 0)
                {
                    Console.WriteLine(name + " takes a swig of their potion! +" + (maximumHP/2) +" HP!" ); //notify the user that the player has drank the potion
                    
                    potionCount--; //remove one potion from current capacity
                    
                    currentHP += (maximumHP / 2); //heal for 50% of the player's max hp //[student note]: add some RNG later on, like 15% max hp plus 1 to 5

                    if(currentHP >= maximumHP) // if player heals over the maximum hp, set current hp to max hp
                    {
                        currentHP = maximumHP;
                    }

                }
                else if (potionCount <= 0) // Do NOT allow the player to use a potion if they are out of them!
                {
                    Console.WriteLine(name +" trys to take a swig of their potion but they're all out!");
                }
            }
        }
        //*/
    }
}

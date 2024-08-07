namespace COIS2020.vrajchauhan0827768.Assignments;

using System.Xml.Schema;
using System.Linq;
using Microsoft.Xna.Framework; // Needed for `Vector2`

public class WizardFighterDX : Visualizer
{
    private const int NUM_WIZARDS = 2; // Number of wizards to start with.
    private const int NUM_GOBLINS = 2; // Number of goblins to start with.
    private ArrayList<CombatEntity> AllEntities { get; set; }  // All entities arrayList in the game.
    private ArrayList<Goblin> Goblins { get; set; } // All goblins arrayList in the game.
    private ArrayList<Wizard> Wizards { get; set; } // All wizards arrayList in the game.

    private bool isBossGoblin = false; // Boss goblin flag.

    #region Game Initialization
    /// <summary>
    /// Creates a new instance of the game.
    /// </summary>
    public WizardFighterDX()
    {
        Goblins = new ArrayList<Goblin>();
        Wizards = new ArrayList<Wizard>();
        AllEntities = new ArrayList<CombatEntity>();

        // Add the initial entities to the game.
        for (int i = 0; i < NUM_WIZARDS; i++)

            Wizards.AddBack(new Wizard()); // Add a new wizard to the wizards arrayList.

        for (int i = 0; i < NUM_GOBLINS; i++)

            Goblins.AddBack(new Goblin()); // Add a new goblin to the goblins arrayList.

        foreach (Goblin goblin in Goblins)

            AllEntities.AddBack(goblin); // Add all goblins to the allEntities arrayList.

        foreach (Wizard wizard in Wizards)

            AllEntities.AddBack(wizard); // Add all wizards to the allEntities arrayList.

        AllEntities.Sort(reverse: true); // Sort all entities by HP in descending order.
    }

    #endregion
    protected override IEnumerable<CombatEntity> GetEntities()
    {
        return AllEntities;
    }

    #region Main Game Logic
    /// <summary>
    /// Updates the game state.
    /// </summary>
    protected override void Update()
    {
        foreach (Goblin goblin in Goblins)
        {
            updateGoblin(goblin);
        }
        foreach (Wizard wizard in Wizards)
        {
            updateWizard(wizard);
        }
        AddGoblinAfter15Ticks();
        AddWizardAfter50Ticks();

        checkEntities(Goblins, Wizards);
        checkGameOver();
    }
    #endregion

    #region Add methods
    protected void AddGoblinAfter15Ticks() // Add goblin after 15 ticks method
    {
        if (CurrentTimestamp % 15 == 0 && !isBossGoblin) // If the current timestamp is divisible by 15 and we don't have a boss goblin
        {
            Goblins.AddBack(new Goblin()); // Add a new goblin
            Goblin goblin = Goblins.Get(Goblins.Count - 1); // Get the goblin
            AllEntities.InsertAt(getSortedIndex(goblin), goblin); // Insert the goblin into the allEntities arrayList
        }
    }
    protected void AddWizardAfter50Ticks() // Add wizard after 50 ticks method
    {
        if (CurrentTimestamp % 50 == 0) // If the current timestamp is divisible by 50
        {
            Wizards.AddBack(new Wizard()); // Add a new wizard
            Wizard wizard = Wizards.Get(Wizards.Count - 1); // Get the wizard
            AllEntities.InsertAt(getSortedIndex(wizard), wizard); // Insert the wizard into the allEntities arrayList
        }
    }

    #endregion


    #region Update methods

    // Update methods
    // --------------------------------
    protected void updateWizard(Wizard wizard) // Update wizard method
    {
        if (wizard.CanAttack(CurrentTimestamp)) // If wizard can attack
        {
            for (int i = 0; i < Goblins.Count; i++) // Loop through all goblins
            {
                Goblin goblin = Goblins.Get(i); // Get the goblin
                if (wizard.DistanceTo(goblin) <= wizard.SpellRange) // If the distance between the wizard and the goblin is less than or equal to the wizard's spell range
                {
                    LogMessage($"Wizard {wizard} is attacking Goblin {goblin} with spell level {wizard.SpellLevel}."); // Log the message
                    wizard.Attack(goblin, (int)Math.Ceiling(wizard.SpellLevel / wizard.DistanceTo(goblin)), CurrentTimestamp); // Wizard attacks the goblin

                    goblin.PushAwayFrom(wizard, 1.50f); // Push the goblin away from the wizard
                    goblin.ClampPosition(EntityXRange, EntityYRange); // Clamp the goblin's position
                }
                else // If the distance between the wizard and the goblin is greater than the wizard's spell range
                {
                    wizard.MoveTowards(getClosestGoblin(wizard, Goblins), 0.50f); // Move the wizard towards the closest goblin
                    wizard.ClampPosition(EntityXRange, EntityYRange); // Clamp the wizard's position
                }
            }
        }
        else
        {
            wizard.MoveTowards(getClosestGoblin(wizard, Goblins), 0.50f); // Move the wizard towards the closest goblin
            wizard.ClampPosition(EntityXRange, EntityYRange); // Clamp the wizard's position
        }
    }
    protected void updateGoblin(Goblin goblin)
    {
        moveGoblinRandomly(goblin); // Move the goblin randomly


        if (goblin.CanAttack(CurrentTimestamp)) // If the goblin can attack
        {
            for (int i = 0; i < Wizards.Count; i++) // Loop through all wizards
            {
                Wizard wizard = Wizards.Get(i); // Get the wizard
                if (isLeastHP(wizard) && goblin.DistanceTo(wizard) <= goblin.AttackRange) // If the wizard has the least HP and the distance between the goblin and the wizard is less than or equal to the goblin's attack range
                {
                    LogMessage($"Goblin {goblin} is attacking Wizard {wizard} with attack power {goblin.AttackPower}."); // Log the message
                    goblin.Attack(wizard, goblin.AttackPower, CurrentTimestamp); // Goblin attacks the wizard
                    if (goblin.HP < 0.25 * goblin.MaxHP) // If the goblin's HP is less than 0.25 of the goblin's max HP
                    {
                        goblin.HP += (wizard.MaxHP - wizard.HP) / 2; // Add half of the difference between the wizard's max HP and the wizard's HP to the goblin's HP
                    }
                    wizard.PushAwayFrom(goblin, 1.50f); // Push the wizard away from the goblin
                    wizard.ClampPosition(EntityXRange, EntityYRange); // Clamp the wizard's position
                }
            }
        }
    }
    #endregion

    #region Check methods

    protected void checkEntities(ArrayList<Goblin> Goblins, ArrayList<Wizard> Wizards) // Check entities method
    {
        for (int i = 0; i < Goblins.Count; i++) // Loop through all goblins
        {
            if (Goblins.Get(i).HP <= 0) // If the goblin's HP is less than or equal to 0
            {
                AllEntities.RemoveAt(AllEntities.FindIndex(Goblins.RemoveAt(i))); // Remove the goblin from the allEntities arrayList and the goblins arrayList
                if (Goblins.Count == 0 && !isBossGoblin) // If there are no goblins left and we don't have a boss goblin
                {
                    LogMessage("BOSS TIME"); // Log the message
                    Goblins.AddBack(new Goblin(isBossGoblin: true)); // Add a new boss goblin
                    Goblin goblin = Goblins.Get(Goblins.Count - 1); // Get the boss goblin
                    AllEntities.InsertAt(getSortedIndex(goblin), goblin); // Insert the boss goblin into the allEntities arrayList
                    isBossGoblin = true; // Set the boss goblin flag to true
                }
            }
        }
        for (int i = 0; i < Wizards.Count; i++) // Loop through all wizards
        {
            if (Wizards.Get(i).HP <= 0) // If the wizard's HP is less than or equal to 0
            {
                AllEntities.RemoveAt(AllEntities.FindIndex(Wizards.RemoveAt(i))); // Remove the wizard from the allEntities arrayList and the wizards arrayList

            }
        }
    }
    protected void checkGameOver() // Check game over method
    {
        if (Wizards.Count == 0 || Goblins.Count == 0) // If there are no wizards left or no goblins left
        {
            Stop(); // Stop the game
        }
    }


    #endregion

    #region Helper methods
    protected void moveGoblinRandomly(Goblin goblin) //Move the goblin randomly method
    {
        float dx = (RNG.NextSingle() * 2 - 1) * 0.60f; // Randomly generate a float between -1 and 1 and multiply it by the speed of the goblin
        float dy = (RNG.NextSingle() * 2 - 1) * 0.60f; // Randomly generate a float between -1 and 1 and multiply it by the speed of the goblin
        goblin.Move(dx, dy); // Move the goblin
        goblin.ClampPosition(EntityXRange, EntityYRange); // Clamp the goblin's position
    }


    protected Goblin getClosestGoblin(Wizard wizard, ArrayList<Goblin> goblins) // Get the closest goblin method
    {
        Goblin closestGoblin = goblins.Get(0); // Get the first goblin
        foreach (Goblin goblin in goblins) // Loop through all goblins
        {
            if (wizard.DistanceTo(goblin) < wizard.DistanceTo(closestGoblin)) // If the distance between the wizard and the goblin is less than the distance between the wizard and the closest goblin
            {
                closestGoblin = goblin; // Set the closest goblin to the goblin
            }
        }
        return closestGoblin; // Return the closest goblin
    }


    protected int getSortedIndex(CombatEntity entity) // Get the sorted index method
    {
        // Binary search to find the index where the entity should be inserted.
        int left = 0; // Left index
        int right = AllEntities.Count - 1; // Right index

        while (left <= right) // While the left index is less than or equal to the right index
        {
            int mid = (left + right) / 2; // Calculate the middle index
            if (entity.MaxHP >= AllEntities.Get(mid).MaxHP) // If maxHP of the entity is greater than or equal to the maxHP of the entity at the middle index
            {
                right = mid - 1; // Set the right index to the middle index - 1
            }
            else
            {
                left = mid + 1; // Set the left index to the middle index + 1
            }
        }
        return left; // Return the left index
    }

    protected bool isLeastHP(Wizard wizard) // Is least HP method
    {
        foreach (Wizard w in Wizards) // Loop through all wizards
        {
            if (w.HP < wizard.HP) // If the HP of the wizard is less than the HP of the wizard
            {
                return false; // Return false if the HP of the wizard is not the least
            }

        }
        return true; // Return true if the HP of the wizard is the least
    }

}

#endregion
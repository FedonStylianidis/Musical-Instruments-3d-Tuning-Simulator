
using System.Collections.Generic;
using UnityEngine;

public class SnareTuningAlgorithm : MonoBehaviour
{
    private const int NumberOfLugs = 10;


    // Stores every lug that has already
    // been successfully tuned.
    private List<int> TunedLugs =
        new ();


    // False before the first lug has
    // been successfully tuned.
    private bool AlgorithmStarted;


    // After the first lug, the next step
    // must always be its opposite.
    //
    // After an opposite lug is tuned,
    // the next step becomes a skip-one step.
    private bool NextLugShouldBeOpposite = true;


    // The most recently successfully tuned lug.
    //
    // All next-lug calculations are made
    // relative to this lug.
    private int LastTunedLug = -1;



    // ------------------------------------------------
    // OPPOSITE LUG
    // ------------------------------------------------

    public int getOppositeLug(
        int lugNumber)
    {
        int opposite =
            lugNumber + 5;


        // Wrap around the 1-10 numbering.
        if (opposite > NumberOfLugs)
        {
            opposite -= NumberOfLugs;
        }


        return opposite;
    }



    // ------------------------------------------------
    // SKIP ONE TO THE RIGHT
    // ------------------------------------------------

    public int getSkipRightLug(
        int lugNumber)
    {
        int nextLug =
            lugNumber + 2;


        // Example:
        //
        // 9 + 2 = 11 -> 1
        // 10 + 2 = 12 -> 2

        if (nextLug > NumberOfLugs)
        {
            nextLug -= NumberOfLugs;
        }


        return nextLug;
    }



    // ------------------------------------------------
    // SKIP ONE TO THE LEFT
    // ------------------------------------------------

    public int getSkipLeftLug(
        int lugNumber)
    {
        int nextLug =
            lugNumber - 2;


        // Example:
        //
        // 1 - 2 = -1 -> 9
        // 2 - 2 =  0 -> 10

        if (nextLug < 1)
        {
            nextLug += NumberOfLugs;
        }


        return nextLug;
    }



    // ------------------------------------------------
    // CHECK WHETHER A LUG HAS ALREADY BEEN TUNED
    // ------------------------------------------------

    public bool isLugTuned(
        int lugNumber)
    {
        return TunedLugs.Contains(
            lugNumber
        );
    }



    // ------------------------------------------------
    // CHECK WHETHER THE PLAYER MAY CHOOSE THIS LUG
    // ------------------------------------------------

    public bool isValidNextLug(
        int lugNumber)
    {
        // Before the first lug has been tuned,
        // the player may start from ANY lug.
        if (!AlgorithmStarted)
        {
            return true;
        }


        // A lug that has already been completed
        // cannot be selected as a new tuning step.
        if (isLugTuned(lugNumber))
        {
            return false;
        }


        // --------------------------------------------
        // OPPOSITE STEP
        // --------------------------------------------
        //
        // There is exactly ONE correct choice.

        if (NextLugShouldBeOpposite)
        {
            int oppositeLug =
                getOppositeLug(
                    LastTunedLug
                );


            return lugNumber ==
                oppositeLug;
        }


        // --------------------------------------------
        // SKIP-ONE STEP
        // --------------------------------------------
        //
        // There may be TWO correct choices:
        //
        // LastTunedLug + 2
        //
        // or
        //
        // LastTunedLug - 2
        //
        // The player is allowed to choose either,
        // provided that lug has not already
        // been tuned.

        int rightLug =
            getSkipRightLug(
                LastTunedLug
            );


        int leftLug =
            getSkipLeftLug(
                LastTunedLug
            );


        bool rightIsValid =
            !isLugTuned(
                rightLug
            );


        bool leftIsValid =
            !isLugTuned(
                leftLug
            );


        if (lugNumber == rightLug &&
            rightIsValid)
        {
            return true;
        }


        if (lugNumber == leftLug &&
            leftIsValid)
        {
            return true;
        }


        return false;
    }



    // ------------------------------------------------
    // REGISTER A SUCCESSFULLY TUNED LUG
    // ------------------------------------------------

    public void registerTunedLug(
        int lugNumber)
    {
        // Don't register the same lug twice.
        if (isLugTuned(lugNumber))
        {
            return;
        }


        TunedLugs.Add(
            lugNumber
        );


        LastTunedLug =
            lugNumber;


        // --------------------------------------------
        // FIRST LUG
        // --------------------------------------------
        //
        // The first lug establishes the starting
        // position.
        //
        // The next lug must be its opposite.

        if (!AlgorithmStarted)
        {
            AlgorithmStarted = true;

            NextLugShouldBeOpposite =
                true;

            return;
        }


        // --------------------------------------------
        // ADVANCE THE PATTERN
        // --------------------------------------------
        //
        // If this lug was reached through an
        // opposite step, next we skip one.
        //
        // If this lug was reached through a
        // skip-one step, next we go opposite.

        NextLugShouldBeOpposite =
            !NextLugShouldBeOpposite;
    }



    // ------------------------------------------------
    // COMPLETION
    // ------------------------------------------------

    public bool isComplete()
    {
        return TunedLugs.Count >=
            NumberOfLugs;
    }



    // ------------------------------------------------
    // RESET
    // ------------------------------------------------

    public void resetAlgorithm()
    {
        TunedLugs.Clear();

        AlgorithmStarted =
            false;

        NextLugShouldBeOpposite =
            true;

        LastTunedLug =
            -1;
    }
}
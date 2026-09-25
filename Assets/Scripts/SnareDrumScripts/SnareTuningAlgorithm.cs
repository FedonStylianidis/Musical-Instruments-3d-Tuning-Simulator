/*using System.Collections.Generic;
using UnityEngine;

public class SnareTuningAlgorithm : MonoBehaviour
{
    private const int NumberOfLugs = 10;

    private List<int> TunedLugs =
        new List<int>();

    private bool NextLugShouldBeOpposite = true;

    private int ExpectedLug = -1;

    private bool AlgorithmStarted = false;


    public int getOppositeLug(
        int lugNumber)
    {
        int opposite =
            lugNumber + 5;

        if (opposite > NumberOfLugs)
        {
            opposite -= NumberOfLugs;
        }

        return opposite;
    }


    public int getSkipRightLug(
        int lugNumber)
    {
        int nextLug =
            lugNumber + 2;

        if (nextLug > NumberOfLugs)
        {
            nextLug -= NumberOfLugs;
        }

        return nextLug;
    }


    public int getSkipLeftLug(
        int lugNumber)
    {
        int nextLug =
            lugNumber - 2;

        if (nextLug < 1)
        {
            nextLug += NumberOfLugs;
        }

        return nextLug;
    }


    public void markLugAsTuned(
        int lugNumber)
    {
        if (!TunedLugs.Contains(lugNumber))
        {
            TunedLugs.Add(lugNumber);
        }
    }


    public bool isLugTuned(
        int lugNumber)
    {
        return TunedLugs.Contains(lugNumber);
    }


    public int getNextSkippedLug(
        int lugNumber)
    {
        int rightLug =
            getSkipRightLug(lugNumber);

        int leftLug =
            getSkipLeftLug(lugNumber);


        bool rightIsTuned =
            isLugTuned(rightLug);

        bool leftIsTuned =
            isLugTuned(leftLug);


        if (!rightIsTuned &&
            leftIsTuned)
        {
            return rightLug;
        }


        if (rightIsTuned &&
            !leftIsTuned)
        {
            return leftLug;
        }


        if (!rightIsTuned &&
            !leftIsTuned)
        {
            if (Random.value < 0.5f)
                return rightLug;
            else
                return leftLug;
        }


        return -1;
    }


    public int getNextLug(
        int currentLug)
    {
        int nextLug;


        if (NextLugShouldBeOpposite)
        {
            nextLug =
                getOppositeLug(
                    currentLug
                );

            NextLugShouldBeOpposite =
                false;
        }
        else
        {
            nextLug =
                getNextSkippedLug(
                    currentLug
                );

            NextLugShouldBeOpposite =
                true;
        }


        return nextLug;
    }


    public void registerTunedLug(
        int lugNumber)
    {
        markLugAsTuned(
            lugNumber
        );


        // The first correctly tuned lug
        // starts the tuning exercise.
        if (!AlgorithmStarted)
        {
            AlgorithmStarted = true;

            ExpectedLug =
                getNextLug(
                    lugNumber
                );

            return;
        }


        // All ten lugs are now tuned.
        if (TunedLugs.Count >= NumberOfLugs)
        {
            ExpectedLug = -1;

            return;
        }


        ExpectedLug =
            getNextLug(
                lugNumber
            );
    }


    public bool isCorrectLug(
        int lugNumber)
    {
        // Before the exercise starts,
        // any lug may be chosen first.
        if (!AlgorithmStarted)
            return true;

        return lugNumber ==
            ExpectedLug;
    }

    public bool isValidNextLug(
    int lugNumber)
    {
        // Before the algorithm starts,
        // ANY lug is valid as the first lug.
        if (!AlgorithmStarted)
            return true;


        // ---------------------------------
        // OPPOSITE STEP
        // ---------------------------------
        //
        // There is only one valid lug:
        // the exact opposite of the
        // previously tuned lug.
        if (NextLugShouldBeOpposite)
        {
            int oppositeLug =
                getOppositeLug(
                    TunedLugs[
                        TunedLugs.Count - 1
                    ]
                );

            return lugNumber ==
                oppositeLug;
        }


        // ---------------------------------
        // SKIP-ONE STEP
        // ---------------------------------
        //
        // There can be TWO valid choices:
        //
        // current + 2
        // current - 2
        //
        // provided they have not already
        // been tuned.

        int currentLug =
            TunedLugs[
                TunedLugs.Count - 1
            ];


        int rightLug =
            getSkipRightLug(
                currentLug
            );

        int leftLug =
            getSkipLeftLug(
                currentLug
            );


        bool rightIsValid =
            !isLugTuned(rightLug);

        bool leftIsValid =
            !isLugTuned(leftLug);


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
    public bool isComplete()
    {
        return TunedLugs.Count >=
            NumberOfLugs;
    }


    public int getExpectedLug()
    {
        return ExpectedLug;
    }


    public void resetAlgorithm()
    {
        TunedLugs.Clear();

        ExpectedLug = -1;

        AlgorithmStarted = false;

        NextLugShouldBeOpposite = true;
    }
} */
using System.Collections.Generic;
using UnityEngine;

public class SnareTuningAlgorithm : MonoBehaviour
{
    private const int NumberOfLugs = 10;


    // Stores every lug that has already
    // been successfully tuned.
    private List<int> TunedLugs =
        new List<int>();


    // False before the first lug has
    // been successfully tuned.
    private bool AlgorithmStarted = false;


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
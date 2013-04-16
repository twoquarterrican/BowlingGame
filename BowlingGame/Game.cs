using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingGame {
    public class Game {

        /*
         * A frame consists of two bowls, the sum of the pins on the two bowls is at most 10.
         * A roll occurs when the ball is rolled down the lane.  Every roll is a bowl, but the bowls occuring after a strike
         * in frames 1-9 are not rolls.
         */

        //10 frames
        private const int NUM_FRAMES = 10;
        //10 frames times 2 rolls per frame plus bonus possible extra bowl
        private const int MAX_BOWL = 2 * NUM_FRAMES + 1;
        //record number of pins for each bowl
        private int[] pins = new int[MAX_BOWL];
        //keep track of the Bowl number
        private int currentBowl = 0;
        //keep track of the Bowl numbers as a function of the rolls.  There can also be up to 21 rolls
        private int[] bowl = new int[MAX_BOWL];
        //keep track of the roll number
        private int currentRoll = 0;

        public Game() {
        }

        public void roll(int pinsThisRoll) {

            //cannot have a negative number of pins
            if (pinsThisRoll < 0) throw new ArgumentOutOfRangeException("number of pins must be greater than 0");

            //record that this roll corresponds to this bowl and increment roll number
            bowl[currentRoll++] = currentBowl;

            //record number of pins for this roll and increment bowl number
            //the assignment pins[currentBowl++] = pinsThisRoll must occur after the negative value test, but
            //before the too many pin tests, since the too many pin tests use the values in the array pins and are
            //guaranteed to work only if those values are non-negative
            pins[currentBowl] = pinsThisRoll;

            //cannot have more than ten pins in any frame but the last
            if (!isLastFrame(currentBowl) && pinsInFrame(computeFrame(currentBowl)) > 10) throw new ArgumentOutOfRangeException("cannot hit more than ten pins in a single frame");
            //checking for too many in last frame is a bit tricky since strikes allow up to 30 pins in last frame
            if (isLastFrame(currentBowl) && tooManyInLastFrame()) throw new ArgumentOutOfRangeException("too many pins in the last frame");
            //if strike occured on this bowl and this is not on last frame, we don't roll the next bowl, and the next bowl is 0 pins
            if (isStrikeBeforeLastFrame(currentBowl)) {
                //increment bowl number again before assigning array value
                //record 0 since there are no pins left
                pins[++currentBowl] = 0;
            }
            //bowl number always increments by at least one
            currentBowl++;
        }

        public int getScore() {
            int score = 0;
            //compute bonuses and accumulate score
            for (int roll = 0; roll < currentRoll; roll++) {
                score += pins[bowl[roll]];
                if (isStrikeBeforeLastFrame(bowl[roll])) {
                    //strike bonus counts next two rolls, not next two bowls
                    score += pins[bowl[roll + 1]] + pins[bowl[roll + 2]];
                }
                else if (isSpareBeforeLastFrame(bowl[roll])) {
                    //spare bonus counts next roll
                    score += pins[bowl[roll + 1]];
                }
            }
            return score;
        }

        private Boolean isLastFrame(int bowl) {
            //last frame is bowls 18,19,20
            return (bowl >= MAX_BOWL - 3);
        }

        private int pinsInFrame(int frame) {
            //frames 0 is bowls 0 and 1, frame 1 is bowls 2 and 3, ..., frame 8 is bowls 16 and 17
            //frame 9 is bowls 18, 19, and 20
            if (frame < NUM_FRAMES - 1) {
                return pins[2 * frame] + pins[2 * frame + 1];
            }
            else {
                return pins[MAX_BOWL - 3] + pins[MAX_BOWL - 2] + pins[MAX_BOWL - 1];
            }
        }

        private int computeFrame(int bowl) {
            //bowl 0 and 1 in frame 0, bowl 2 and 3 in frame 1, ..., bowls 18, 19, and 20 in frame 9
            int frame = bowl / 2;
            if (frame > NUM_FRAMES - 1) frame = NUM_FRAMES - 1;
            return frame;
        }

        private Boolean tooManyInLastFrame() {
            //check that last frame does not have too many pins.  Can have at most 30 pins.
            //Can think of this as maximum of 10 pins total where every strike counts as 0,
            //and a spare on 18 and 19 also counts as 0.  A simple way to cover all these
            //cases is to keep a running total of pins on 18, 19, and 20, and reset the
            //running total if it reaches exactly 10.
            int totalPins = 0;
            for (int i = MAX_BOWL - 3; i < MAX_BOWL; i++) {
                totalPins += pins[i];
                if (totalPins > 10) return true;
                if (totalPins == 10) totalPins = 0;
            }
            //Finally, the 21 (bowl = 20) can only take place if the first two of the last
            //frame sum to ten pins
            return (pins[MAX_BOWL - 1] > 0 && pins[MAX_BOWL - 2] + pins[MAX_BOWL - 3] < 10);
        }

        private Boolean isStrikeBeforeLastFrame(int bowl) {
            //true iff 10 pins AND first bowl of frame (bowl is even) AND bowl is not in last frame
            return (pins[bowl] == 10 && bowl % 2 == 0 && !isLastFrame(bowl));
        }

        private Boolean isSpareBeforeLastFrame(int bowl) {
            //true iff frame has 10 pins AND second bowl of frame (bowl is odd) AND bowl is not in last frame 
            //check that bowl is odd first to avoid IndexOutOfRangeException when bowl = 0
            return (bowl % 2 == 1 && pins[bowl] + pins[bowl - 1] == 10 && !isLastFrame(bowl));
        }

    }
}

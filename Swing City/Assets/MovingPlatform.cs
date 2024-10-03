using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool loop; // whether platform goes back and forth between points or just respawns at the start point
    public int spd; // how fast the platform moves
    public Vector3 moveDir; // the direction the platform is currently moving
    public Vector3 startP; // the starting position of the platform
    public Vector3 endP; // the ending position
    public Vector3 curP; // the current point to move to
    public Transform[] points; // in case we ever wanna do complex moving platforms
    bool arrDir; // the direction the point array is being looped through (true is ->, false is <-)
    int curInd; // the current index corresponding to curP

    void Start() {
        int p = transform.GetSiblingIndex() + 1;
        Transform pObj = transform.parent.GetChild(p);
        points = new Transform[pObj.childCount];
        for(int i = 0; i < pObj.childCount; i++) points[i] = pObj.GetChild(i);
        init();
    }

    void init() {
        startP = points[0].position; // startP is the first child, so automatically grab that
        endP = points[points.Length-1].position; // ditto for endP but at the last position
        curInd = 1; // current point to move to is the second child
        curP = points[curInd].position;  // get that child
        arrDir = true; // loop forwards through array
        transform.position = startP; // set position to starting point
        moveDir = (curP - transform.position).normalized; // set direction
    }

    void Update() {
        if(Vector3.Distance(curP, transform.position) < 0.1f) { // if close enough to the current point
            if(curP == endP || curP == startP) { // if at the end or start 
                if(loop) { // if looping
                    arrDir = !arrDir; //  flip the array direction
                    moveArr(arrDir); // increment arrDir (so that we're not on endP or startP)
                    curP = points[curInd].position; // get the next point to move to
                    moveDir = (curP-transform.position).normalized; // and get the direction
                }
                else {
                    init(); // if not looping, just reset the platform
                }
            }
            else { // if not at either end (meaning we're at a midpoint)
                moveArr(arrDir); // move to the next point
                print(curInd);
                curP = points[curInd].position; // get that point
                moveDir = (curP-transform.position).normalized; // and get the direction
            }
        }
        else { // otherwise move in the necessary direction
            transform.Translate(moveDir * Time.deltaTime * spd);
        }
    }

    void moveArr(bool d) { // input false for subtraction, true for addition
        curInd = d ? curInd+1 : curInd-1;
    }
}

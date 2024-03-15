class SkylineProblem
{

//https://leetcode.com/problems/the-skyline-problem/description/
/*
A city's skyline is the outer contour of the silhouette formed by all the buildings in that city when viewed from a distance. Given the locations and heights of all the buildings, return the skyline formed by these buildings collectively.

The geometric information of each building is given in the array buildings where buildings[i] = [lefti, righti, heighti]:

lefti is the x coordinate of the left edge of the ith building.
righti is the x coordinate of the right edge of the ith building.
heighti is the height of the ith building.
You may assume all buildings are perfect rectangles grounded on an absolutely flat surface at height 0.

The skyline should be represented as a list of "key points" sorted by their x-coordinate in the form [[x1,y1],[x2,y2],...]. Each key point is the left endpoint of some horizontal segment in the skyline except the last point in the list, which always has a y-coordinate 0 and is used to mark the skyline's termination where the rightmost building ends. Any ground between the leftmost and rightmost buildings should be part of the skyline's contour.

Note: There must be no consecutive horizontal lines of equal height in the output skyline. For instance, [...,[2 3],[4 5],[7 5],[11 5],[12 7],...] is not acceptable; the three lines of height 5 should be merged into one in the final output as such: [...,[2 3],[4 5],[12 7],...] 
*/
    public static IList<IList<int>> GetSkyline(int[][] buildings) {
        IList<IList<int>> result = [];

        int rightmost = 0;
        for(int i = 0; i < buildings.Length; i++)
        {
            //each building is determined by three points
            int[] points = buildings[i];

            //x1 & x2 determine the values on the x axis where the left and rightmost end of the building lie
            int x1 = points[0];
            int x2 = points[1];
            //y is the height of the building
            int y = points[2];

            //save for later
            if(rightmost < x2)
            {
                rightmost = x2;
            }

            //top left corner of our building and top right respectively.
            int[] left = [x1,y];
            int[] right = [x2,y];

            //these will hold the height of the tallest building where our x point lies between their x1 and x2 values.
            int tallestLeft = 0, tallestRight = 0;
            for(int j = 0; j < buildings.Length; j++)
            {
                if(j == i){
                    continue;
                }
                //look at the buildings x1 and x2 values, if left[0] (its x value) is between them compare to max height
                if(buildings[j][2] >= tallestLeft && left[0] <= buildings[j][1] && left[0] >= buildings[j][0])
                {
                    //if it's another left edge, and they're exactly the same, then include it. We'll remove duplicates at the end.
                    if((buildings[j][0] == left[0]) && left[1] == buildings[j][2])
                    {
                        //do nothing
                    }
                    else{
                        tallestLeft = buildings[j][2];
                    }
                    
                }

                //NOTE: we don't use = comparison, if it's on the border with another
                if(buildings[j][2] >= tallestRight && right[0] <= buildings[j][1] && right[0] >= buildings[j][0])
                {
                    //if the right edges are perfectly on line with eachother, and the other one is shorter (or same), we can ignore it so we don't count it as the intersect later
                    if((buildings[j][1] == right[0]) && right[1] >= buildings[j][2])
                    {
                        continue;
                    }
                    tallestRight = buildings[j][2];
                }
            }

            //if our left point is the tallest then it's one of our horizontal pieces
            if(tallestLeft < left[1])
            {
                result.Add(left.ToList());
            }

            //if our right is the tallest, then we don't want it, we want wherever it ends up intersecting with the next tallest overlapping building
            //also ignore 
            if(tallestRight < right[1])
            {
                result.Add([right[0], tallestRight]);
            }
        }
        //result.Add([rightmost, 0]);
        result = result.OrderBy((a) => a[0]).ToList(); //should likely already be in order, but make certain
        Dictionary<int, int> seen = [];

        IList<int> prior = [-1, -1];
        IList<IList<int>> final = [];
        foreach(var item in result)
        {
            if(prior[1] != item[1])
            {
                final.Add(item);
                prior = item;
            }
        }
        return final.ToList();
    }
}
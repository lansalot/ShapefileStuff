/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Catfood.Shapefile;
using System.Diagnostics;
using PiPDemo;

namespace ShapefileDemo
{
    class Program
    {

        //public bool ContainsPoint(Point point)
        //{
        //    do
        //    {
        //        if (Box.ContainsPoint(point) == false)
        //        {
        //            break;
        //        }

        //        bool result = false;
        //        int j = Points.Length - 1;
        //        for (int i = 0; i < Points.Length; i++)
        //        {
        //            if ((Points[i].Y < point.Y && Points[j].Y >= point.Y)
        //                || (Points[j].Y < point.Y && Points[i].Y >= point.Y))
        //            {
        //                if (Points[i].X +
        //                    ((point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) * (Points[j].X - Points[i].X))
        //                    < point.X)
        //                {
        //                    result = !result;
        //                }
        //            }

        //            j = i;
        //        }

        //        return result;
        //    }
        //    while (false);

        //    return false;
        //}


        static void Main(string[] args)
        {

            // construct shapefile with the path to the .shp file
            //using (Shapefile shapefile = new Shapefile(args[0]))
            /*
            After_harvest_2022-11_Acre.dbf
            After_harvest_2022-4_Acre.dbf
            After_harvest_2022-Behind_Cottages.dbf
            After_harvest_2022-East_Steading.dbf
            After_harvest_2022-Emilys_1.dbf
            After_harvest_2022-Emilys_2.dbf
            After_harvest_2022-Front_of_House.dbf
            After_harvest_2022-Jackies.dbf
            After_harvest_2022-Reids.dbf
             */
            // "C:\\users\\andre\\Downloads\\ex3 reprojected to WGS 84\\ex3 reprojected to WGS 84.shp"
            // using (Shapefile shapefile = new Shapefile("C:\\users\\andre\\Downloads\\ex3 reprojected to WGS 84\\ex3 reprojected to WGS 84.shp"))
            AOGShapeFile aogshapefile = new AOGShapeFile("C:\\Users\\andre\\Downloads\\AOGStuff\\shapes\\After_harvest_2022-Jackies.shp");

            aogshapefile.ReadShapeFile();
            Debug.WriteLine("");
            //discarding feature 4.0023,57.7549 as outside of bounding box L:-4.034981 R: -4.032094 B: 57.756334 :T57.755213
            // wait a minute - are these boxes UPSIDE DOWN????
            aogshapefile.FindValueAtPoint(57.756335 , -4.033);
            Debug.WriteLine("!");
        }
    }
}

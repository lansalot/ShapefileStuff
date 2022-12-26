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
            using (Shapefile shapefile = new Shapefile("C:\\users\\andre\\Downloads\\ex3 reprojected to WGS 84\\ex3 reprojected to WGS 84.shp"))

            {
                //Console.WriteLine("ShapefileDemo Dumping {0}", args[0]);
                Console.WriteLine();

                // a shapefile contains one type of shape (and possibly null shapes)
                Console.WriteLine("Type: {0}, Shapes: {1:n0}", shapefile.Type, shapefile.Count);

                // a shapefile also defines a bounding box for all shapes in the file
                Console.WriteLine("Bounds: {0},{1} -> {2},{3}",
                    shapefile.BoundingBox.Left,
                    shapefile.BoundingBox.Top,
                    shapefile.BoundingBox.Right,
                    shapefile.BoundingBox.Bottom);
                Console.WriteLine();

                // enumerate all shapes

                PointD pointToFind = new PointD(-4.002800, 57.75160);


                foreach (Shape shape in shapefile)
                {
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Shape {0:n0}, Type {1}", shape.RecordNumber, shape.Type);

                    // each shape may have associated metadata
                    string[] metadataNames = shape.GetMetadataNames();
                    if (metadataNames != null)
                    {
                        Console.WriteLine("Metadata:");
                        foreach (string metadataName in metadataNames)
                        {
                            Console.WriteLine("{0}={1} ({2})", metadataName, shape.GetMetadata(metadataName), shape.DataRecord.GetDataTypeName(shape.DataRecord.GetOrdinal(metadataName)));
                        }
                        Console.WriteLine();
                    }

                    // point to test for intersection is;
                    // -4.002800, 57.75160

                    // -4.002904, 57.75142
                    // -4.002601, 57.751564
                    // -4.002247, 57.751665

                    // cast shape based on the type
                    switch (shape.Type)
                    {
                        case ShapeType.Point:
                            // a point is just a single x/y point
                            ShapePoint shapePoint = shape as ShapePoint;
                            Console.WriteLine("Point={0},{1}", shapePoint.Point.X, shapePoint.Point.Y);
                            break;

                        case ShapeType.Polygon:
                            // a polygon contains one or more parts - each part is a list of points which
                            // are clockwise for boundaries and anti-clockwise for holes 
                            // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                            //if (shape.DataRecord) { }
                            ShapePolygon shapePolygon = shape as ShapePolygon;
                            if (shapePolygon.BoundingBox.Left <= pointToFind.X && shapePolygon.BoundingBox.Right >= pointToFind.X &&
                                    shapePolygon.BoundingBox.Top <= pointToFind.Y && shapePolygon.BoundingBox.Bottom >= pointToFind.Y)
                            {
                                Console.WriteLine("possible candidate with polygon bounding box");
                                Console.WriteLine("Bounds: {0},{1} -> {2},{3}",
                                    shapePolygon.BoundingBox.Left,
                                    shapePolygon.BoundingBox.Top,
                                    shapePolygon.BoundingBox.Right,
                                    shapePolygon.BoundingBox.Bottom);
                            }
                            foreach (PointD[] part in shapePolygon.Parts)
                            {
                                Console.WriteLine("Polygon part:");
                                foreach (PointD point in part)
                                {
                                    Console.WriteLine("{0}, {1}", point.X, point.Y);
                                }
                                Console.WriteLine();
                            }
                            break;

                        default:
                            // and so on for other types...
                            break;
                    }

                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine();
                }

            }

            Console.WriteLine("Done");
            Console.WriteLine();
        }
    }
}

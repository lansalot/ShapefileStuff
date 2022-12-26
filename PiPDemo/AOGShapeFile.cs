using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfood.Shapefile;
using System.Diagnostics;

namespace PiPDemo
{
    public class AOGShapeFile
    {
        public int Polygoncount;
        public List<ShapePolygon> ShapePolygons;
        public List<object> dbfValues;
        public string[] ShapeFields;
        private string _SHPFileName;

        private struct Point
        {
            double lat, lon;
        }

        public AOGShapeFile(string SHPFileName)
        {
            // construct empty
            ShapePolygons = new List<ShapePolygon>();
            dbfValues = new List<Object>();
            _SHPFileName = SHPFileName;
        }

        public void ReadShapeFile()
        {
            using (Shapefile shapefile = new Shapefile(_SHPFileName))
            {
                var _polygons = new List<ShapePolygon>();
                var _dbfValues = new List<Object>();
                foreach (Shape shape in shapefile)
                {
                    Polygoncount++;
                    switch (shape.Type)
                    {
                        case ShapeType.Polygon:
                            ShapePolygon shapePolygon = shape as ShapePolygon;
                            //if (shapePolygon.BoundingBox.Left <= pointToFind.X && shapePolygon.BoundingBox.Right >= pointToFind.X &&
                            //        shapePolygon.BoundingBox.Top <= pointToFind.Y && shapePolygon.BoundingBox.Bottom >= pointToFind.Y)
                                _polygons.Add(shapePolygon);
                            break;

                        default:
                            // and so on for other types...
                            break;
                    }
                    //(ShapePolygon)shape.
                    _dbfValues.Add(shape.DataRecord.GetValue(0)); // hard code the first value in
                    Debug.WriteLine(shape.DataRecord.GetValue(0));
                }

                
                ShapePolygons = _polygons;
                dbfValues = _dbfValues;

                #region "Legacy"
                //switch (shape.Type)
                //{
                //    case ShapeType.Polygon:
                // a polygon contains one or more parts - each part is a list of points which
                // are clockwise for boundaries and anti-clockwise for holes 
                // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                //ShapePolygon shapePolygon = shape as ShapePolygon;


                //foreach (PointD[] part in shapePolygon.Parts)
                //{
                //    // new value? new colour
                //    // got to work out the field TYPE!!!
                //    // and yes, only got 2 so far https://www.dbase.com/Knowledgebase/INT/db7_file_fmt.htm
                //    object dbfValue = null;
                //    switch ((string)shape.DataRecord[(string)cbShapeField.SelectedItem].GetType().Name)
                //    {
                //        case "String":
                //            dbfValue = (string)shape.DataRecord.GetValue(shape.DataRecord.GetOrdinal((string)cbShapeField.SelectedItem));
                //            break;
                //        case "Double":
                //            dbfValue = (double)shape.DataRecord.GetValue(shape.DataRecord.GetOrdinal((string)cbShapeField.SelectedItem));
                //            break;
                //        default:
                //            break;
                //    }
                //}
                //break;

                //default:
                //    // not polygon? don't care
                //    break;
                //} // switch
            } //foreach
            #endregion
        } //using

        //private bool isPointInPoly(Point point, ShapePolygon poly)
        //{
        //    bool result = false;

        //    foreach (PointD polypoint in poly.Parts)
        //    {

        //        if ((((PointD)poly.Parts[i]).X < point.Y && Points[j].Y >= point.Y)
        //            || (Points[j].Y < point.Y && Points[i].Y >= point.Y))
        //        {
        //            if (Points[i].X +
        //                ((point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) * (Points[j].X - Points[i].X))
        //                < point.X)
        //            {
        //                result = !result;
        //            }
        //        }

        //        j = i;
        //    }

        //    return false;
        //}
        public bool FindValueAtPoint(double latitude, double longitude)
        {
            foreach (ShapePolygon poly in ShapePolygons )
            {
                // quick filter by BoundingBox - if point not in it, no point checking
                // it APPEARS Bottom and Top are the wrong way round?
                if ((longitude >= poly.BoundingBox.Left && longitude <= poly.BoundingBox.Right) && (latitude >= poly.BoundingBox.Top && latitude <= poly.BoundingBox.Bottom))
                {
                    Debug.WriteLine(String.Format("feature {0},{1} appears to be inside bounding box L:{2} R:{3} B:{4} T:{5}",
                        latitude, longitude,
                        poly.BoundingBox.Left, poly.BoundingBox.Right, poly.BoundingBox.Bottom, poly.BoundingBox.Top));
                }
                else
                {
                    Debug.WriteLine(String.Format("discarding feature {0},{1} as outside of bounding box L:{2} R:{3} B:{4} T:{5}",
                        latitude, longitude,
                        poly.BoundingBox.Left, poly.BoundingBox.Right, poly.BoundingBox.Bottom, poly.BoundingBox.Top));
                }
            }
            
            return (true);
        }
    }
}


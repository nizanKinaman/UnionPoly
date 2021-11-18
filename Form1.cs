using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnionPoly
{
    public partial class Form1 : Form
    {
        List<List<Point>> figures = new List<List<Point>>();
        List<Point> points = new List<Point>();
        static Bitmap bmp = new Bitmap(1100, 1100);
        public Graphics g = Graphics.FromImage(bmp);
        Pen pen = new Pen(Color.Black, 4f);
        static Bitmap bmp_new = new Bitmap(1100, 1100);
        public Graphics g_new = Graphics.FromImage(bmp_new);
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            g.Clear(Color.White);
            bmp.SetPixel(e.X, e.Y, Color.Black);
            Point point = new Point(e.X, e.Y);
            points.Add(point);
            DrawPoints();
            //if(figures.Count == 1)
            //{
            //    MessageBox.Show(""+ InTriangle(figures[0], point));
            //    //MessageBox.Show(""+ IsPointInsidePolygon(figures[0], 3 ,point.X,point.Y));
            //    //MessageBox.Show("" + IsPointInsidePolygon(new List<Point> { new Point(1, 1), new Point(2, 1), new Point(1, 2), new Point(2, 2) }, 4, 1, 2));
            //}

        }
        public void DrawPoints()
        {
            //g.DrawLine(pen,new Point(1,1),new Point(100,0));
            //g.Clear(Color.White);
            foreach (var x in figures)
            {
                for (int i = 0; i < x.Count(); i++)
                {
                    if (i + 1 < x.Count())
                        g.DrawLine(pen, x[i], x[i + 1]);
                    else
                        g.DrawLine(pen, x[i], x[0]);
                }
            }
            for (int i = 0; i < points.Count(); i++)
            {
                if (i + 1 < points.Count())
                    g.DrawLine(pen, points[i], points[i + 1]);
                else
                    g.DrawLine(pen, points[i], points[0]);
            }
            pictureBox1.Image = bmp;
        }

        public void AddLastPoly()
        {
            if (points.Count() != 0)
            {
                List<Point> newpoint = new List<Point>();
                foreach (var x in points)
                {
                    newpoint.Add(x);
                }
                figures.Add(newpoint);
                points.Clear();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            AddLastPoly();
        }

        bool InFigure(List<Point> p, Point point)
        {
            //p = new List<Point> { new Point(1, 1), new Point(3, 1), new Point(3,3), new Point(1, 3) };
            //point = new Point(2, 2);
            //bool is_on_line = false;
            //for (int i = 0; i < p.Count; i++)
            //{
            //    double x = 0;
            //    if (p.Count - 1 == i)
            //    {
            //        x = (p[i].X - point.X) * (p[0].Y - p[i].Y) - (p[0].X - p[i].X) * (p[i].Y - point.Y);
            //    }
            //    else
            //        x = (p[i].X - point.X) * (p[i + 1].Y - p[i].Y) - (p[i + 1].X - p[i].X) * (p[i].Y - point.Y);
            //    if (x >= 0 && x <= 0)
            //        is_on_line = true;
            //}
            //if (is_on_line)
            //    return is_on_line;
            int npol = p.Count();
            bool c = false;
            for (int i = 0, j = npol - 1; i < npol; j = i++)
            {
                if ((((p[i].Y <= point.Y) && (point.Y <= p[j].Y)) || ((p[j].Y <= point.Y) && (point.Y <= p[i].Y))) &&
                  (((p[j].Y - p[i].Y) != 0) && (point.X >= ((p[j].X - p[i].X) * (point.Y - p[i].Y) / (p[j].Y - p[i].Y) + p[i].X))))
                    c = !c;
            }
            return c;
        }


        Point cross_point = new Point();
        bool cross(Point p1,Point p2, Point p3, Point p4)
        {
            float x1 = p1.X;
            float y1 = p1.Y;
            float x2 = p2.X;
            float y2 = p2.Y;
            float x3 = p3.X;
            float y3 = p3.Y;
            float x4 = p4.X;
            float y4 = p4.Y;
            float n;
            if (y2 - y1 != 0)
            {  // a(y)
                float q = (x2 - x1) / (y1 - y2);
                float sn = (x3 - x4) + (y3 - y4) * q; if (sn == 0) { return false; }  // c(x) + c(y)*q
                float fn = (x3 - x1) + (y3 - y1) * q;   // b(x) + b(y)*q
                n = fn / sn;
            }
            else
            {
                if ((y3 - y4) == 0) { return false; }  // b(y)
                n = (y3 - y1) / (y3 - y4);   // c(y)/b(y)
            }
            cross_point.X = (int)(x3 + (x4 - x3) * n);  // x3 + (-b(x))*n
            cross_point.Y = (int)(y3 + (y4 - y3) * n);  // y3 +(-b(y))*n
            return true;
        }
        bool is_cross(Point p1, Point p2, Point p3, Point p4)
        {
            double x1 = p1.X;
            double x2 = p2.X;
            double x3 = p3.X;
            double x4 = p4.X;

            double y1 = p1.Y;
            double y2 = p2.Y;
            double y3 = p3.Y;
            double y4 = p4.Y;

            double x0 = p3.X;
            double y0 = p3.Y;
            if (Math.Sign((x1 - p3.X) * (y2 - y1) - (x2 - x1) * (y1 - p3.Y)) == Math.Sign((x1 - p4.X) * (y2 - y1) - (x2 - x1) * (y1 - p4.Y)))
                return false;
            if (Math.Sign((x3 - p1.X) * (y4 - y3) - (x4 - x3) * (y3 - p1.Y)) == Math.Sign((x3 - p2.X) * (y4 - y3) - (x4 - x3) * (y3 - p2.Y)))
                return false;
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AddLastPoly();
            //Формирование правильного порядка фигур и ошибка в случае не пересечения
            List<Point> perhaps_not_united = new List<Point>();
            for (var i = 0; i < figures.Count - 1; i++)
            {
                int count_cross = 0;
                for (int z = 0; z <= i; z++) 
                    for (var j = 0; j < figures[z].Count; j++)
                        for (var k = 0; k < figures[i + 1].Count; k++)
                            if (is_cross(figures[z][j], figures[z][(j + 1) % figures[z].Count], figures[i + 1][k], figures[i + 1][(k + 1) % figures[(i + 1) % figures.Count].Count]))
                                count_cross++;

                if (count_cross == 0)
                {
                    var dummy = new List<Point>();
                    foreach (var x in figures[i + 1])
                    {
                        dummy.Add(x);
                    }
                    figures.Remove(figures[i + 1]);
                    figures.Add(dummy);
                    if (perhaps_not_united.Count == 0)
                        perhaps_not_united = dummy;
                    else
                        if (perhaps_not_united.SequenceEqual(dummy))
                    {
                        figures.Remove(figures[i + 1]);
                        //MessageBox.Show("Error");
                        //return;
                    }
                    i--;
                }
                else
                {
                    perhaps_not_united = new List<Point>();
                }
            }


            List<Point> result_figure = figures[0];

            for (int k = 1; k < figures.Count(); k++)
            {
                List<Point> first_figure = new List<Point>();

                for (var i = 0; i < result_figure.Count; i++)
                {
                    List<Point> cross_points = new List<Point>();
                    first_figure.Add(result_figure[i]);
                    for (var j = 0; j < figures[k].Count; j++)
                    {
                        cross(result_figure[i], result_figure[(i + 1) % result_figure.Count], figures[k][j], figures[k][(j + 1) % figures[k].Count]);

                        if (is_cross(result_figure[i], result_figure[(i + 1) % result_figure.Count], figures[k][j], figures[k][(j + 1) % figures[k].Count]))
                        {
                            if (cross_points.Count == 0)
                                cross_points.Add(new Point(cross_point.X, cross_point.Y));
                            else
                            if (cross_point.X > result_figure[i].X)
                                if (cross_point.X < cross_points[0].X)
                                    cross_points.Insert(0, new Point(cross_point.X, cross_point.Y));
                                else
                                    cross_points.Add(new Point(cross_point.X, cross_point.Y));
                            else
                                if (cross_point.X > cross_points[0].X)
                                cross_points.Insert(0, new Point(cross_point.X, cross_point.Y));
                            else
                                cross_points.Add(new Point(cross_point.X, cross_point.Y));

                            //g.DrawLine(pen, cross_point, new Point(10, 10));
                            //pictureBox1.Image = bmp;
                        }
                    }
                    foreach (var point in cross_points)
                        first_figure.Add(point);
                }
                //for(int k = 1;k<figures.Count();k++)
                //{
                List<Point> second_figure = new List<Point>();
                for (var i = 0; i < figures[k].Count; i++)
                {
                    List<Point> cross_points = new List<Point>();
                    second_figure.Add(figures[k][i]);
                    for (var j = 0; j < result_figure.Count; j++)
                    {
                        cross(result_figure[j], result_figure[(j + 1) % result_figure.Count], figures[k][i], figures[k][(i + 1) % figures[k].Count]);

                        if (is_cross(result_figure[j], result_figure[(j + 1) % result_figure.Count], figures[k][i], figures[k][(i + 1) % figures[k].Count]))
                        {
                            if (cross_points.Count == 0)
                                cross_points.Add(new Point(cross_point.X, cross_point.Y));
                            else
                            if (cross_point.X > figures[k][i].X)
                                if (cross_point.X < cross_points[0].X)
                                    cross_points.Insert(0, new Point(cross_point.X, cross_point.Y));
                                else
                                    cross_points.Add(new Point(cross_point.X, cross_point.Y));
                            else
                                if (cross_point.X > cross_points[0].X)
                                cross_points.Insert(0, new Point(cross_point.X, cross_point.Y));
                            else
                                cross_points.Add(new Point(cross_point.X, cross_point.Y));

                            //g.DrawLine(pen, cross_point, new Point(10, 10));
                            //pictureBox1.Image = bmp;
                        }
                    }
                    //if (cross_points.Count == 0)
                    //{
                    //    MessageBox.Show("Есть не пересекающиеся полигоны!");
                    //    return;
                    //}
                    foreach (var point in cross_points)
                        second_figure.Add(point);
                }
                //result_figure = new List<Point>();
                var new_figure = new List<Point>();


                //Point p = new Point(2, 2);
                //for (int i = 0; i < first_figure.Count(); i++)
                //{
                //    g.DrawLine(pen, first_figure[i], first_figure[(i + 1) % first_figure.Count()]);
                //    g.DrawLine(pen, p, first_figure[(i + 1) % first_figure.Count()]);
                //    g.DrawLine(pen, first_figure[i], p);

                //}
                //for (int i = 0; i < second_figure.Count(); i++)
                //{
                //    g.DrawLine(pen, second_figure[i], second_figure[(i + 1) % second_figure.Count()]);
                //    g.DrawLine(pen, p, second_figure[(i + 1) % second_figure.Count()]);
                //    g.DrawLine(pen, second_figure[i], p);
                //}
                //pictureBox1.Image = bmp;




                var dummy_first = new List<Point>();
                var dummy_second = new List<Point>();
                foreach (var x in first_figure)
                {
                    if (!InFigure(second_figure, x) || (first_figure.Contains(x) && second_figure.Contains(x)))
                    {
                        dummy_first.Add(x);
                        //first_figure.Remove(x);
                        //next_value = x;
                        //break;
                    }

                }
                foreach (var x in second_figure)
                {

                    if (!InFigure(first_figure, x) || (first_figure.Contains(x) && second_figure.Contains(x)))
                    {
                        dummy_second.Add(x);
                        //second_figure.Remove(x);
                        //next_value = x;
                        //break;
                    }
                }
                
                //while (!dummy_first.SequenceEqual(dummy_second))
                {
                    int turn = 1;
                    first_figure = new List<Point>();
                    second_figure = new List<Point>();
                    dummy_first.ForEach((item) => { first_figure.Add(new Point(item.X, item.Y)); });
                    dummy_second.ForEach((item) => { second_figure.Add(new Point(item.X, item.Y)); });
                    var next_value = new Point();
                    for (var l = 0; l < first_figure.Count; l++)
                    {
                        if (!second_figure.Contains(first_figure[l]))
                        {
                            next_value = first_figure[l];
                            break;
                        }
                    }
                    if(next_value.IsEmpty)
                    {
                        for (var l = 0; l < second_figure.Count; l++)
                        {
                            if (!first_figure.Contains(second_figure[l]))
                            {
                                next_value = second_figure[l];
                                turn = 2;
                                break;
                            }
                        }
                    }
                    var to_point = next_value;
                    
                    do
                    {
                        if (turn == 1)
                        {
                           
                            new_figure.Add(next_value);
                            if (first_figure.Contains(next_value) && second_figure.Contains(next_value))
                                turn = 2;
                            else
                                dummy_first.Remove(next_value);
                        }
                        else
                        {
                            new_figure.Add(next_value);
                            if (first_figure.Contains(next_value) && second_figure.Contains(next_value))
                                turn = 1;
                            else
                                dummy_second.Remove(next_value);
                        }
                        next_value = turn == 1 ? first_figure[(first_figure.FindIndex(h => h == next_value) + 1) % first_figure.Count] : second_figure[(second_figure.FindIndex(h => h == next_value) + 1) % second_figure.Count];
                    } while (next_value != to_point);
                }
                result_figure = new_figure;
            }
            figures.Clear();
            figures.Add(result_figure);
            g.Clear(Color.White);
            DrawPoints();


        }
    }
}

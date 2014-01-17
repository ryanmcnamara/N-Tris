using N_Tris.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace N_Tris
{
    public class Polyomino
    {
        public byte colorR;
        public byte colorG;
        public byte colorB;

        private bool evenWidth = false;
        private bool evenHeight = false;

        public int rotation
        {
            get;
            private set;
        }

        public HashSet<Vector2> Minos;

        public Polyomino(HashSet<Vector2> a)
        {
            Minos = new HashSet<Vector2>();
            foreach (Vector2 p in a)
            {
                Minos.Add(new Vector2( p.X, p.Y ));
            }
        }

        public Polyomino(HashBitArray a, int n)
        {
            Minos = new HashSet<Vector2>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (a[i * n + j])
                    {
                        Minos.Add(new Vector2(j, i));
                    }
                }
            }
        }

        public Polyomino(Polyomino prevPoly, Vector2 adj)
        {
            this.Minos = new HashSet<Vector2>();
            foreach (Vector2 v in prevPoly.Minos)
            {
                Minos.Add(new Vector2(v.X, v.Y));
            }
            Minos.Add(adj);
        }

        public Polyomino(Polyomino polyomino) : this(polyomino.Minos)
        {
            this.evenHeight = polyomino.evenHeight;
            this.evenWidth = polyomino.evenWidth;
            this.colorR = polyomino.colorR;
            this.colorG = polyomino.colorG;
            this.colorB = polyomino.colorB;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException("getHashCode not implemented for polyomino class");
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException("equals not implemented for polyomino class");
        }

        public void translateToOrigin()
        {
            int minx = int.MaxValue;
            int miny = int.MaxValue;
            foreach (Vector2 v in Minos)
            {
                minx = Math.Min(v.X, minx);
                miny = Math.Min(v.Y, miny);
            }
            HashSet<Vector2> newMinos = new HashSet<Vector2>();

            foreach (Vector2 v in Minos)
            {
                newMinos.Add(new Vector2(v.X - minx, v.Y - miny));
            }
            Minos = newMinos;
        }

        public void rotate90CC()
        {
            HashSet<Vector2> newMinos = new HashSet<Vector2>();
            foreach ( Vector2 v in Minos )
            {
                int temp = v.X;
                int x = -v.Y;
                int y = temp;
                if ( evenWidth && evenHeight )
                {
                    x++;
                }
                else if ( evenWidth )
                {
                    y--;
                }
                Vector2 p = new Vector2(x, y);
                newMinos.Add(p);
            }
            Minos = newMinos;
            rotation += 3;
            rotation %= 4;
        }
        public void rotate90C()
        {
            // slightly inefficient but should never ever be a problem
            for (int i = 0; i < 3; i++)
            {
                rotate90CC();
            }
        }
    

        public HashSet<Vector2> getAdj()
        {
            HashSet<Vector2> asdf = new HashSet<Vector2>();
            foreach (Vector2 v in Minos)
            {
                asdf.Add(new Vector2(v.X, v.Y));
            }
            Minos = asdf; //todo remove

            HashSet<Vector2> ret = new HashSet<Vector2>();
            foreach (Vector2 p in Minos)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 v = new Vector2(Constants.orthogonalDirs[i, 0] + p.X, Constants.orthogonalDirs[i, 1] + p.Y);
                    if (!Minos.Contains(v))
                    {
                        ret.Add(v);
                    }
                }
            }
            return ret;
        }

        public HashBitArray toBitArray()
        {
            translateToOrigin();
            int n = Minos.Count;
            HashBitArray ret = new HashBitArray(n * n);
            foreach (Vector2 v in Minos)
            {
                ret[v.Y * n + v.X] = true;
            }

            return ret;
        }

        /**
     * A piece is SRS normalized iff
     * 
     * its bounding square is as centered around the origin as possible 
     * the smallest rectangle the piece fits in is not taller than it is wide.
     * the bottom half of the bounding box does not have fewer Minos
     * there are not more negative pieces than there are positive pieces (in the y direction).
     * 
     * This method rotates / translates the mino so that it is SRS normalized.
     * 
     * A piece must be SRS normalized when it emerges into the playing field.
     */
        public void SRSNormalize()
        {
            // get width
            // get height
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach ( Vector2 p in Minos )
            {
                minX = Math.Min(minX, p.X );
                maxX = Math.Max( maxX, p.X );
                minY = Math.Min(minY, p.Y );
                maxY = Math.Max( maxY, p.Y );
            }
        
            int width = maxX - minX + 1;
            int height = maxY - minY + 1;
        
            if ( height > width )
            {
                rotate90CC();
                SRSNormalize();
                return;
            }
        
            // determine if bottom half and top half count
            int bottomCount = 0; 
            int topCount = 0;
            foreach ( Vector2 p in Minos )
            {
                if ( p.Y - minY <= height / 2 -1 ) 
                {
                    bottomCount++;
                }
                if ( maxY - p.Y <=  height / 2 - 1 )
                {
                    topCount++;
                }
            }
            if ( bottomCount < topCount )
            {
                // not allowed, so flip 180
                rotate90CC();
                rotate90CC();
                SRSNormalize();
                return;
            }
        
            evenWidth = width % 2 == 0;
            evenHeight = height % 2 == 0;
        
        
        
            int xTranslate = -( width - 1 ) / 2  - minX;
            int yTranslate = -( height - 1 ) / 2  - minY;

            HashSet<Vector2> newMinos = new HashSet<Vector2>();
            foreach ( Vector2 p in Minos )
            {
                newMinos.Add( new Vector2( p.X + xTranslate, p.Y + yTranslate ) );
            }

            Minos = newMinos;

            rotation = 0;
        }

        public bool isIPolyomino()
        {
            bool first = true;
            Vector2 target = new Vector2(0,0);
            foreach (Vector2 v in Minos)
            {
                if (first)
                {
                    first = false;
                    target = v;
                }
                if (v.X != target.X && v.Y != target.Y)
                {
                    return false;
                }
            }
            return true;
        }


        public Polyomino Clone()
        {
            return new Polyomino(this);
        }

        public void print()
        {
            foreach (Vector2 p in Minos)
            {
                Console.Write("(" + p.X + ", " + p.Y + "), ");
            }
            Console.WriteLine(Minos.GetHashCode());
        }

        public void drawPolyomino(Canvas canvas, Vector2 location, double minoSize, bool ghost)
        {
            foreach (Vector2 v in Minos)
            {
                Mino m = new Mino(v + location, this);
                m.drawMino(canvas, minoSize, ghost);
            }

            return;
        }

        public void drawPolyomino( Canvas canvas, double minoSize, double locx, double locy, bool ghost )
        {
            foreach (Vector2 v in Minos)
            {
                double x = v.X * minoSize + locx;
                double y = -v.Y * minoSize + locy;

                // todo slow garbage making
                Mino m = new Mino(new Vector2(int.MaxValue, int.MaxValue ), this) ;
                m.drawMino(canvas, minoSize, x, y, ghost);
            }

        }

        public void drawPolyomino(Canvas canvas, Rect r, bool ghost)
        {
            int minx = int.MaxValue;
            int miny = int.MaxValue;
            int maxx = int.MinValue;
            int maxy = int.MinValue;

            foreach (Vector2 v in Minos)
            {
                minx = Math.Min(minx, v.X);
                maxx = Math.Max(maxx, v.X);
                miny = Math.Min(miny, v.Y);
                maxy = Math.Max(maxy, v.Y);
            }

            double minoWidth = (r.Width) / (maxx - minx + 1);
            double minoHeight = (r.Height) / (maxy - miny + 1);


            double minoSize = r.Width / Minos.Count;

            // assume wider than tall
            double realWidth = minoSize * (maxx - minx + 1);
            double realHeight = minoSize * (maxy - miny + 1);

            double midx = ((double)minx + maxx) / 2;
            double midy = ((double)miny + maxy) / 2;

            midx *= minoSize;
            midy *= minoSize;

            double x = r.Left + r.Width / 2 - midx - minoSize / 2;
            double y = r.Top + r.Height / 2 + midy - minoSize / 2;

            drawPolyomino(canvas, minoSize, x, y, ghost);
            
        }
    }


}

﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;
using OpenTKFramework.Framework;

namespace _02 {
    internal static class Program {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            while ( true ) {
                new MainClass();
            }
        }
    }

    internal class MainClass : OpenTKFramework.MainClass {
        private struct Paket {
            public Point pos;
            public byte  value;

            public Paket(Point pos, byte value) {
                this.pos   = pos;
                this.value = value;
            }
        }

        private Queue<Paket> _update = new Queue<Paket>();

        private const int COLORS_I     = 40;
        private const int COLOR_STEP_I = 240 / COLORS_I;
        private const int COLOR_HALF   = 255 / 2;

        private       float   scale   = .1F;
        private       float   PS      = 20;
        private const int     maxSize = 10000;
        private       byte[,] areal   = new byte[maxSize, maxSize];
        private       int     w, h, w2, h2, ox, oy;


        #region Overrides of MainClass

        /// <inheritdoc />
        public override void Initialize(object sender, EventArgs e) { }

        /// <inheritdoc />
        public override void Shutdown(object sender, EventArgs e) { }

        /// <inheritdoc />
        public override void Render(object sender, FrameEventArgs e) {
            this.I.ClearScreen( Color.FromArgb( 1, 82, 82, 82 ) );
            this.Window.Title = "     ArraySize: " + maxSize + "     Position: {x: " + ox + ",y: " + oy + "}" + "     Moves: " + IntText( Moves ) + "     FPS: " + this.frameRate + "     Scale: " + this.scale + "     PixelSize: " + this.PS;

            //while ( this._update.Count > 0 ) {
            //    var p = this._update.Dequeue();
            //    if ( p.value == 0 ) continue;
            //
            //    var x = p.pos.X + this.ox;
            //    var y = p.pos.Y + this.oy;
            //    
            //    if ( this.scale * x < 0  || this.scale * y < 0 ) continue;
            //    if ( this.scale * x >= w || this.scale * y >= h ) continue;
            //    
            //    if ( x < 0        || y < 0 ) continue;
            //    if ( x >= maxSize || y >= maxSize ) continue;
            //    
            //    
            //    var po = new PointF( this.scale * x, this.scale * y );
            //    
            //    //if ( Math.Abs( ( (int) p.X - p.X ) ) < this.scale/2 || Math.Abs( ( (int) p.Y - p.Y ) ) < this.scale /2)
            //    this.I.DrawPoint( po, colorFromValue( p.value ) );
            //    //this.I.DrawRect( new RectangleF( p, new SizeF(1,1) ), colorFromValue( v ) );
            //}

            //
            //for ( int i = 0; i < this.w; i++ ) {
            //    for ( int j = 0; j < this.h; j++ ) {
            //        var x = i + this.ox;
            //        var y = j + this.oy;
            //        if ( this.scale * x < 0  || this.scale * y < 0 ) continue;
            //        if ( this.scale * x >= maxSize || this.scale * y >= maxSize ) continue;
            //
            //        var v = this.areal[(int) x, (int) y];
            //
            //        if ( v == 0 ) continue;
            //
            //        var p = new PointF( this.scale * x, this.scale * y );
            //        
            //        this.I.DrawPoint( p, colorFromValue( v ) );
            //    }
            //}

            //SizeF s = new SizeF( PS, PS);
            for ( float i = 0; i < maxSize; i += PS ) {
                for ( float j = 0; j < maxSize; j += PS ) {
                    var x = i + this.ox;
                    var y = j + this.oy;

                    if ( x < 0 || y < 0 ) continue;

                    if ( x >= maxSize || y >= maxSize ) break;

                    var v = this.areal[(int) i, (int) j];

                    if ( v == 0 ) continue;

                    if ( this.scale * x < 0 || this.scale * y < 0 ) continue;

                    if ( this.scale * x >= w || this.scale * y >= h ) break;

                    var p = new PointF( this.scale * x, this.scale * y );
                    var s = new SizeF( this.scale * ( x + this.PS ) - this.scale * x, this.scale * ( y + PS ) - this.scale * y );

                    //if ( Math.Abs( ( (int) p.X - p.X ) ) < .1 || Math.Abs( ( (int) p.Y - p.Y ) ) < .1 )
                    this.I.DrawRect( new RectangleF( p, s ), colorFromValue( v ) );
                    //this.I.DrawRect( new RectangleF( p, new SizeF(1,1) ), colorFromValue( v ) );
                }
            }
        }

        private Color colorFromValue(byte b) {
            return Color.FromArgb( (int) ( ( Math.Sin( b * COLOR_STEP_I ) + 1 ) * COLOR_HALF ), (int) ( ( Math.Cos( b * COLOR_STEP_I ) + 1 ) * COLOR_HALF ), (int) ( ( -Math.Sin( b * COLOR_STEP_I ) + 1 ) * COLOR_HALF ) );

            //if ( b == 0 ) return Color.Black;
            //if ( b == 1 ) return Color.Red;
            //if ( b == 2 ) return Color.Aqua;
            //if ( b == 3 ) return Color.Coral;
            //if ( b == 4 ) return Color.DarkOrchid;
            //if ( b == 5 ) return Color.DeepPink;
            //if ( b == 6 ) return Color.GreenYellow;
            //if ( b == 7 ) return Color.Green;

            // return Color.Blue;
        }

        /// <inheritdoc />
        public override void Update(object sender, FrameEventArgs e) {
            this.w  = this.Window.ClientSize.Width;
            this.h  = this.Window.ClientSize.Height;
            this.h2 = this.Window.ClientSize.Height / 2;
            this.w2 = this.Window.ClientSize.Width  / 2;
        }

        #endregion

        private static string IntText(long score) {
            const int l = 2;

            var s = score.ToString();

            if ( s.Length < l + l ) return s;

            var e = s.Length - l;

            return s.Substring( 0, l ) + "," + s.Substring( l, l ) + "E+" + e.ToString( "D3" ); // + "  # " + score;
        }

        private bool exit = true;

        private bool fe = true;

        List<Thread> _ts = new List<Thread>();

        public MainClass() {
            for ( int i = 0; i < maxSize; i++ ) {
                for ( int j = 0; j < maxSize; j++ ) {
                    this.areal[i, j] = 0;
                }
            }

            Create( new Size( 1000, 1000 ) );
            this.Window.Location = new Point( 300, 100 );

            //this.Window.WindowState = WindowState.Fullscreen;
            //this.Window.WindowBorder = WindowBorder.Hidden;
            //this.Window.Bounds = Screen.PrimaryScreen.Bounds;

            this.Window.VSync = VSyncMode.Off;

            this.Window.KeyDown += delegate(object s, KeyboardKeyEventArgs e) {
                switch (e.Key) {
                    case Key.Escape:
                        this.exit = false;
                        this.Window.Close();

                        break;

                    case Key.ControlLeft:
                        this.fe = !this.fe;

                        break;

                    case Key.F:
                        new Thread( ConvertToBitmap ) { ApartmentState = ApartmentState.STA }.Start();

                        break;
                }
            };

            this.Window.MouseWheel += delegate(object s, MouseWheelEventArgs a) {
                if ( fe ) {
                    this.PS += a.DeltaPrecise * .1F;
                    //Console.WriteLine( a.Delta + " | " + this.PS );
                }
                else {
                    this.scale += a.DeltaPrecise * .1F;
                    //Console.WriteLine( a.Delta + " | " + this.scale );
                }

                if ( this.scale <= 0 ) this.scale = 0.05F;
                if ( this.PS    <= .01 ) this.PS  = .01F;
            };
            bool down = false;
            this.Window.MouseDown += (sender, args) => down = true;
            this.Window.MouseUp   += (sender, args) => down = false;

            this.Window.MouseMove += delegate(object sender, MouseMoveEventArgs args) {
                if ( !down ) return;

                this.ox += (int) ( args.XDelta * this.PS );
                this.oy += (int) ( args.YDelta * this.PS );
            };

            this.Window.Closing += delegate {
                if ( this.exit ) Environment.Exit( 0 );
                else {
                    foreach ( var t in this._ts )
                        t.Abort();
                    this.areal = null;
                    GC.Collect();
                }
            };
            this.w  = this.Window.ClientSize.Width;
            this.h  = this.Window.ClientSize.Height;
            this.h2 = this.Window.ClientSize.Height / 2;
            this.w2 = this.Window.ClientSize.Width  / 2;

            this.mv.AddRange( Enumerable.Repeat( false, COLORS_I * 2 ) );

            Random r = new Random();

            for ( int i = 0; i < COLORS_I / 4; i++ ) {
                var v = r.NextDouble() > .5;
                Thread.Sleep( 1 );
                this.mv[r.Next( 0, COLORS_I )] = v;
                Thread.Sleep( 1 );
            }

            bool True = true;

            //this.mv[2] = true;
            //this.mv[8] = true;

            //this.mv[13] = true;
            //this.mv[21] = true;
            //this.mv[39] = true;
            //this.mv[40] = true;
            //this.mv[41] = true;
            //this.mv[75] = true;
            //this.mv[77] = true;
            //this.mv[78] = true;
            //this.mv[89] = true;

            //this.mv[4] = true;
            //this.mv[9] = true;
            //this.mv[10] = true;
            //this.mv[24] = true;
            //this.mv[30] = true;
            //this.mv[41] = true;
            //this.mv[52] = true;
            //this.mv[61] = true;
            //this.mv[64] = true;
            //this.mv[65] = true;
            //this.mv[70] = true;
            //this.mv[74] = true;
            //this.mv[78] = true;
            //this.mv[81] = true;

            //mv[10] = true;
            //mv[11] = true;
            //mv[13] = true;
            //mv[36] = true;
            //mv[38] = true;
            //
            //mv[1]  = True;
            //mv[2]  = True;
            //mv[10] = True;
            //mv[11] = True;
            //mv[13] = True;
            //mv[36] = True;
            //mv[37] = True;
            //mv[38] = True;

            //mv[1] = True; /// # R
            //mv[2] = True; 
            //mv[4] = True;  
            //mv[10] = True; /// # R
            //mv[11] = True; /// # R
            //mv[12] = True; /// # R
            //mv[13] = True; 
            //mv[17] = True;  
            //mv[21] = True;   
            //mv[36] = True; /// # R
            //mv[37] = True; /// # R
            //mv[38] = True; 

            //for ( int i = 0; i < 1; i++ ) {
            //    this._creeps.Add( new Creep( Direction.n, new Point( ( maxSize / 2 ) + (int) ( i * r.NextDouble() * 100 ), ( maxSize / 2 ) + (int) ( i * r.NextDouble() * 100 ) ) ) );
            //}

            const int creeps = 4;

            for ( int i = 1; i < creeps + 1; i++ ) {
                this._creeps.Add( new Creep( Direction.n, new Point( maxSize / ( creeps + 1 ) * i, maxSize / ( creeps + 1 ) * i ) ) );
            }

            Console.WriteLine( "setupDirections" );

            for ( var i = 0; i < COLORS_I; i++ ) {
                Console.WriteLine( "mv[" + i + "] = " + this.mv[i] + ";           /// # " + ( this.mv[i] ? "R" : "L" ) );
            }

            foreach ( var c in this._creeps ) {
                var t = new Thread( () => Work( c ) );
                t.Start();
                this._ts.Add( t );
            }

            Run();
        }

        private void ConvertToBitmap() {
            unsafe {
                var s = new SaveFileDialog { Filter = "*.png|*.png" };
                int len = maxSize * maxSize *4;

                if ( s.ShowDialog() != DialogResult.OK ) return;
            
                var bm = new Bitmap( maxSize, maxSize);

                //var p = Marshal.AllocHGlobal( len );
                byte[] arr = new byte[len];

                for ( int i = 0; i < maxSize; i++ ) {
                    for ( int j = 0; j < maxSize; j++ ) {
                        arr[i * maxSize        + j *4] = this.areal[i, j];
                        arr[i * maxSize + j *4 +1]     = 0;
                        arr[i * maxSize + j *4 +2]     = 0;
                        arr[i * maxSize + j *4 +3]     = 255;
                    }
                }

                //var data = bm.LockBits( new Rectangle( 0, 0, maxSize, maxSize ), ImageLockMode.ReadWrite, PixelFormat.Max );
            
                BitmapData BtmDt   = bm.LockBits( new Rectangle( 0, 0, bm.Width, bm.Height ), ImageLockMode.ReadWrite, bm.PixelFormat );
                IntPtr     pointer = BtmDt.Scan0;
                int        size    = Math.Abs( BtmDt.Stride ) * bm.Height;
                //byte[]     pixels  = new byte[size];
                //Marshal.Copy( pointer, pixels, 0, size );
            

                Marshal.Copy( arr, 0, pointer, size );
                bm.UnlockBits( BtmDt );

                //for ( int i = 0; i < maxSize; i++ ) {
                //    for ( int j = 0; j < maxSize; j++ ) {
                //        bm.SetPixel( i, j, colorFromValue( this.areal[i, j] ) );
                //    }
                //}

                bm.Save( s.FileName, ImageFormat.Png );
            }
        }

        private List<Creep> _creeps = new List<Creep>();

        public static long Moves = 0;


        private void Work(Creep c) {
            while ( true ) {
                if ( c.pos.X < 1 ) c.pos.X            = 1;
                if ( c.pos.Y < 1 ) c.pos.Y            = 1;
                if ( c.pos.Y >= maxSize - 1 ) c.pos.Y = maxSize - 2;
                if ( c.pos.X >= maxSize - 1 ) c.pos.X = maxSize - 2;
                WorkDirection( GetArealByCreep( c ), c );
                Moves++;
                //Thread.Sleep( 1 );
            }
        }

        byte GetArealByCreep(Creep c) {
            if ( c.pos.X < 1 ) c.pos.X            = 1;
            if ( c.pos.Y < 1 ) c.pos.Y            = 1;
            if ( c.pos.Y >= maxSize - 1 ) c.pos.Y = maxSize - 2;
            if ( c.pos.X >= maxSize - 1 ) c.pos.X = maxSize - 2;

            return this.areal[c.pos.X, c.pos.Y];
        }

        void SetArealByCreep(Creep c, byte value) {
            if ( c.pos.X < 1 ) c.pos.X            = 1;
            if ( c.pos.Y < 1 ) c.pos.Y            = 1;
            if ( c.pos.Y >= maxSize - 1 ) c.pos.Y = maxSize - 2;
            if ( c.pos.X >= maxSize - 1 ) c.pos.X = maxSize - 2;

            this.areal[c.pos.X, c.pos.Y] = value;
        }

        private List<bool> mv = new List<bool>();

        private void WorkDirection(byte value, Creep c) {
            bool icrese = true;

            bool dx = this.mv[value];

            if ( value >= COLORS_I - 1 ) {
                dx     = true;
                icrese = false;
                SetArealByCreep( c, 0 );
            }

            if ( icrese )
                SetArealByCreep( c, (byte) ( GetArealByCreep( c ) + 1 ) );

            switch (c.direction) {
                case Direction.n:
                    if ( dx ) {
                        c.pos.X++;
                        c.direction = Direction.o;
                    }
                    else {
                        c.pos.X--;
                        c.direction = Direction.w;
                    }

                    break;

                case Direction.s:
                    if ( !dx ) {
                        c.pos.X++;
                        c.direction = Direction.o;
                    }
                    else {
                        c.pos.X--;
                        c.direction = Direction.w;
                    }

                    break;

                case Direction.o:
                    if ( dx ) {
                        c.pos.Y++;
                        c.direction = Direction.s;
                    }
                    else {
                        c.pos.Y--;
                        c.direction = Direction.n;
                    }

                    break;

                case Direction.w:
                    if ( !dx ) {
                        c.pos.Y++;
                        c.direction = Direction.s;
                    }
                    else {
                        c.pos.Y--;
                        c.direction = Direction.n;
                    }

                    break;
                default: break;
            }

            //this._update.Enqueue( new paket(c.pos, this.areal[c.pos.X, c.pos.Y]) );
            //Console.WriteLine( "arnt: " + this.arnt );
        }

        private enum Direction {
            n, s, o, w
        }

        private class Creep {
            public Point     pos;
            public Direction direction;

            /// <inheritdoc />
            public Creep(Direction direction, Point pos) {
                this.direction = direction;
                this.pos       = pos;
            }
        }
    }
}
